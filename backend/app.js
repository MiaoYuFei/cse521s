import express from 'express';
import bodyParser from 'body-parser';
import { iot, mqtt, io } from "aws-iot-device-sdk-v2";
const decoder = new TextDecoder('utf8');

const app = express();
app.use(bodyParser.json());
app.use(express.json());

app.use(bodyParser.urlencoded({ extended: true }));

app.get('/', (req, res) => {
  res.send('Hello World!');
});

app.post('/getExampleData', (req, res) => {
  const result = {
    title: "POST request with Axios",
    body: "POST request",
    userId: 10,
  };
  res.json(result);
});

// Setup AWS IoT
let iotConfigBuilder = iot.AwsIotMqttConnectionConfigBuilder.new_with_websockets({
  region: "us-east-2",
  credentials_provider: undefined
});
iotConfigBuilder.with_client_id("web-client");
iotConfigBuilder.with_credentials("us-east-2", "AKIA4VOJVMETAAK4YAAV", "hlsGKxO8DL1V1o8FXd6YZztY6nC6HpSUFL/f1ldB", "");
iotConfigBuilder.with_endpoint("a38tjqu822q0m8-ats.iot.us-east-2.amazonaws.com");
const iotConfig = iotConfigBuilder.build();
const iotClient = new mqtt.MqttClient(new io.ClientBootstrap());
const iotConn = iotClient.new_connection(iotConfig);
const iotTopicTagScanResult = "RFIDReader/TagScanResult";
const iotTopicStatus = "RFIDReadr/Status";
iotConn.on("connect", () => {
  console.log("[AWS IoT] Connected");
  iotConn.subscribe(iotTopicStatus, mqtt.QoS.AtLeastOnce, onIotStatusReceived);
  iotConn.subscribe(iotTopicTagScanResult, mqtt.QoS.AtLeastOnce, onIotTagScanResultReceived);
});
iotConn.on('disconnect', (eventData) => {
  console.log("[AWS IoT] Disconnected: " + eventData);
});
iotConn.on("error", (eventData) => {
  console.log("[AWS IoT] Error: " + eventData);
});
iotConn.connect();

async function onIotTagScanResultReceived(topic, payload, dup, qos, retain) {
  const json = JSON.parse(decoder.decode(payload));
  console.log(json);
}

async function onIotStatusReceived(topic, payload, dup, qos, retain) {
  const json = JSON.parse(decoder.decode(payload));
  console.log(json);
}

// Listen for API requests
const port = process.env.PORT || 3000;
app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
