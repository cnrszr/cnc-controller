/*
 * STEPPER.h
 *
 * Created: 08/07/2012 15:53:42
 *  Author: hslovett
 */ 


#ifndef STEPPER_H_
#define STEPPER_H_




//////////////////////////////////////////////////////////////////////////
/*Methods*/

//Initialisation sets up timer for inter-step delay. 
void StepperInitialise(void); 

//Method to move a given number of steps
void MoveSteps(unsigned int NumSteps);


void StartTimer(void);

void StopTimer(void);

void SetMaxSpeed(int Speed);

void SetAccel(int Accel);

void SetDecel(int Decel);

#endif /* STEPPER_H_ */