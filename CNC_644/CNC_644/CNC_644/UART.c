/*
 * UART.c
 * C code to activate and use the UART0 on the 644P
 * Speed and F_CPU defined in UART.h
 * Created: 08/07/2012 14:11:34
 *  Author: hslovett
 */ 
 #include "UART.h"
 #include <avr/io.h>
 #include <avr/interrupt.h>
 
void USART_Init (unsigned int ubrr)
{
	//Set baud rate
	UBRR0H = (unsigned char)(ubrr>>8);
	UBRR0L = (unsigned char)ubrr;
	//Enable receiver and transmitter
	UCSR0B = (1<<RXEN0)|(1<<TXEN0);
	
	UCSR0C = (1<<UCSZ00)|(1<<UCSZ01); //set asynchronous, no parity, one stop bit, 8 bit transfer.
	
	UCSR0B |= (1 << RXCIE0); //set RX interrupt on
	
}
void USART_Transmit( unsigned char data )
{
	/* Wait for empty transmit buffer */
	while ( !( UCSR0A & (1<<UDRE0)) )
		;
	/* Put data into buffer, sends the data */
	UDR0 = data;
}
unsigned char USART_Receive( void )
{
	/* Wait for data to be received */
	while ( !(UCSR0A & (1<<RXC0)) )
		;
	/* Get and return received data from buffer */
	return UDR0;
}
ISR(USART0_RX_vect) //trigger interrupt when uart1 receives data   USART0_RX_vect
{ 
	// Code to be executed when the USART receives a byte here
	Received = UDR0; // Fetch the received byte value into the variable "Received" 

}