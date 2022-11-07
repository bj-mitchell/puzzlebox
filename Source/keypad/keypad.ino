//#include <Wire.h>

//#define SLAVE_ADDRESS 0x2a

const int selectPins[3] = {2, 3, 4}; // S0~2, S1~3, S2~4
const int zOutput = 5; 
const int zInput = A0; // Connect common (Z) to A0 (analog input)
const int KEY_COUNT = 16;
const int PIN_COUNT = 8;

const int PIN_HIGH = 800;
const int PIN_LOW = 0;

//bool state[KEY_COUNT];
bool keys[KEY_COUNT];
bool pins[PIN_COUNT];

void setup() 
{
  //Wire.begin(SLAVE_ADDRESS);
  //Wire.onRequest(requestEvent);
  Serial.begin(9600); // Initialize the serial port
  // Set up the select pins as outputs:
  for (int i=0; i<3; i++)
  {
    pinMode(selectPins[i], OUTPUT);
    digitalWrite(selectPins[i], HIGH);
  }
  pinMode(zInput, INPUT); // Set up Z as an input

  for (int i=0; i < KEY_COUNT; i++) {
    keys[KEY_COUNT] = false;
  }

  for (int i=0; i < PIN_COUNT; i++) {
    pins[PIN_COUNT] = false;
  }
}

void loop() 
{
  // Loop through all eight pins.
  for (byte pin=0; pin<=7; pin++)
  {
    selectMuxPin(pin); // Select one at a time
    int inputValue = analogRead(zInput); // and read Z
 
    // Row pins are low when not pressed.  Exactly 0.
    // Column pins are high when not pressed.  Typically above 800.
    if (pin < 4 && inputValue > PIN_LOW) {
      pins[pin] = true;
    } else if (pin < 4) {
      pins[pin] = false; 
    }

    if (pin >= 4 && inputValue < PIN_HIGH) {
      pins[pin] = true;
    } else if (pin >= 4) {
      pins[pin] = false;
    }
  } 

  char keys[] = {'1', '2', '3', 'A', '4', '5', '6', 'B', '7', '8', '9', 'C', '*', '0', '#', 'D'};
  
  int index = 0;
  
  for (int row = 0; row < 4; row++) {
    for (int column = 4; column < 8; column++) {
      sendEvent(row, column, index, keys[index]);
      index++;
    }
  }

  delay(100);
}

// The selectMuxPin function sets the S0, S1, and S2 pins
// accordingly, given a pin from 0-7.
void selectMuxPin(byte pin)
{
  for (int i=0; i<3; i++)
  {
    if (pin & (1<<i))
      digitalWrite(selectPins[i], HIGH);
    else
      digitalWrite(selectPins[i], LOW);
  }
}

void sendEvent(int row, int column, int index, char key) {
  if ((pins[row] && pins[column]) && !keys[index]) {
    keys[index] = true;
    Serial.println("{'type': 'keypad', 'keypush': '" + String(key) + "'}");
    //Wire.beginTransmission(SLAVE_ADDRESS);
    //Wire.write("kp;" + key);
    //Wire.endTransmission();
  }

  if ((!pins[row] || !pins[column]) && keys[index]) {
    keys[index] = false;
  }
}

/*void requestEvent() {
  Wire.write("hello "); // respond with message of 6 bytes
  // as expected by master
}*/