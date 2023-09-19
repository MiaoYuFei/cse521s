# cse521s

Smart Counter: IoT-based items tracking platform

## Frontend

Webpage UI, makes requests to backend restful API.

Node.js: Vue3 + Vite

How to run: `npm run dev`

## Backend

Accepts requests from frontend via restful API. Communicates with AWS IoT Core using MQTT protocol.

Node.js: Express

How to run: `npm run start`

## Firmware

Controls RFID receiver hardware, collects data, communicates with AWS IoT Core using MQTT protocol.

C# program on Raspberry Pi
