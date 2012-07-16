/*
 * UART.c
 * C code to activate and use the UART0 on the 644P
 * Speed and F_CPU defined in UART.h
 * Created: 08/07/2012 14:11:34
 *  Author: hslovett
 */ 
 #include "UART.h"
 #include "global.h"
 #include "speed_cntr.h"
 //#include "STEPPER.h"
 
 #include <avr/io.h>
 #include <avr/interrupt.h>
 #include <avr/eeprom.h>

//! RX buffer for uart.
unsigned char UART_RxBuffer[UART_RX_BUFFER_SIZE];
//! RX buffer pointer.
unsigned char UART_RxPtr;

// Static Variables.
//! TX buffer for uart.
static unsigned char UART_TxBuffer[UART_TX_BUFFER_SIZE];
//! TX buffer head pointer.
static volatile unsigned char UART_TxHead;
//! TX buffer tail pointer.
static volatile unsigned char UART_TxTail;


void USART_Init (unsigned int ubrr)
{
	//Set baud rate
	UBRR0H = (unsigned char)(ubrr>>8);
	UBRR0L = (unsigned char)ubrr;
	//Enable receiver and transmitter
	UCSR0B = (1<<RXEN0)|(1<<TXEN0);
	
	UCSR0C = (1<<UCSZ00)|(1<<UCSZ01); //set asynchronous, no parity, one stop bit, 8 bit transfer.
	
	UCSR0B |= (1 << RXCIE0); //set RX interrupt on
	Received = ' ';
	Command = 0;
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
void uart_SendByte(unsigned char data)
{
//   unsigned char tmphead;
// 
//   // Calculate buffer index
//   tmphead = ( UART_TxHead + 1 ) & UART_TX_BUFFER_MASK;
//   // Wait for free space in buffer
//   while ( tmphead == UART_TxTail )
//     ;
//   // Store data in buffer
//   UART_TxBuffer[tmphead] = data;
//   // Store new index
//   UART_TxHead = tmphead;
//   // Enable UDRE interrupt
//   SET_UDRIE;
	USART_Transmit(data);
}

/*! \brief Sends a string.
 *
 *  Loops thru a string and send each byte with uart_SendByte.
 *  If TX buffer is full it will hang until space.
 *
 *  \param Str  String to be sent.
 */
void uart_SendString(char Str[])
{
  unsigned char n = 0;
  while(Str[n])
    uart_SendByte(Str[n++]);
}

/*! \brief Sends a integer.
 *
 *  Converts a integer to ASCII and sends it using uart_SendByte.
 *  If TX buffer is full it will hang until space.
 *
 *  \param x  Integer to be sent.
 */
void uart_SendInt(int x)
{
  static const char dec[] = "0123456789";
  unsigned int div_val = 10000;

  if (x < 0)
  {
    x = - x;
    uart_SendByte('-');
  }
  
  while (div_val > 1 && div_val > x)
    div_val /= 10;
	
  do
  {
    uart_SendByte (dec[x / div_val]);
    x %= div_val;
    div_val /= 10;
  }
  while(div_val);
}

/*! \brief Empties the uart RX buffer.
 *
 *  Empties the uart RX buffer.
 *
 *  \return x  Integer to be sent.
 */
void uart_FlushRxBuffer(void)
{
	UART_RxPtr = 0;
	UART_RxBuffer[UART_RxPtr] = 0;
}
ISR(USART0_RX_vect) //trigger interrupt when uart1 receives data   USART0_RX_vect
{ 
	unsigned char data;

  // Read the received data.
  data = UDR0;
	
  if(status.running == FALSE)
  {
	
      // Put the data into RxBuf
      // and place 0x00 after it. If buffer is full,
      // data is written to UART_RX_BUFFER_SIZE - 1.
      if(UART_RxPtr < (UART_RX_BUFFER_SIZE - 1)){
        UART_RxBuffer[UART_RxPtr] = data;
        UART_RxBuffer[UART_RxPtr + 1]=0x00;
        UART_RxPtr++;
      }
      else
      {
        UART_RxBuffer[UART_RxPtr - 1] = data;
        uart_SendByte('b');
      }
      // If enter.
      if(data == 13){
        status.cmd = TRUE;
      }
      else
        uart_SendByte(data);
    
  }
}

ISR(USART0_TX_vect)
{
   unsigned char UART_TxTail_tmp;
   UART_TxTail_tmp = UART_TxTail;

  // Check if all data is transmitted
  if ( UART_TxHead !=  UART_TxTail_tmp )
  {
    // Calculate buffer index
    UART_TxTail_tmp = ( UART_TxTail + 1 ) & UART_TX_BUFFER_MASK;
    // Store new index
    UART_TxTail =  UART_TxTail_tmp;
    // Start transmition
    UDR0= UART_TxBuffer[ UART_TxTail_tmp];
  }
  else
    // Disable UD
	CLR_UDRIE;
}

