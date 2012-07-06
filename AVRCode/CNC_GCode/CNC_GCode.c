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
char steps[8] = {0x08, 0x0A, 0x02, 0x06, 0x04, 0x05, 0x01, 0x09};
char recieved;
char hasrecieved;
char NumSteps;
int X, Y, Z;
uint8_t RevOrStep;

void Init(void)
{
	DDRB = 0xFF; //Set PORTB to outputs
	DDRC = 0xFF; //Set PORTC to outputs
	DDRD = 0x00; //set to inputs. USART overrides D0 and D1
	PORTD = 0xFF; //set pull up resistors on
	recieved = 65;
	hasrecieved = 0;
	NumSteps = 12;
	X = 0;
	Y = 0;
	Z = 0;
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
ISR(USART_RX_vect) //trigger interrupt when uart1 receives data   USART_RXC1_vect
	{ 
	// Code to be executed when the USART receives a byte here

	recieved = UDR0; // Fetch the recieved byte value into the variable "ByteReceived" 
/*	hasrecieved = 65;*/
// 	USART_Transmit('k');
	//USART_Transmit('K');
/*		USART_Transmit(recieved);*/
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
	X++;
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
	X --;
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
	Y--;
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
	Z--;
}

int main(void)
{
	Init();
	USART_Init(MYUBBR); //initialise UART 
	sei(); //turn global interrupts on
	RevOrStep = 0;
	uint8_t State = CAL;
	//uint8_t calCheck;
	
	//while(State != NOOP) //initial calibration MUST Occur. 
	//{
		//calCheck = PIND; //read data
		//calCheck = calCheck >> 0x02;//shift
		//calCheck &= 0x03;//remove unwanted data
		//
		//switch (calCheck)
		//{
			//case 0: //neither are active
				//XStepDown();
				//YStepDown();
				//break;
						//
			//case 1: //x is on, y is off
				//YStepDown();
				//break;
					//
			//case 2: //y is on, x is off
				//XStepDown();
				//break;
						//
			//case 3: //both on, calibrate complete
				//State = NOOP;
				//USART_Transmit('C');
				//recieved = ' ';
				//break;
						//
			//default://for completion and debugging
				//USART_Transmit((char)calCheck);
				//break;
		//}
	//}	
	
    while(1)
    {
		switch(recieved)
		{
			case 'x': //xdown
				XStepDown();
				USART_Transmit('D');
				recieved = ' ';
				break;
				
			case 'X': //x up
				XStepUp();
				USART_Transmit('D');
				recieved = ' ';
				break;
				
			case 'y': //y down
				YStepDown();
				USART_Transmit('D');
				recieved = ' ';
				break;
				
			case 'Y': //y up
				YStepUp();
				USART_Transmit('D');
				recieved = ' ';
				break;
				
			case 'z': //z down
				ZStepDown();
				USART_Transmit('D');
				recieved = ' ';
				break;
				
			case 'Z': //z up
				ZStepUp();
				USART_Transmit('D');
				recieved = ' ';
				break;
			
			case 'R': //set to do 1 full revolution
				RevOrStep = 0;
				break;
				
			case 'S': //set to do one step (1/12th revolution)
				RevOrStep = 1;
				break;
			
			case 'O': //make machine go to machine Origin
				State = CAL;
				break;
				
			case 'E': //escape clause
				State = NOOP;
				break;
				
			default:
				recieved = recieved;
				break;
		}
		
		//states used for anything over 1 instruction
// 		switch (State)
// 		{
// 			case NOOP: //just listen on UART
// 				State = NOOP;
// // 				if((PIND |= ~(1<<X0)) == 0xFF)
// // 				{
// // 					USART_Transmit('Q');
// // 				}
// 				break;
// 				
//  			case CAL://return to Machine 0
// 				calCheck = PIND; //read data
// 				calCheck = calCheck >> 0x02;//shift
// 				calCheck &= 0x03;//remove unwanted data
// 				switch (calCheck)
// 				{
// 					case 0: //neither are active
// 						XStepDown();
// 						YStepDown();
// 						break;
// 						
// 					case 1: //x is on, y is off
// 						YStepDown();
// 						break;
// 					
// 					case 2: //y is on, x is off
// 						XStepDown();
// 						break;
// 						
// 					case 3: //both on, calibrate complete
// 						State = NOOP;
// 						USART_Transmit('C');
// 						recieved = ' ';
// 						break;
// 						
// 					default:
// 						State = NOOP;
// 						USART_Transmit((char)calCheck);
// 						break;
// 				}
// 
// 			default: 
// 				State = NOOP;
// 				break;
		//}
	}
}