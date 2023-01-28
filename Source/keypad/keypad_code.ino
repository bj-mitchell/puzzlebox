//#include <SoftwareSerial.h>
#include <Keypad.h>
#include <Wire.h>

#define SLAVE_ADDRESS 0x2a

const byte RX_PIN = 3;
const byte TX_PIN = 4;
const byte IRQ_PIN = 7;

const byte ROWS = 4;
const byte COLS = 4;

char current_keypress = ' ';

//SoftwareSerial serial(RX_PIN,TX_PIN);

char keypadChars[ROWS][COLS] = {
  { '1', '2', '3', 'A' },
  { '4', '5', '6', 'B' },
  { '7', '8', '9', 'C' },
  { '*', '0', '#', 'D' }
};

byte rowPins[ROWS] = { 18, 17, 16, 15 }; 
byte colPins[COLS] = { 22, 21, 20, 19 }; 

Keypad keypad = Keypad(makeKeymap(keypadChars), rowPins, colPins, ROWS, COLS); 

void setup() {
  pinMode(IRQ_PIN, OUTPUT);

  //serial.begin(9600);
  //serial.println("Serial output from keypad");

  Wire.begin(SLAVE_ADDRESS);
  Wire.onRequest(requestEvent);
}

void loop() {
  char key = keypad.getKey();

  if (key) {
    //serial.print(key);
    current_keypress = key;
    digitalWrite(IRQ_PIN, HIGH);
  }
  delay(20);
}

void requestEvent() {
  //serial.println("Request received");

  Wire.write(current_keypress);
  digitalWrite(IRQ_PIN, LOW);
  //delay(100);
}
