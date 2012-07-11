/*
 * ObjectProtocol.h
 *
 * Created: 09/07/2012 22:12:11
 *  Author: hslovett
 */ 


#ifndef OBJECTPROTOCOL_H_
#define OBJECTPROTOCOL_H_

#define FAMILY 0x48
#define CHIP 0x40
#define VERSION 0x01

uint8_t InfoSize;

void MakeInfo(void);

void SendInfo(void);

#endif /* OBJECTPROTOCOL_H_ */

/*
---Info Block ---
FAMILY
CHIP
VERSION
NUMBER OF OBJECTS
OBJ1 - ObjID
OBJ1 - StartAddr
OBJ1 - Size
OBJ1 - Instance
OBJ2 - ObjID
...
--------------------









*/