/*
 * ObjectProtocol.c
 *
 * Created: 09/07/2012 22:12:25
 *  Author: hslovett
 */ 
#include "UART.h"
#include "ObjectProtocol.h"
#include <avr/eeprom.h>
#include <avr/>
	
void MakeInfo(void)//method populates the info block in EEPROM Memory	
{
	eeprom_write_byte(0x00, FAMILY);
	eeprom_write_byte(0x01, CHIP);
	eeprom_write_byte(0x02, VERSION);
}

void SendInfo(void)
{
	for(uint8_t i = 0; i < 3; i++)
	{
		USART_Transmit(eeprom_read_byte(i));
	}
}