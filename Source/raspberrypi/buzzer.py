#!/usr/bin/python

import RPi.GPIO as GPIO
import time
import signal
import sys

BUZZER_PIN = 12

GPIO.setwarnings(False)
GPIO.setmode(GPIO.BCM)
GPIO.setup(BUZZER_PIN, GPIO.OUT,initial=GPIO.LOW)
buzzer = GPIO.PWM(BUZZER_PIN, 1000)

def signal_handler(signum, frame):
  print("")
  print("Received break!  Shutting down...")
  GPIO.output(BUZZER_PIN, GPIO.LOW)
  GPIO.cleanup()
  sys.exit()

signal.signal(signal.SIGINT, signal_handler)

note_a = 440
note_b = 494
note_c = 523
note_d = 587
note_e = 659
note_f = 698
note_g = 784


#song = [ note_a, note_b, note_c, note_d, note_e, note_f, note_g ]

song = [
  note_f,
  note_f,
  note_d,
  note_d,
  note_a,
  note_a,
  note_b,
  note_b,
  note_c,
  note_b,
  note_a,
]

while True:
  for note in song:
    buzzer.ChangeFrequency(note)
    buzzer.start(10)
    time.sleep(0.4)
    buzzer.stop()
    time.sleep(0.1)
  #GPIO.output(BUZZER_PIN, GPIO.HIGH)
  #time.sleep(0.5)
  #time.sleep(0.5)
  
