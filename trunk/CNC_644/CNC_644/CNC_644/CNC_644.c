/*
 * CNC_644.c
 *
 * Created: 08/07/2012 14:08:55
 *  Author: hslovett
 */ 

#include <avr/io.h>
#include <avr/interrupt.h>

#include "UART.h" //UART0

int main(void)
{
	USART_Init(MYUBBR); //initialise UART0
	sei();
	DDRA = 0xFF;
	PORTA = 0;
	DDRC = 0xFF;//set port c to outputs
    while(1)
    {
        
    }
}