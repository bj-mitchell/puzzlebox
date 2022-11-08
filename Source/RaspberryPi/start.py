#!/usr/bin/python

import RPi.GPIO as GPIO
import smbus
import time
import sys

def keypad():
  I2C_BUS = 1
  I2C_SLAVE = 0x2a
  INTERRUPT_PIN = 17

  GPIO.setmode(GPIO.BCM)
  GPIO.setup(INTERRUPT_PIN, GPIO.IN)

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
