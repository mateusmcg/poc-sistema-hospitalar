const awsIot = require("aws-iot-device-sdk");
const configs = require("./configs/configs.json");
const uuidv1 = require('uuid/v1');

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
  device.subscribe("hospital");

  // Publish to myTopic
  publishToTopic("hospital", getPacienteAleatorio(), 2500);
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
    publishToTopic(topic, getPacienteAleatorio(), interval);
  }, interval);
}

function getRandomInt(min, max) {
  min = Math.ceil(min);
  max = Math.floor(max);
  return Math.floor(Math.random() * (max - min)) + min;
}

function getPacienteAleatorio(){
  const pressaoSistolica = getRandomInt(100, 200);
  const pressaoDiastolica  = getRandomInt(60, 130);
  return { 
    pacienteId: uuidv1(), 
    timestamp: new Date(), 
    pressaoSistolica: pressaoSistolica,
    pressaoDiastolica: pressaoDiastolica,
    pressaoResumida: pressaoSistolica.toString() + "/" + pressaoDiastolica.toString()
  }
}
