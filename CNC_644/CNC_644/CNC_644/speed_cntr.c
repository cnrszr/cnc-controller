/*This file has been prepared for Doxygen automatic documentation generation.*/
/*! \file *********************************************************************
 *
 * \brief Linear speed ramp controller.
 *
 * Stepper motor driver, increment/decrement the position and outputs the
 * correct signals to stepper motor.
 *
 * - File:               speed_cntr.c
 * - Compiler:           IAR EWAAVR 4.11A
 * - Supported devices:  All devices with a 16 bit timer can be used.
 *                       The example is written for ATmega48
 * - AppNote:            AVR446 - Linear speed control of stepper motor
 *
 * \author               Atmel Corporation: http://www.atmel.com \n
 *                       Support email: avr@atmel.com
 *
 * $Name: RELEASE_1_0 $
 * $Revision: 1.2 $
 * $RCSfile: speed_cntr.c,v $
 * $Date: 2006/05/08 12:25:58 $
 *****************************************************************************/

#include <avr/io.h>
#include <avr/interrupt.h>
#include "global.h"
#include "sm_driver.h"
#include "speed_cntr.h"
#include "UART.h"

//! Cointains data for timer interrupt.
speedRampData srdZ;
speedRampData srdX;
speedRampData srdY;
/*! \brief Init of Timer/Counter1.
 *
 *  Set up Timer/Counter1 to use mode 1 CTC and
 *  enable Output Compare A Match Interrupt.
 */
void speed_cntr_Init_Timer1(void)
{
  // Tells what part of speed ramp we are in.
  srdZ.run_state = STOP;
  // Timer/Counter 1 in mode 4 CTC (Not running).
  TCCR1B = (1<<WGM12);
  // Timer/Counter 1 Output Compare A Match Interrupt enable.
  TIMSK1 = (1<<OCIE1A);
  
  srdX.run_state = STOP;
  TCCR0B = (1<<WGM02);
  TIMSK0 = (1<<OCIE0A);
  EightBitTempX = 0;
  
  srdY.run_state = STOP;
  TCCR2B = (1<<WGM22);
  TIMSK2 = (1<<OCIE2A);
  EightBitTempY = 0;  
}
/*! \brief Move the stepper motor a given number of steps.
 *
 *  Makes the stepper motor move the given number of steps.
 *  It accelrate with given accelration up to maximum speed and decelerate
 *  with given deceleration so it stops at the given step.
 *  If accel/decel is to small and steps to move is to few, speed might not
 *  reach the max speed limit before deceleration starts.
 *
 *  \param step  Number of steps to move (pos - CW, neg - CCW).
 *  \param accel  Accelration to use, in 0.01*rad/sec^2.
 *  \param decel  Decelration to use, in 0.01*rad/sec^2.
 *  \param speed  Max speed, in 0.01*rad/sec.
 */
