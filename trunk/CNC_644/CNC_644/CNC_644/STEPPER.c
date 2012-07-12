/*
 * STEPPER.c
 * C code to drive a stepper motor
 * Created: 08/07/2012 15:56:36
 *  Author: hslovett
 */ 
#define F_CPU 12000000
#include "STEPPER.h"
#include "Pin_Definitions.h"
#include <avr/io.h>
#include <avr/interrupt.h>
#include <util/delay.h>

//States
#define STOP 0
#define ACCELERATE 1
#define RUN 2
#define DECELERATE 3
uint8_t STATE;
//Local Variables
uint8_t StepPoint;
uint8_t ACCEL;
uint8_t DECEL;
uint8_t MIN_OCR0A;
uint8_t NUM_STEPS;
float alpha = 7.8;

//Constants
uint8_t StepsPerRound = 96;


//Step Sequences
uint8_t HalfSteps[8] = {0x08, 0x0A, 0x02, 0x06, 0x04, 0x05, 0x01, 0x09};
uint8_t FullSteps[4] = {0x08, 0x02, 0x04, 0x01};

void StepperInitialise(void)
{
	StepPoint = 0;//Initialise the stepper pointer
	ACCEL = 1;
	DECEL = 1;
	MIN_OCR0A = 100;
	NUM_STEPS = 0;
	STATE = STOP;
}

//Step up/down is for use with the timer.
void StepDown(void)
{
	StepPoint--;
	StepPoint %= 8; 
	PORTA = HalfSteps[StepPoint];	
}
void StepUp(void)
{
	StepPoint++;
	StepPoint %= 8; 
	PORTA = HalfSteps[StepPoint];	
}

//Method to move a number of steps
void MoveSteps(unsigned int NumSteps)
{
	//Make calculations
	if (NumSteps == 0)
	{
		STATE  = STOP;
	}
	else if(NumSteps == 1)
	{
		STATE = DECELERATE;
	}
	else //NumSteps > 1
	{
		STATE = ACCELERATE;
	}
	//Start Timer
}


//Timer Interrupt
ISR(TIMER0_COMPA_vect)
{
	PORTB |= (1<<MotorEN);
	StepUp(); //step up each time overflow occurs
	switch(STATE)
	{
		case STOP:
			PORTB &= ~(1<<MotorEN); //disable motor.
			TCCR0B = 0x00;//stop timer.
			break;
			
		case ACCELERATE:
			OCR0A -= ACCEL;
			if(OCR0A <= MIN_OCR0A)
				STATE = RUN;
			break;
			
		case RUN:
			break;
			
		case DECELERATE:
			OCR0A += DECEL;
			if(OCR0A >= 0xF0)
				STATE = STOP;
			break;
			
		default:
			STATE = STOP;
			break;
	}
	
}


void StartTimer(void)
{
	TCCR0A |= (1<<WGM01);//Clear timer on compare - variable overflow.
	TCCR0B |= (1<<CS02)|(1<<CS00); //prescaler to 1024
	OCR0A = 0xFF;//Timer compare
	TIMSK0 |= (1<<OCIE0A); //enable interrupt on compare. 
	STATE = ACCELERATE;
}

void StopTimer(void)
{
	STATE = DECELERATE; //set the state - if the timer is running it will close itself. 
}

void SetMaxSpeed(int Speed)
{
	MIN_OCR0A = 0xFF - (uint8_t)Speed;
}

void SetAccel(int Accel)
{
	ACCEL = (uint8_t)Accel;
}

void SetDecel(int Decel)
{
	DECEL = (uint8_t)Decel;
}
