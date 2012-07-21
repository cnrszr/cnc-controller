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
	
void StartMotors(void)
{
	if(statusX.running == TRUE)
	{
		StartX();
	}
				
	if(statusY.running == TRUE)
	{
		StartY();
	}
				
	if(statusZ.running == TRUE)
	{
		StartZ();
	}
}

void G00() 
{
	//uart_SendString("Rapid Move");
	int point = 3;
	int Step;
	while(Received[point++])
	{
		if(Received[point] == 'X')
		{
			double dblTemp = 0;
			int LocTemp;
			point++;											
			dblTemp = atof((char const *)Received + point);
			LocTemp = (int)(dblTemp*100);
			//uart_SendInt(LocTemp);
			Step = LocTemp - XLocation;
			speed_cntr_SetupX(Step, 500, 500, 350);
		}
		if(Received[point] == 'Y')
		{
			double dblTemp = 0;
			int LocTemp;
			point++;											
			dblTemp = atof((char const *)Received + point);
			LocTemp = (int)(dblTemp*100);
			//uart_SendInt(LocTemp);
			Step = LocTemp - YLocation;
			speed_cntr_SetupY(Step, 500, 500, 350);
		}
		if(Received[point] == 'Z')
		{
			double dblTemp = 0;
			int LocTemp;
			point++;											
			dblTemp = atof((char const *)Received + point);
			LocTemp = (int)(dblTemp*100);
			Step = LocTemp - ZLocation;
			speed_cntr_SetupZ(Step, 500, 500, 350);
			//uart_SendInt(LocTemp);
		}
										
	}
	StartMotors();					
}

int main(void)
{
	USART_Init(MYUBBR); //initialise UART0
	uart_SendString("Set Up...\n\r");
	speed_cntr_Init_Timer1();
	sm_driver_Init_IO();
	XLocation = 0;
	YLocation = 0;
	ZLocation = 0;
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
	//int point = 0;
	
	uart_SendString("Ready!\n\r");
	while(1)
	{
		if(COMMANDRECEIVED == TRUE)
		{			
			switch(Received[0])
			{
				case 'G':
					switch(Received[1])
					{
						case '0':
							switch(Received[2])
							{
								case '0':
									G00();
									break;
									
								default:
									uart_SendString("Unsupported G Code");
									break;
							}
							break;
						
						default:
							uart_SendString("Unsupported G Code");
							break;
					}
					break;
				case 'g': //start all set up motors. 
					StartMotors();
					break;
				
				case 'X': //Set up the X motor with current settings
					speed_cntr_SetupX(Step, accel, decel, speed);
					break;
					
				case 'Y': //set up the Y motor with current settings
					speed_cntr_SetupY(Step, accel, decel, speed);
					break;
					
				case 'Z': //set up the Z motor with current settings
					speed_cntr_SetupZ(Step, accel, decel, speed);
					break;
					
				case 'S'://set the Step variable
					Step = atoi((char const *)Received+2);
					break;
					
				case 'a'://set the Accel Variable
					accel = atoi((char const *)Received+2);
					break;
					
				case 'd'://set up the deceleration variable
					decel = atoi((char const *)Received+2);
					break;
					
				case 's'://set the steps variable
					speed = atoi((char const *)Received+2);
					//speed_cntr1_Setup1(Step, accel, decel, speed);
					if (speed > MAXSPEED)
					{
						uart_SendString("WARNING: EXCEEDED MAXIMUM SPEED");
					}
					break;
					
				case 'I'://show all variable information
					uart_SendString("\n\rSteps: ");
					uart_SendInt(Step);
					uart_SendString("\n\rAccel: ");
					uart_SendInt(accel);
					uart_SendString("\n\rSpeed: ");
					uart_SendInt(speed);
					uart_SendString("\n\rDecel: ");
					uart_SendInt(decel);
					break;
					
				case 'L'://show current locations of device
					USART_Transmit('X');
					USART_Transmit('=');
					uart_SendInt(XLocation);
					USART_Transmit('\r');
					USART_Transmit('\n');
					USART_Transmit('Y');
					USART_Transmit('=');
					uart_SendInt(YLocation);
					USART_Transmit('\r');
					USART_Transmit('\n');
					USART_Transmit('Z');
					USART_Transmit('=');
					uart_SendInt(ZLocation);
					USART_Transmit('\r');
					USART_Transmit('\n');
					break;
					
				case '0'://Zero all Variables.
					XLocation = 0;
					YLocation = 0;
					ZLocation = 0;
					break;	
					
				default://unsupported command
					uart_SendString("Incorrect Command");
					break;
			}
			COMMANDRECEIVED = FALSE;
			FlushBuffer();
			uart_SendString("\n\r>");
		}
		else
		{
			PORTA = PORTA;
		}
	}
}