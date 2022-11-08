#!/usr/bin/python

import RPi.GPIO as GPIO
import smbus
import time
import sys

I2C_BUS = 1
I2C_SLAVE = 0x2a

INTERRUPT_PIN = 17

if __name__ == '__main__':
  # Initialize the interrupt pin...
  GPIO.setmode(GPIO.BCM)
  GPIO.setup(INTERRUPT_PIN, GPIO.IN)

  # Initialize the RPi I2C bus...
  i2c = smbus.SMBus(I2C_BUS)

  while True:
    try:
      GPIO.wait_for_edge(INTERRUPT_PIN, GPIO.RISING)

      try:
        key = chr(i2c.read_i2c_block_data(I2C_SLAVE, 0x00, 1)[0])
        print(key)
      except IOError:
        continue

    except KeyboardInterrupt:
      GPIO.cleanup()
      sys.exit()
