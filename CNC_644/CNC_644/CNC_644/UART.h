/*
 * UART.h
 * Header file for UART on 644P. 
 * F_CPU and BITRATE set in this file
 * Created: 08/07/2012 14:09:13
 *  Author: hslovett
 */ 
#include <avr/interrupt.h>

#ifndef UART_H_
#define UART_H_
#define F_CPU 12000000
#define BITRATE 57600
#define MYUBBR F_CPU/16/BITRATE-1

//#define BufferSize 20
char Received;
char Command;

void USART_Init (unsigned int ubrr);

void USART_Transmit( unsigned char data );

unsigned char USART_Receive( void );

#endif /* UART_H_ */