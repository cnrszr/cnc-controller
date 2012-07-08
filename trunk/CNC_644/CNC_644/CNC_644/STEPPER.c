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
unsigned int ACCEL;
unsigned int DECEL;
unsigned int MAX_SPEED;
unsigned int NUM_STEPS;

//Constants
uint8_t StepsPerRound = 96;
uint8_t StepAngle = 4; //actual value = 3.75

//Step Sequences
uint8_t HalfSteps[8] = {0x08, 0x0A, 0x02, 0x06, 0x04, 0x05, 0x01, 0x09};
uint8_t FullSteps[4] = {0x08, 0x02, 0x04, 0x01};

void StepperInitialise(void)
{
	StepPoint = 0;//Initialise the stepper pointer
	ACCEL = 100;
	DECEL = 100;
	MAX_SPEED = 100;
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
	
	//Start Timer
}

//Method to make all the necessary calculations to begin a movement.
void SetUpCalculations(void)
{
	//
}
//Timer Interrupt
ISR(TIMER0_COMPA_vect)
{
	switch(STATE)
	{
		case STOP:
			break;
			
		case ACCELERATE:
			break;
			
		case RUN:
			break;
			
		case DECELERATE:
			break;
			
		default:
			STATE = STOP;
			break;
	}
	
}

unsigned int C0(void)
{
	
}