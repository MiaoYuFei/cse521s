import { promises as fsPromises, constants } from 'fs';
const { access, unlink } = fsPromises;
import express from "express";
import bodyParser from "body-parser";
import { iot, mqtt, io } from "aws-iot-device-sdk-v2";
const decoder = new TextDecoder("utf8");
import dbconn from "./dbConfig.js";

const app = express();
app.use(bodyParser.json());
app.use(express.json());
app.use(bodyParser.urlencoded({ extended: true }));

app.post("/api/getAllTags", async (req, res) => {
  let payload = {
    "success": false
  };

  try {
    const [result] = await dbconn.execute("SELECT `tag_id`, `name`, `is_distractor` FROM `521tag`;");
    payload["success"] = true;
    payload["tags"] = result;
    payload["tags"].forEach((element) => {
      if (element["is_distractor"] === "1" || element["is_distractor"] === 1) {
        element["is_distractor"] = "true";
      } else {
        element["is_distractor"] = "false";
      }
    });
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

app.post("/api/addTag", async (req, res) => {
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
    const [result] = await dbconn.execute("INSERT INTO `521tag` (`tag_id`, `name`, `is_distractor`) VALUES (?, ?, ?);",
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

app.post("/api/getScanResult", async (req, res) => {
  const tagsIdArray = tagsList.slice();
  const myMap = {
    "success": false,
    "tags":[],
  };
  if (tagsIdArray.length <= 0) {
    myMap["success"] = true;
    res.json(myMap);
    return;
  }
  try {
    const query = `SELECT \`tag_id\`, \`name\`, \`is_distractor\` FROM \`521tag\` WHERE \`tag_id\` IN (${tagsIdArray.map(() => "?").join(",")})`;
    const [result] = await dbconn.execute(query, tagsIdArray);
    myMap["success"] = true;
    result.forEach((element) => {
      if (element["is_distractor"] === "1" || element["is_distractor"] === 1) {
        element["is_distractor"] = true;
      } else {
        element["is_distractor"] = false;
      }
    });
    const idsInList = result.map((item) => item.tag_id);
    const idsNotInList = tagsIdArray.filter((id) => !idsInList.includes(id));
    idsNotInList.forEach((element) => {
      const item = {
        "tag_id": element,
        "name": null,
        "is_distractor": true
      };
      result.push(item);
    });
    myMap["tags"] = result;
  }
  catch (err) {
    console.error(err);
    myMap["success"] = false;
    myMap["error"] = { "message": err.message };
  }
  finally {
    res.json(myMap);
  }
});

app.post("/api/deleteTag", async(req, res) => {
  let payload = {
    "success": false
  };
  
  const tag_id = req.body.tag_id; 
  try {
    const [result] = await dbconn.execute("DELETE FROM `521tag` WHERE `tag_id` = ?;", [tag_id]);
  
    if (result.affectedRows === 1) {
      payload["success"] = true;
      payload["message"] = `Tag with ID ${tag_id} deleted successfully.`;
    } else {
      payload["message"] = `Tag with ID ${tag_id} not found.`;
    }
  } catch (err) {
    console.error(err);
    payload["error"] = { "message": err.message };
  } finally {
    res.json(payload);
  }
});

app.post("/api/editTag", async (req, res) => {
  let payload = {
    "success": false
  };
  const tag_id = req.body.tag_id; // Get the tag ID from the URL parameter 
  if (!req.body) {
    payload["success"] = false;
    payload["error"] = { "message": "Bad request" };
    res.status(400).json(payload);
    return;
  }
  let { name: data_name, is_distractor: data_is_distractor } = req.body; 
  if (data_name === undefined || data_is_distractor === undefined) {
    payload["success"] = false;
    payload["error"] = { "message": "Bad request" };
    res.status(400).json(payload);
    return;
  }
  if (data_is_distractor === "true" || data_is_distractor === true) {
    data_is_distractor = 1;
  } else {
    data_is_distractor = 0;
  }
  
  try {
    const [result] = await dbconn.execute("UPDATE `521tag` SET `name` = ?, `is_distractor` = ? WHERE `tag_id` = ?;",
      [data_name, data_is_distractor, tag_id]);
  
    if (result.affectedRows === 1) {
      payload["success"] = true;
      payload["message"] = `Tag with ID ${tag_id} edited successfully.`;
    } else {
      payload["message"] = `Tag with ID ${tag_id} not found.`;
    }
  } catch (err) {
    console.error(err);
    payload["error"] = { "message": err.message };
  } finally {
    res.json(payload);
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
iotConn.on("disconnect", (eventData) => {
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
if (process.env.SOCK) {
  try {
    access(process.env.SOCK, constants.F_OK);
    unlink(process.env.SOCK);
  } catch (err) {
  }
}

const endpoint = process.env.SOCK || process.env.PORT || 3000;
app.listen(endpoint, () => {
  console.log(`Server is running on port ${endpoint}`);
});

// Close db connection when exit
process.on("SIGINT", async () => {
  await dbconn.end();
  process.exit();
});