void speed_cntr_SetupX(signed int step, unsigned int accel, unsigned int decel, unsigned int speed)
{
	EightBitTempX = 0;
	speed = min(speed, MAXSPEED);//don't exceed max speed
	MotorEnPortX |= (1<<MotorENX);
  //! Number of steps before we hit max speed.
  unsigned int max_s_lim;
  //! Number of steps before we must start deceleration (if accel does not hit max speed).
  unsigned int accel_lim;

  // Set direction from sign on step value.
  if(step < 0){
    srdX.dir = CCW;
    step = -step;
  }
  else{
    srdX.dir = CW;
  }

  // If moving only 1 step.
  if(step == 1){
    // Move one step...
    srdX.accel_count = -1;
    // ...in DECEL state.
    srdX.run_state = DECEL;
    // Just a short delay so main() can act on 'running'.
	
    srdX.step_delay = 1000;
    statusX.running = TRUE;
    OCR0A = 10;
    // Run Timer/Counter 1 with prescaler = 8.
    //TCCR0B |= ((0<<CS12)|(1<<CS11)|(0<<CS10));Moved to StartX()
  }
  // Only move if number of steps to move is not zero.
  else if(step != 0){
    // Refer to documentation for detailed information about these calculations.

    // Set max speed limit, by calc min_delay to use in timer.
    // min_delay = (alpha / tt)/ w
    srdX.min_delay = A_T_x100 / speed;

    // Set accelration by calc the first (c0) step delay .
    // step_delay = 1/tt * sqrt(2*alpha/accel)
    // step_delay = ( tfreq*0.676/100 )*100 * sqrt( (2*alpha*10000000000) / (accel*100) )/10000
    srdX.step_delay = (T1_FREQ_148 * sqrt(A_SQ / accel))/100;

    // Find out after how many steps does the speed hit the max speed limit.
    // max_s_lim = speed^2 / (2*alpha*accel)
    max_s_lim = (long)speed*speed/(long)(((long)A_x20000*accel)/100);
    // If we hit max speed limit before 0,5 step it will round to 0.
    // But in practice we need to move atleast 1 step to get any speed at all.
    if(max_s_lim == 0){
      max_s_lim = 1;
    }

    // Find out after how many steps we must start deceleration.
    // n1 = (n1+n2)decel / (accel + decel)
    accel_lim = ((long)step*decel) / (accel+decel);
    // We must accelrate at least 1 step before we can start deceleration.
    if(accel_lim == 0){
      accel_lim = 1;
    }

    // Use the limit we hit first to calc decel.
    if(accel_lim <= max_s_lim){
      srdX.decel_val = accel_lim - step;
    }
    else{
      srdX.decel_val = -((long)max_s_lim*accel)/decel;
    }
    // We must decelrate at least 1 step to stop.
    if(srdX.decel_val == 0){
      srdX.decel_val = -1;
    }

    // Find step to start decleration.
    srdX.decel_start = step + srdX.decel_val;

    // If the maximum speed is so low that we dont need to go via accelration state.
    if(srdX.step_delay <= srdX.min_delay){
      srdX.step_delay = srdX.min_delay;
      srdX.run_state = RUN;
    }
    else{
      srdX.run_state = ACCEL;
    }

    // Reset counter.
    srdX.accel_count = 0;
    statusX.running = TRUE;
    OCR0A = 10;
    // Set Timer/Counter to divide clock by 8
    //TCCR0B |= ((0<<CS12)|(1<<CS11)|(0<<CS10));//Moved to StartX();
  }
  EightBitTempX = srdX.step_delay;
  
}
void speed_cntr_SetupY(signed int step, unsigned int accel, unsigned int decel, unsigned int speed)
{
	EightBitTempY = 0;
	speed = min(speed, MAXSPEED);//don't exceed max speed
	MotorEnPortY |= (1<<MotorENY);
  //! Number of steps before we hit max speed.
  unsigned int max_s_lim;
  //! Number of steps before we must start deceleration (if accel does not hit max speed).
  unsigned int accel_lim;

  // Set direction from sign on step value.
  if(step < 0){
    srdY.dir = CCW;
    step = -step;
  }
  else{
    srdY.dir = CW;
  }

  // If moving only 1 step.
  if(step == 1){
    // Move one step...
    srdY.accel_count = -1;
    // ...in DECEL state.
    srdY.run_state = DECEL;
    // Just a short delay so main() can act on 'running'.
	
    srdY.step_delay = 1000;
    statusY.running = TRUE;
    OCR2A = 10;
    // Run Timer/Counter 1 with prescaler = 8.
    /*TCCR2B |= ((0<<CS22)|(1<<CS21)|(0<<CS20));*///Moved to StartY();
  }
  // Only move if number of steps to move is not zero.
  else if(step != 0){
    // Refer to documentation for detailed information about these calculations.

    // Set max speed limit, by calc min_delay to use in timer.
    // min_delay = (alpha / tt)/ w
    srdY.min_delay = A_T_x100 / speed;

    // Set accelration by calc the first (c0) step delay .
    // step_delay = 1/tt * sqrt(2*alpha/accel)
    // step_delay = ( tfreq*0.676/100 )*100 * sqrt( (2*alpha*10000000000) / (accel*100) )/10000
    srdY.step_delay = (T1_FREQ_148 * sqrt(A_SQ / accel))/100;

    // Find out after how many steps does the speed hit the max speed limit.
    // max_s_lim = speed^2 / (2*alpha*accel)
    max_s_lim = (long)speed*speed/(long)(((long)A_x20000*accel)/100);
    // If we hit max speed limit before 0,5 step it will round to 0.
    // But in practice we need to move atleast 1 step to get any speed at all.
    if(max_s_lim == 0){
      max_s_lim = 1;
    }

    // Find out after how many steps we must start deceleration.
    // n1 = (n1+n2)decel / (accel + decel)
    accel_lim = ((long)step*decel) / (accel+decel);
    // We must accelrate at least 1 step before we can start deceleration.
    if(accel_lim == 0){
      accel_lim = 1;
    }

    // Use the limit we hit first to calc decel.
    if(accel_lim <= max_s_lim){
      srdY.decel_val = accel_lim - step;
    }
    else{
      srdY.decel_val = -((long)max_s_lim*accel)/decel;
    }
    // We must decelrate at least 1 step to stop.
    if(srdY.decel_val == 0){
      srdY.decel_val = -1;
    }

    // Find step to start decleration.
    srdY.decel_start = step + srdY.decel_val;

    // If the maximum speed is so low that we dont need to go via accelration state.
    if(srdY.step_delay <= srdY.min_delay){
      srdY.step_delay = srdY.min_delay;
      srdY.run_state = RUN;
    }
    else{
      srdY.run_state = ACCEL;
    }

    // Reset counter.
    srdY.accel_count = 0;
    statusY.running = TRUE;
    OCR2A = 10;
    // Set Timer/Counter to divide clock by 8
    //TCCR2B |= ((0<<CS22)|(1<<CS21)|(0<<CS20));Moved to StartY();
  }
  EightBitTempY = srdY.step_delay;
}
void speed_cntr_SetupZ(signed int step, unsigned int accel, unsigned int decel, unsigned int speed)
{
	speed = min(speed, MAXSPEED);//don't exceed max speed
	MotorEnPortZ |= (1<<MotorENZ);
  //! Number of steps before we hit max speed.
  unsigned int max_s_lim;
  //! Number of steps before we must start deceleration (if accel does not hit max speed).
  unsigned int accel_lim;

  // Set direction from sign on step value.
  if(step < 0){
    srdZ.dir = CCW;
    step = -step;
  }
  else{
    srdZ.dir = CW;
  }

  // If moving only 1 step.
  if(step == 1){
    // Move one step...
    srdZ.accel_count = -1;
    // ...in DECEL state.
    srdZ.run_state = DECEL;
    // Just a short delay so main() can act on 'running'.
    srdZ.step_delay = 1000;
    statusZ.running = TRUE;
    OCR1A = 10;
    // Run Timer/Counter 1 with prescaler = 8.
    //TCCR1B |= ((0<<CS12)|(1<<CS11)|(0<<CS10));//Moved to StartZ();
  }
  // Only move if number of steps to move is not zero.
  else if(step != 0){
    // Refer to documentation for detailed information about these calculations.

    // Set max speed limit, by calc min_delay to use in timer.
    // min_delay = (alpha / tt)/ w
    srdZ.min_delay = A_T_x100 / speed;

    // Set accelration by calc the first (c0) step delay .
    // step_delay = 1/tt * sqrt(2*alpha/accel)
    // step_delay = ( tfreq*0.676/100 )*100 * sqrt( (2*alpha*10000000000) / (accel*100) )/10000
    srdZ.step_delay = (T1_FREQ_148 * sqrt(A_SQ / accel))/100;

    // Find out after how many steps does the speed hit the max speed limit.
    // max_s_lim = speed^2 / (2*alpha*accel)
    max_s_lim = (long)speed*speed/(long)(((long)A_x20000*accel)/100);
    // If we hit max speed limit before 0,5 step it will round to 0.
    // But in practice we need to move atleast 1 step to get any speed at all.
    if(max_s_lim == 0){
      max_s_lim = 1;
    }

    // Find out after how many steps we must start deceleration.
    // n1 = (n1+n2)decel / (accel + decel)
    accel_lim = ((long)step*decel) / (accel+decel);
    // We must accelrate at least 1 step before we can start deceleration.
    if(accel_lim == 0){
      accel_lim = 1;
    }

    // Use the limit we hit first to calc decel.
    if(accel_lim <= max_s_lim){
      srdZ.decel_val = accel_lim - step;
    }
    else{
      srdZ.decel_val = -((long)max_s_lim*accel)/decel;
    }
    // We must decelrate at least 1 step to stop.
    if(srdZ.decel_val == 0){
      srdZ.decel_val = -1;
    }

    // Find step to start decleration.
    srdZ.decel_start = step + srdZ.decel_val;

    // If the maximum speed is so low that we dont need to go via accelration state.
    if(srdZ.step_delay <= srdZ.min_delay){
      srdZ.step_delay = srdZ.min_delay;
      srdZ.run_state = RUN;
    }
    else{
      srdZ.run_state = ACCEL;
    }

    // Reset counter.
    srdZ.accel_count = 0;
    statusZ.running = TRUE;
    OCR1A = 10;
    // Set Timer/Counter to divide clock by 8
    //TCCR1B |= ((0<<CS12)|(1<<CS11)|(0<<CS10));
  }
}

