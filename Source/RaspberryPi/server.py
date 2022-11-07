#!/usr/bin/python

import websockets
import asyncio
import serial

# Server data
PORT = 7777
ser = serial.Serial('/dev/ttyACM0', 9600, timeout=1)
ser.reset_input_buffer()

print("Server listening on Port " + str(PORT))

# A set of connected ws clients
connected = set()

async def consumer_handler(websocket):
    async for message in websocket:
        await consumer(message)

async def producer_handler(websocket):
    while True:
        message = await producer()
        await websocket.send(message)

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
  key = ser.readline().decode('utf-8').rstrip()
  print (key)
  #message = "Hello world..."
  #await asyncio.sleep(1)
  #return message.encode('utf-8')
  return key.encode('utf-8')



# The main behavior function for this server
#async def echo(websocket, path):
#    print("A client just connected")
#    # Store a copy of the connected client
#    connected.add(websocket)
#    # Handle incoming messages
#    try:
#        async for message in websocket:
#            print("Received message from client: " + message.decode('utf-8'))
#            # Send a response to all connected clients except sender
#            for conn in connected:
#                if conn != websocket:
#                    await conn.send("Someone said: " + message)
#    # Handle disconnecting clients 
#    except websockets.exceptions.ConnectionClosed as e:
#        print("A client just disconnected")
#    finally:
#        connected.remove(websocket)

# Start the server
#start_server = websockets.serve(echo, "0.0.0.0", PORT)
#asyncio.get_event_loop().run_until_complete(start_server)
#asyncio.get_event_loop().run_forever()

start_server = websockets.serve(handler, "0.0.0.0", PORT)
asyncio.get_event_loop().run_until_complete(start_server)
asyncio.get_event_loop().run_forever()
