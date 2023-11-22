import express from 'express';
import bodyParser from 'body-parser';
import { iot, mqtt, io } from "aws-iot-device-sdk-v2";
const decoder = new TextDecoder('utf8');
import dbconn from './dbConfig.js';

const app = express();
app.use(bodyParser.json());
app.use(express.json());
app.use(bodyParser.urlencoded({ extended: true }));

app.post('/getAllTags', async (req, res) => {
  let payload = {
    "success": false
  };

  try {
    const [result] = await dbconn.execute('SELECT `tag_id`, `name`, `is_distractor` FROM `521tag`;');
    payload["success"] = true;
    payload["tags"] = result;
  }
  catch (err) {
    console.error(err);
    payload["success"] = false;
    payload["error"] = { "message": err.message };
  }
  finally {
    res.json(payload);
  }
});

app.post('/addTag', async (req, res) => {
  let payload = {
    "success": false
  };
  if (!req.body) {
    payload["success"] = false;
    payload["error"] = { "message": "Bad request" };
    res.status(400).json(payload);
    return;
  }
  let { tag_id: data_tag_id, name: data_name, is_distractor: data_is_distractor } = req.body;
  if (data_tag_id === undefined || data_name === undefined || data_is_distractor === undefined) {
    payload["success"] = false;
    payload["error"] = { "message": "Bad request" };
    res.status(400).json(payload);
    return;
  }
  if(data_is_distractor == "true"){
    data_is_distractor = 1;
  }
  else{
    data_is_distractor = 0;
  }
  try {
    const [result] = await dbconn.execute('INSERT INTO `521tag` (`tag_id`, `name`, `is_distractor`) VALUES (?, ?, ?);',
      [data_tag_id, data_name, data_is_distractor]);
    if (result.affectedRows <= 0) {
      payload["success"] = false;
      console.error("Failed to add tag.");
    } else {
      payload["success"] = true;
    }
  }
  catch (err) {
    console.error(err);
    payload["success"] = false;
    payload["error"] = { "message": err.message };
  }
  finally {
    res.json(payload);
  }
});


// API endpoint to get tag IDs
app.post('/getScanResult', (req, res) => {
  const myMap = {};
  const tagsValue = tagsList.slice();
  myMap["tags"] = tagsValue;
  res.json(myMap);
});

//delete a tag by its ID
app.delete('/deleteTag', async (req, res) => {
  try {
    const tagId = req.params.tagId;
    const connection = await dbPool.getConnection();

    await connection.query('DELETE FROM tags WHERE id = ?', [tagId]);
    connection.release();

    res.status(200).json({ message: 'Tag deleted successfully' });
  } catch (error) {
    console.error('Error deleting tag:', error);
    res.status(500).json({ message: 'Internal server error' });
  }
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

let tagsList = [];
// Received tag scan result from AWSIoT
async function onIotTagScanResultReceived(topic, payload, dup, qos, retain) {
  const json = JSON.parse(decoder.decode(payload));
  if ("tags" in json) {
    // Return the value associated with the key "tags"
    // update the list
    tagsList = json["tags"];
  } 
}

// Received reader status update from AWSIoT
async function onIotStatusReceived(topic, payload, dup, qos, retain) {
  const json = JSON.parse(decoder.decode(payload));
  console.log(json);
}

// Listen for API requests
const port = process.env.PORT || 3000;
app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});

// Close db connection when exit
process.on('SIGINT', async () => {
  await dbconn.end();
  process.exit();
});