void StartX(void)
{
	TCCR0B |= ((0<<CS12)|(1<<CS11)|(0<<CS10));
}
void StartY(void)
{
	TCCR2B |= ((0<<CS22)|(1<<CS21)|(0<<CS20));
}
void StartZ(void)
{
	TCCR1B |= ((0<<CS12)|(1<<CS11)|(0<<CS10));
}
/*! \brief Timer/Counter1 Output Compare A Match Interrupt.
 *
 *  Timer/Counter1 Output Compare A Match Interrupt.
 *  Increments/decrements the position of the stepper motor
 *  exept after last position, when it stops.
 *  The \ref step_delay defines the period of this interrupt
 *  and controls the speed of the stepper motor.
 *  A new step delay is calculated to follow wanted speed profile
 *  on basis of accel/decel parameters.
 */
ISR(TIMER0_COMPA_vect )
{
	// Holds next delay period.
	unsigned int new_step_delayX;
	// Remember the last step delay used when accelerating.
	static int last_accel_delayX;
	// Counting steps when moving.
	static unsigned int step_countX = 0;
	// Keep track of remainder from new_step-delay calculation to increase accuracy
	static unsigned int restX = 0;
	
	if(EightBitTempX > 255)
	{
		EightBitTempX -= 255;
		OCR0A = 255;	
	}
	else
	{
		EightBitTempX = srdX.step_delay;
		if(EightBitTempX > 255)
		{
			OCR0A = 255;	
		}				
		else
		{
			OCR0A = EightBitTempX;	
		}				
		switch(srdX.run_state) 
		{
			case STOP:
				MotorEnPortX &= ~(1<<MotorENX);
				step_countX = 0;
				restX = 0;
				// Stop Timer/Counter 1.
				TCCR0B &= ~((1<<CS22)|(1<<CS21)|(1<<CS20));
				statusX.running = FALSE;
				break;

			case ACCEL:
				smX_driver_StepCounter(srdX.dir);
				step_countX++;
				srdX.accel_count++;
				new_step_delayX = srdX.step_delay - (((2 * (long)srdX.step_delay) + restX)/(4 * srdX.accel_count + 1));
				restX = ((2 * (long)srdX.step_delay)+restX)%(4 * srdX.accel_count + 1);
				// Check if we should start decelration.
				if(step_countX >= srdX.decel_start) 
				{
					srdX.accel_count = srdX.decel_val;
					srdX.run_state = DECEL;
				}
			// Chech if we hitted max speed.
				else if(new_step_delayX <= srdX.min_delay) 
				{
					last_accel_delayX = new_step_delayX;
					new_step_delayX = srdX.min_delay;
					restX = 0;
					srdX.run_state = RUN;
				}
				break;

			case RUN:
				smX_driver_StepCounter(srdX.dir);
				step_countX++;
				new_step_delayX = srdX.min_delay;
				// Chech if we should start decelration.
				if(step_countX >= srdX.decel_start) 
				{
					srdX.accel_count = srdX.decel_val;
					// Start decelration with same delay as accel ended with.
					new_step_delayX = last_accel_delayX;
					srdX.run_state = DECEL;
				}
				break;

			case DECEL:
				smX_driver_StepCounter(srdY.dir);
				step_countX++;
				srdX.accel_count++;
				new_step_delayX = srdX.step_delay - (((2 * (long)srdX.step_delay) + restX)/(4 * srdX.accel_count + 1));
				restX = ((2 * (long)srdX.step_delay)+restX)%(4 * srdX.accel_count + 1);
				// Check if we at last step
				if(srdX.accel_count >= 0)
				{
					srdX.run_state = STOP;
				}
				break;
		}
		srdX.step_delay = new_step_delayX;
	}  
}
ISR(TIMER1_COMPA_vect )
{
	PORTA |= (1<<PA1);
  // Holds next delay period.
  unsigned int new_step_delay;
  // Remember the last step delay used when accelrating.
  static int last_accel_delay;
  // Counting steps when moving.
  static unsigned int step_count = 0;
  // Keep track of remainder from new_step-delay calculation to incrase accurancy
  static unsigned int rest = 0;

  OCR1A = srdZ.step_delay;

  switch(srdZ.run_state) 
  {
    case STOP:
	  MotorEnPortZ &= ~(1<<MotorENZ);
      step_count = 0;
      rest = 0;
      // Stop Timer/Counter 1.
      TCCR1B &= ~((1<<CS12)|(1<<CS11)|(1<<CS10));
      statusZ.running = FALSE;
      break;

    case ACCEL:
      smZ_driver_StepCounter(srdZ.dir);
      step_count++;
      srdZ.accel_count++;
      new_step_delay = srdZ.step_delay - (((2 * (long)srdZ.step_delay) + rest)/(4 * srdZ.accel_count + 1));
      rest = ((2 * (long)srdZ.step_delay)+rest)%(4 * srdZ.accel_count + 1);
      // Check if we should start decelration.
      if(step_count >= srdZ.decel_start) {
        srdZ.accel_count = srdZ.decel_val;
        srdZ.run_state = DECEL;
      }
      // Chech if we hitted max speed.
      else if(new_step_delay <= srdZ.min_delay) {
        last_accel_delay = new_step_delay;
        new_step_delay = srdZ.min_delay;
        rest = 0;
        srdZ.run_state = RUN;
      }
      break;

    case RUN:
      smZ_driver_StepCounter(srdZ.dir);
      step_count++;
      new_step_delay = srdZ.min_delay;
      // Chech if we should start decelration.
      if(step_count >= srdZ.decel_start) {
        srdZ.accel_count = srdZ.decel_val;
        // Start decelration with same delay as accel ended with.
        new_step_delay = last_accel_delay;
        srdZ.run_state = DECEL;
      }
      break;

    case DECEL:
      smZ_driver_StepCounter(srdZ.dir);
      step_count++;
      srdZ.accel_count++;
      new_step_delay = srdZ.step_delay - (((2 * (long)srdZ.step_delay) + rest)/(4 * srdZ.accel_count + 1));
      rest = ((2 * (long)srdZ.step_delay)+rest)%(4 * srdZ.accel_count + 1);
      // Check if we at last step
      if(srdZ.accel_count >= 0){
        srdZ.run_state = STOP;
      }
      break;
  }
  srdZ.step_delay = new_step_delay;
  PORTA &= ~(1<<PA1);
}
ISR(TIMER2_COMPA_vect )
{
	// Holds next delay period.
	unsigned int new_step_delayY;
	// Remember the last step delay used when accelrating.
	static int last_accel_delayY;
	// Counting steps when moving.
	static unsigned int step_countY = 0;
	// Keep track of remainder from new_step-delay calculation to incrase accurancy
	static unsigned int restY = 0;
	
	if(EightBitTempY > 255)
	{
		EightBitTempY -= 255;
		OCR2A = 255;	
	}
	else
	{
		EightBitTempY = srdY.step_delay;
		if(EightBitTempY > 255)
		{
			OCR2A = 255;	
		}				
		else
		{
			OCR2A = EightBitTempY;	
		}				
		switch(srdY.run_state) 
		{
			case STOP:
				MotorEnPortY &= ~(1<<MotorENY);
				step_countY = 0;
				restY = 0;
				// Stop Timer/Counter 1.
				TCCR2B &= ~((1<<CS22)|(1<<CS21)|(1<<CS20));
				statusY.running = FALSE;
				break;

			case ACCEL:
				smY_driver_StepCounter(srdY.dir);
				step_countY++;
				srdY.accel_count++;
				new_step_delayY = srdY.step_delay - (((2 * (long)srdY.step_delay) + restY)/(4 * srdY.accel_count + 1));
				restY = ((2 * (long)srdY.step_delay)+restY)%(4 * srdY.accel_count + 1);
				// Check if we should start decelration.
				if(step_countY >= srdY.decel_start) 
				{
					srdY.accel_count = srdY.decel_val;
					srdY.run_state = DECEL;
				}
			// Chech if we hitted max speed.
				else if(new_step_delayY <= srdY.min_delay) 
				{
					last_accel_delayY = new_step_delayY;
					new_step_delayY = srdY.min_delay;
					restY = 0;
					srdY.run_state = RUN;
				}
				break;

			case RUN:
				smY_driver_StepCounter(srdY.dir);
				step_countY++;
				new_step_delayY = srdY.min_delay;
				// Chech if we should start decelration.
				if(step_countY >= srdY.decel_start) 
				{
					srdY.accel_count = srdY.decel_val;
					// Start decelration with same delay as accel ended with.
					new_step_delayY = last_accel_delayY;
					srdY.run_state = DECEL;
				}
				break;

			case DECEL:
				smY_driver_StepCounter(srdY.dir);
				step_countY++;
				srdY.accel_count++;
				new_step_delayY = srdY.step_delay - (((2 * (long)srdY.step_delay) + restY)/(4 * srdY.accel_count + 1));
				restY = ((2 * (long)srdY.step_delay)+restY)%(4 * srdY.accel_count + 1);
				// Check if we at last step
				if(srdY.accel_count >= 0)
				{
					srdY.run_state = STOP;
				}
				break;
		}
		srdY.step_delay = new_step_delayY;
	}  
}

