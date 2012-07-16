/*
 * CNC_644.c
 *
 * Created: 08/07/2012 14:08:55
 *  Author: hslovett
 */ 
#define F_CPU 12000000
#include <avr/io.h>
#include <avr/interrupt.h>
#include <stdlib.h>

#include "UART.h"
#include "global.h"
#include "speed_cntr.h"
#include "sm_driver.h"

unsigned char str[] = {"Command Received"};
int main(void)
{
	USART_Init(MYUBBR); //initialise UART0
	speed_cntr_Init_Timer1();
	sm_driver_Init_IO();
	
	sei();
	DDRA = 0xFF;//set port c to outputs
	PORTA = 0x00;
	DDRB = 0xFF;
	PORTB = 0x00;
	DDRC = 0xFF;
	PORTC = 0x00;
	DDRD = 0xFF;
	PORTD = 0x00;
    int Step;
	unsigned int accel, decel, speed;
	Step = 100;
	accel = 71;
	speed = 12;
	decel = 71;
	while(1)
	{
		if(COMMANDRECEIVED == TRUE)
		{
			if(Received[0] == 'G')
			{
				if(Received[1] == '1')
				{
					speed_cntr_Move(Step, accel, decel, speed);
					//StartTimer1();
				}
				else if (Received[1] == '2')
				{
					speed_cntr_MoveX(Step, accel, decel, speed);
				}
				
			}
			else if(Received[0] == 'S')//set steps
			{
				Step = atoi((char const *)Received+2);	
				//speed_cntr1_Setup1(Step, accel, decel, speed);
			}
			else if (Received[0] == 'A')//set acceleration
			{
				accel = atoi((char const *)Received+2);
				//speed_cntr1_Setup1(Step, accel, decel, speed);
			}
			else if (Received[0] == 'd')//set deceleration
			{
				decel = atoi((char const *)Received+2);
				//speed_cntr1_Setup1(Step, accel, decel, speed);
			}
			else if (Received[0] == 's')//set speed
			{
				speed = atoi((char const *)Received+2);
				//speed_cntr1_Setup1(Step, accel, decel, speed);
				if (speed > MAXSPEED)
				{
					uart_SendString("WARNING: EXCEEDED MAXIMUM SPEED");
				}
			}
			else if (Received[0] == 'I')
			{
				uart_SendString("\n\rSteps: ");
				uart_SendInt(Step);
				uart_SendString("\n\rAccel: ");
				uart_SendInt(accel);
				uart_SendString("\n\rSpeed: ");
				uart_SendInt(speed);
				uart_SendString("\n\rDecel: ");
				uart_SendInt(decel);
				
			}
			else
			{
				uart_SendString("Incorrect Command");
			}
			COMMANDRECEIVED = FALSE;
			FlushBuffer();
			uart_SendString("\n\r>");
		}
		else
		{
			PORTA = PORTA;//added to fill the else statement else the compiler removes the entire if/else
		}
	}
}