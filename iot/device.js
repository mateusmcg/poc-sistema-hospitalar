var awsIot = require("aws-iot-device-sdk");
var configs = require("./configs/configs.json");

// Config
var device = awsIot.device({
  keyPath: configs.keyPath,
  certPath: configs.certPath,
  caPath: configs.caPath,
  host: configs.host
});

// Connect
device.on("connect", function() {
  console.log("Connected");

  // Subscribe to myTopic
  device.subscribe("myTopic");

  // Publish to myTopic
  publishToTopic("myTopic", { test: "Just testing the AWS IoT" }, 2000);
});

// Error
device.on("error", function(error) {
  console.log("Error: ", error);
});

// Subscription
device.on("message", function(topic, payload) {
  console.log("message", topic, payload.toString());
});

function publishToTopic(topic, content, interval) {
  setTimeout(() => {
    device.publish(topic, JSON.stringify(content));
    publishToTopic(topic, content, interval);
  }, interval);
}