/*! \brief Square root routine.
 *
 * sqrt routine 'grupe', from comp.sys.ibm.pc.programmer
 * Subject: Summary: SQRT(int) algorithm (with profiling)
 *    From: warwick@cs.uq.oz.au (Warwick Allison)
 *    Date: Tue Oct 8 09:16:35 1991
 *
 *  \param x  Value to find square root of.
 *  \return  Square root of x.
 */
static unsigned long sqrt(unsigned long x)
{
  register unsigned long xr;  // result register
  register unsigned long q2;  // scan-bit register
  register unsigned char f;   // flag (one bit)

  xr = 0;                     // clear result
  q2 = 0x40000000L;           // higest possible result bit
  do
  {
    if((xr + q2) <= x)
    {
      x -= xr + q2;
      f = 1;                  // set flag
    }
    else
	{
      f = 0;                  // clear flag
    }
    xr >>= 1;
    if(f)
	{
      xr += q2;               // test flag
    }
  }
  while(q2 >>= 2);          // shift twice
  if(xr < x)
  {
    return xr +1;             // add for rounding
  }
  else
  {
    return xr;
  }
}

/*! \brief Find minimum value.
 *
 *  Returns the smallest value.
 *
 *  \return  Min(x,y).
 */
unsigned int min(unsigned int x, unsigned int y)
{
  if(x < y){
    return x;
  }
  else{
    return y;
  }
}

