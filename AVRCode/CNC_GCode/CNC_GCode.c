/*
 * cnc_buss.c
 *
 * Created: 04/06/2012 12:06:33
 *  Author: hslovett
 */ 

#define F_CPU 12000000

#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>

#define SPEED 57600
#define MYUBBR F_CPU/16/SPEED-1

#define MOTOR1 PB0
#define MOTOR2 PB1
#define MOTOR3 PB2
#define MOTOR4 PB3

#define X_EN PC0
#define Y_EN PC1
#define Z_EN PC2

#define X0 PD2
#define Y0 PD3

#define NOOP 0
#define CAL 1

#define DELAY 3
#define BufferSize 30

char steps[8] = {0x08, 0x0A, 0x02, 0x06, 0x04, 0x05, 0x01, 0x09};
char recieved;
uint8_t RevOrStep;
uint8_t Pointer;
char Buffer[BufferSize];

// char BeginChar = 'B';
// char EndChar ='E';
void Init(void)
{
	DDRB = 0xFF; //Set PORTB to outputs
	DDRC = 0xFF; //Set PORTC to outputs
	DDRD = 0x00; //set to inputs. USART overrides D0 and D1
	PORTD = 0xFF; //set pull up resistors on
	recieved = 65;
	RevOrStep = 0;
	Pointer = 0;
}
void USART_Init (unsigned int ubrr)
{
	//Set baud rate
	UBRR0H = (unsigned char)(ubrr>>8);
	UBRR0L = (unsigned char)ubrr;
	//Enable receiver and transmitter
	UCSR0B = (1<<RXEN0)|(1<<TXEN0);
	
	UCSR0C = 0x06; //set asynchronous, no parity, one stop bit, 8 bit transfer.
	
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

void StepUp(void)
{
	//PORTB = 0x05;
	for(int i = 0; i < 8; i++)
	{
		PORTB = steps[i];
		_delay_ms(DELAY);
	}

}
void StepDown(void)
{
	for(int j = 7; j >= 0; j--)
	{
		PORTB = steps[j];
		_delay_ms(DELAY);
	}	
}
void RevDown(void)
{
	for(int i = 0; i < 12; i++)
	{
		for(int j = 7; j >= 0; j--)
		{
			PORTB = steps[j];
			_delay_ms(DELAY);
		}
	}
}
void RevUp(void)
{
	for(int j = 0; j < 12; j++)
	{
		for(int i = 0; i < 8; i++)
		{
			PORTB = steps[i];
			_delay_ms(DELAY);
		}
	}
}
void XStepDown(void)//if the passed value is greater than 1, the x will step up
{
	//PORTD = 0x04;
	//PORTB = steps[0];	
	PORTC |= (1<<X_EN); //enable the x movement
	if(RevOrStep == 0)
	{
		RevUp();
	}
	else
	{
		StepUp();
	}		
	PORTC &= ~(1<<X_EN); //disable x
	
}
void XStepUp(void)
{
	PORTB = steps[7];
	PORTC |= (1<<X_EN); //enable the x movement
	if(RevOrStep == 0)
	{
		RevDown();
	}
	else
	{
		StepDown();
	}	
	PORTC &= ~(1<<X_EN); //disable x
	
}
void YStepUp(void)//if the passed value is greater than 1, the y will step up
{
	PORTB = 0x08;
	PORTC |= (1<<Y_EN); //enable the y movement
	if(RevOrStep == 0)
	{
		RevUp();
	}
	else
	{
		StepUp();
	}	
	PORTC &= ~(1<<Y_EN); //disable y
}
void YStepDown(void)
{
	PORTB = 0x09;
	PORTC |= (1<<Y_EN); //enable the y movement
	if(RevOrStep == 0)
	{
		RevDown();
	}
	else
	{
		StepDown();
	}	
	PORTC &= ~(1<<Y_EN); //disable y

}
void ZStepUp(void)//if the passed value is greater than 1, the x will step up
{
	PORTB = 0x08;
	PORTC |= (1<<Z_EN); //enable the z movement
	if(RevOrStep == 0)
	{
		RevUp();
	}
	else
	{
		StepUp();
	}	
	PORTC &= ~(1<<Z_EN); //disable z
}
void ZStepDown(void)
{
	PORTB = 0x09;
	PORTC |= (1<<Z_EN); //enable the z movement
	if(RevOrStep == 0)
	{
		RevDown();
	}
	else
	{
		StepDown();
	}	
	PORTC &= ~(1<<Z_EN); //disable z

}
void MillBuffer(void)
{
	for(int i = 0; i < BufferSize; i ++)
	{
		if(recieved == 'Q')//escape clause
			i = BufferSize;
		else
		{
			switch(Buffer[i])
			{
				case 'x': //xdown
					XStepDown();
					break;
				
				case 'X': //x up
					XStepUp();
					break;
				
				case 'y': //y down
					YStepDown();
					break;
				
				case 'Y': //y up
					YStepUp();
					break;
				
				case 'z': //z down
					ZStepDown();
					break;
				
				case 'Z': //z up
					ZStepUp();
					break;
			
				case 'R': //set to do 1 full revolution
					RevOrStep = 0;
					break;
				
				case 'S': //set to do one step (1/12th revolution)
					RevOrStep = 1;
					break;
				
				case 'E':
					i = BufferSize;
					break;
				
				default:
					RevOrStep = RevOrStep;
					break;
			}
		}		
	}
	
	
}
ISR(USART_RX_vect) //trigger interrupt when uart1 receives data   USART_RXC1_vect
{ 
// Code to be executed when the USART receives a byte here
	recieved = UDR0; // Fetch the recieved byte value into the variable "ByteReceived" 
	if(recieved == 'G')
	{
		recieved = recieved;//do nothing, doesn't want to be added into the buffer
	}
	else 
	{
		Buffer[Pointer] = recieved;
		Pointer++;
		if(Pointer == BufferSize)
		{
			Pointer = 0;
			sei();//re-enable interrupts so USART will still work
			MillBuffer();
		}
	}
}
int main(void)
{
	Init();
	USART_Init(MYUBBR); //initialise UART 
	sei(); //turn global interrupts on
	RevOrStep = 0;
	
    while(1)
    {
		//USART_Transmit('0');
		if(recieved == 'B')
		{
			//USART_Transmit('Q');
			recieved = ' ';
			Pointer = 0;
			for(uint8_t i = 0; i < BufferSize; i++)
			{
				Buffer[i] = '.';
			}
		}
		else if(recieved == 'E')
		{
			//USART_Transmit('q');
			MillBuffer();
			recieved = ' ';
			
		}
		else if(recieved == 'G')
		{
			for(int i = 0; i < BufferSize; i ++)
			{
				USART_Transmit(Buffer[i]); //transmit the entire buffer back
			}
			recieved = ' ';
		}
		else
		{
			recieved = recieved;
			
			
		}
	}
}