/*
 * UART.c
 * C code to activate and use the UART0 on the 644P
 * Speed and F_CPU defined in UART.h
 * Created: 08/07/2012 14:11:34
 *  Author: hslovett
 */ 
 #include "UART.h"
 #include "STEPPER.h"
#include "Pin_Definitions.h"

 #include <avr/io.h>
 #include <avr/interrupt.h>
 #include <avr/eeprom.h>


 //uint8_t LastReceived;
void USART_Init (unsigned int ubrr)
{
	//Set baud rate
	UBRR0H = (unsigned char)(ubrr>>8);
	UBRR0L = (unsigned char)ubrr;
	//Enable receiver and transmitter
	UCSR0B = (1<<RXEN0)|(1<<TXEN0);
	
	UCSR0C = (1<<UCSZ00)|(1<<UCSZ01); //set asynchronous, no parity, one stop bit, 8 bit transfer.
	
	UCSR0B |= (1 << RXCIE0); //set RX interrupt on
	Pointer = 0;
	Received[Pointer] = 0;
	COMMANDRECEIVED = FALSE;
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
void uart_SendInt(int x)
{
  static const char dec[] = "0123456789";
  unsigned int div_val = 10000;

  if (x < 0){
    x = - x;
    USART_Transmit('-');
  }
  while (div_val > 1 && div_val > x)
    div_val /= 10;
  do{
    USART_Transmit(dec[x / div_val]);
    x %= div_val;
    div_val /= 10;
  }while(div_val);
}
void uart_SendString(unsigned char Str[])
{
  unsigned char n = 0;
  while(Str[n])
    USART_Transmit(Str[n++]);
}
void FlushBuffer(void)
{
	COMMANDRECEIVED = FALSE;
	for(int i = 0; i < Pointer; i++)
	{
		Received[i] = 0;
	}
	Pointer = 0;
	Received[Pointer] = 0x00;
}
ISR(USART0_RX_vect) //trigger interrupt when uart1 receives data   USART0_RX_vect
{ 
	char r = UDR0;
	if(r == '`')
	{
		uart_SendString("Buffer:\"");
		for(int i = 0; i < Pointer; i++)
		{
			USART_Transmit(Received[i]);
		}
		uart_SendString("\" ");
		
		USART_Transmit((char)13);
		USART_Transmit('\n');
	}
	else if (r == '¬')
	{
		FlushBuffer();
		USART_Transmit((char)13);
		USART_Transmit('\n');
	}
	else if (r == (char)13)
	{
		COMMANDRECEIVED = TRUE;
		
		USART_Transmit((char)13);
		USART_Transmit('\n');
	}
	else
	{
		USART_Transmit(r);
		Received[Pointer++] = r;
		if(r == 13)
		{
			COMMANDRECEIVED = 'Y';
			USART_Transmit('\n');
		}
	}	
	
// 	Received[Pointer++] = UDR0;
// 	USART_Transmit(Received[Pointer]);
// 		
// 	if(Received[Pointer] == 13)
// 	{
// 		CommandReceived = TRUE;
// 	}
}


