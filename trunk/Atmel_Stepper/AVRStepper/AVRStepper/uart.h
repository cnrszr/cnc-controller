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
#define SET_UDRIE (UCSR0B |=  (1<<UDRIE0))
#define CLR_UDRIE (UCSR0B &= ~(1<<UDRIE0))

// UART Buffer Defines
#define UART_RX_BUFFER_SIZE 32 // 2,4,8,16,32,64,128 or 256 bytes
#define UART_RX_BUFFER_MASK ( UART_RX_BUFFER_SIZE - 1 )
#if ( UART_RX_BUFFER_SIZE & UART_RX_BUFFER_MASK )
  #error RX buffer size is not a of 2
#endif

#define UART_TX_BUFFER_SIZE 64 // 2,4,8,16,32,64,128 or 256 bytes
#define UART_TX_BUFFER_MASK ( UART_TX_BUFFER_SIZE - 1 )
#if ( UART_TX_BUFFER_SIZE & UART_TX_BUFFER_MASK )
  #error TX buffer size is not a power of 2
#endif

//! Buffer with received string from uart.
extern unsigned char UART_RxBuffer[UART_RX_BUFFER_SIZE];

void USART_Init (unsigned int ubrr);

void uart_SendByte(unsigned char data);
void uart_SendString(char Tab[]);
void uart_SendInt(int Tall);
void uart_FlushRxBuffer(void);

#endif /* UART_H_ */