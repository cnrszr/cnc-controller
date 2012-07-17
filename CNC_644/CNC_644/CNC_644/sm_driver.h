/*This file has been prepared for Doxygen automatic documentation generation.*/
/*! \file *********************************************************************
 *
 * \brief Header file for sm_driver.c.
 *
 * - File:               sm_driver.h
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
 * $RCSfile: sm_driver.h,v $
 * $Date: 2006/05/08 12:25:58 $
 *****************************************************************************/

#ifndef SM_DRIVER_H
#define SM_DRIVER_H

// Direction of stepper motor movement
#define CW  0
#define CCW 1

/*! \Brief Define stepping mode to use in stepper motor.
 *
 * Either halfsteps (HALFSTEPS) or fullsteps (FULLSTEPS) are allowed.
 *
 */
#define HALFSTEPS
//#define FULLSTEPS

/*! \Brief Define IO port and pins
 *
 * Set the desired drive port and pins to support your device
 *
 */
#define SMZ_PORT         PORTD
#define SMZ_DRIVE        DDRD
#define ZA1    PD7 //!< Stepper motor winding A positive pole.
#define ZA2    PD6 //!< Stepper motor winding A negative pole.
#define ZB1    PD5 //!< Stepper motor winding B positive pole.
#define ZB2    PD4 //!< Stepper motor winding B negative pole.

#define SMX_PORT         PORTA
#define SMX_DRIVE        DDRA
#define XA1    PA7 //!< Stepper motor winding A positive pole.
#define XA2    PA6 //!< Stepper motor winding A negative pole.
#define XB1    PA5 //!< Stepper motor winding B positive pole.
#define XB2    PA4 //!< Stepper motor winding B negative pole.

#define SMY_PORT         PORTB
#define SMY_DRIVE        DDRB
#define YA1    PB7 //!< Stepper motor winding A positive pole.
#define YA2    PB6 //!< Stepper motor winding A negative pole.
#define YB1    PB5 //!< Stepper motor winding B positive pole.
#define YB2    PB4 //!< Stepper motor winding B negative pole.

void sm_driver_Init_IO(void);
unsigned char smZ_driver_StepCounter(signed char inc);
unsigned char smX_driver_StepCounter(signed char inc);
unsigned char smY_driver_StepCounter(signed char inc);
void smZ_driver_StepOutput(unsigned char pos);
void smX_driver_StepOutput(unsigned char pos);
void smY_driver_StepOutput(unsigned char pos);

//! Position of stepper motor.
extern int stepPositionZ;
extern int stepPositionX;
extern int stepPositionY;

#endif
