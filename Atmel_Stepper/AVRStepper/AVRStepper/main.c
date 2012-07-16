/*This file has been prepared for Doxygen automatic documentation generation.*/
/*! \file *********************************************************************
 *
 * \brief Demo of Linear speed ramp controller.
 *
 * Demo of linear speed ramp controller. Control of stepper motor by the
 * serial port. A menu gives the user status and shows the avaliable commands.
 *
 * - File:               main.c
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
 * $RCSfile: main.c,v $
 * $Date: 2006/05/08 12:25:58 $
 *****************************************************************************/

#include <avr/io.h>
#include <avr/interrupt.h>
#include <stdlib.h>

#include "global.h"
#include "uart.h"
#include "sm_driver.h"
#include "speed_cntr.h"

//! Global status flags
struct GLOBAL_FLAGS status = {FALSE, FALSE, 0};

void ShowHelp(void);
void ShowData(int position, int acceleration, int deceleration, int speed, int steps);

/*! \brief Init of peripheral devices.
 *
 *  Setup IO, uart, stepper, timer and interrupt.
 */
void Init(void)
{
  // Init of IO pins
  sm_driver_Init_IO();
  // Init of uart
  USART_Init(MYUBBR);

  // Set stepper motor driver output
  sm_driver_StepOutput(0);

  // Init of Timer/Counter1
  speed_cntr_Init_Timer1();

  sei();
}

/*! \brief Demo of linear speed controller.
 *
 *  Serial interface frontend to test linear speed controller.
 */
void main(void)
{
  // Number of steps to move.
  int steps = 1000;
  // Accelration to use.
  int acceleration = 100;
  // Deceleration to use.
  int deceleration = 100;
  // Speed to use.
  int speed = 800;
  // Tells if the received string was a valid command.
  char okCmd = FALSE;

  Init();

  // Outputs help screen.
  uart_SendString("\n\r");
  ShowHelp();
  ShowData(stepPosition, acceleration, deceleration, speed, steps);

  while(1) 
  {
	if(status.cmd == TRUE)
	{
		uart_SendByte('A');
		status.cmd = FALSE;
	}      
  }//end while(1)
}

//! Help message
//__flash 
char Help[] = {"\n\r--------------------------------------------------------------\n\rAtmel AVR446 - Linear speed control of stepper motor\n\r\n\r?        - Show help\n\ra [data] - Set acceleration (range: 71 - 32000)\n\rd [data] - Set deceleration (range: 71 - 32000)\n\rs [data] - Set speed (range: 12 - motor limit)\n\rm [data] - Move [data] steps (range: -64000 - 64000)\n\rmove [steps] [accel] [decel] [speed]\n\r         - Move with all parameters given\n\r<enter>  - Repeat last move\n\r\n\r    acc/dec data given in 0.01*rad/sec^2 (100 = 1 rad/sec^2)\n\r    speed data given in 0.01*rad/sec (100 = 1 rad/sec)\n\r--------------------------------------------------------------\n\r"};

/*! \brief Sends help message.
 *
 *  Outputs help message.
 */
void ShowHelp(void)
{
  unsigned int i = 0;
  while(Help[i] != 0)
    uart_SendByte(Help[i++]);
}

/*! \brief Sends out data.
 *
 *  Outputs the values of the data you can control by serial interface
 *  and the current position of the stepper motor.
 *
 *  \param acceleration Accelration setting.
 *  \param deceleration Deceleration setting.
 *  \param speed Speed setting.
 *  \param steps Position of the stepper motor.
 */
void ShowData(int position, int acceleration, int deceleration, int speed, int steps)
{
  uart_SendString("\n\r  Motor pos: ");
  uart_SendInt(position);
  uart_SendString("    a:");
  uart_SendInt(acceleration);
  uart_SendString("  d:");
  uart_SendInt(deceleration);
  uart_SendString("  s:");
  uart_SendInt(speed);
  uart_SendString("  m:");
  uart_SendInt(steps);
  uart_SendString("\n\r> ");
}

/*! \mainpage
 * \section Intro Introduction
 * This documents data structures, functions, variables, defines, enums, and
 * typedefs in the software for application note AVR446.
 *
 * \section CI Compilation Info
 * This software was written for the IAR Embedded Workbench 4.11A.
 *
 * To make project:
 * <ol>
 * <li> Add the file main.c, sm_driver.c, speed_cntr.c and uart.c to project.
 * <li> Under processor configuration, select desired Atmel AVR device.
 * <li> Enable bit definitions in I/O include files
 * <li> Compiler optimizations must be size medium or high optimization to fit
 * the code in a mega48.
 * - For size medium optimization CSTACK size must be >= 0x24.
 * - For size high optimization CSTACK size must be >= 0x23.
 * </ol>
 *
 * \section DI Device Info
 * The included source code is written for all Atmel AVR devices with 16 bit timers.
 *
 * \section HW Hardware Setup
 * Fuse settings: 0xFF, 0xDD, 0xC0
 * (External Clock running on 3,68Mhz).
 *
 * Port Connections:
 * <ol>
 * <li>PD0 - Serial RXD.
 * <li>PD1 - Serial TXD.
 * <li>PD4 - Stepper motor winding \ref B2.
 * <li>PD5 - Stepper motor winding \ref B1.
 * <li>PD6 - Stepper motor winding \ref A2.
 * <li>PD7 - Stepper motor winding \ref A1.
 * </ol>
 *
 *
 * \section TDL ToDo List
 *
 * \todo Specify the running frequency \ref T1_FREQ according to the timer frequency
 * \todo Specify the number of fullsteps in \ref FSPR
 * \todo Modify the \ref SM_PORT and \ref SM_DRIVE to support desired drive port.
 * \todo Modify \ref A1, \ref A2, \ref B1 and \ref B2 to support the pins on the desired drive port.
 * \todo Set the stepping mode in \ref HALFSTEPS to either halfsteps (HALFSTEPS) or fullsteps (FULLSTEPS)
 */
