#!/usr/bin/python

import websockets
import asyncio
import RPi.GPIO as GPIO
import smbus
import time
#import sys
#import serial

# Server data
I2C_BUS = 1
I2C_SLAVE = 0x2a
INTERRUPT_PIN = 17
RESET_PIN = 18

GPIO.setwarnings(False)

GPIO.setmode(GPIO.BCM)
GPIO.setup(INTERRUPT_PIN, GPIO.IN)
GPIO.setup(RESET_PIN, GPIO.OUT)
i2c = smbus.SMBus(I2C_BUS)

PORT = 7777

# Initialize the interrupt pin...


#ser = serial.Serial('/dev/ttyACM0', 9600, timeout=1)
#ser.reset_input_buffer()

print("Resetting devices...")
GPIO.output(RESET_PIN, 1)
time.sleep(0.2)
GPIO.output(RESET_PIN, 0)
#time.sleep(5)


print("Server listening on Port " + str(PORT))

# A set of connected ws clients
connected = set()

async def consumer_handler(websocket):
    async for message in websocket:
        await consumer(message)

async def producer_handler(websocket):
#  await websocket.send("Startup of raspberry pi server script")
  while True:
    try:
      message = await producer()
      await websocket.send(message)
    except Exception:
      break

async def handler(websocket):
  connected.add(websocket)
  consumer_task = asyncio.create_task(consumer_handler(websocket))
  producer_task = asyncio.create_task(producer_handler(websocket))
  done, pending = await asyncio.wait(
      [consumer_task, producer_task],
      return_when=asyncio.FIRST_COMPLETED,
  )
  for task in pending:
    task.cancel()

async def consumer(message):
  print("consumer> " + message.decode('utf-8'))

async def producer():
  try:
    GPIO.wait_for_edge(INTERRUPT_PIN, GPIO.RISING)
    
    key = chr(i2c.read_i2c_block_data(I2C_SLAVE, 0x00, 1)[0])
    print(key)
  except KeyboardInterrupt:
    GPIO.cleanup();
  return key.encode('utf-8')



start_server = websockets.serve(handler, "0.0.0.0", PORT)
asyncio.get_event_loop().run_until_complete(start_server)
asyncio.get_event_loop().run_forever()
