/*
 * CNC_644.c
 *
 * Created: 08/07/2012 14:08:55
 *  Author: hslovett
 */ 
#define F_CPU 12000000
#include <avr/io.h>
#include <avr/interrupt.h>

#include "UART.h" //UART0
#include "STEPPER.h"//stepper0
uint8_t Full_NHalf;

int main(void)
{
	USART_Init(MYUBBR); //initialise UART0
	StepperInitialise();
	sei();
	DDRA = 0xFF;//set port c to outputs
	PORTA = 0x00;
	DDRB = 0xFF;
	PORTB = 0x00;
    while(1)
    {
        if(Received == 'X')
		{
			StepUp();
			Received = ' ';
		}
		else if (Received == 'x')
		{
			StepDown();
			Received = ' ';
		}
		else if (Received == 'F')
		{
			Full_NHalf = 1;
		}
		else if (Received == 'f')
		{
			Full_NHalf = 0;
		}
		else
		{
			Received = Received;
		}	
										
    }
}