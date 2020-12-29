"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/feature").build();
connection.onclose(start);

function checkSessionConnection() {
    console.log("checkSessionConnection");
    console.log(connection);

    if (connection != null) {
        console.log("NotNull");


        console.log("State : " + connection.connectionState);

    }
}

async function start() {
    console.log("SignalR Connected2aaaa.");
    try {
        await connection.start();
        console.log("SignalR Connected2.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};


function checkFeatureConnection() {
    console.log("checkFeatureConnection");
    console.log(connection);
}

function EnterInGroupFeature(group) {
    console.log("EnterInGroupFeature");
    connection.start().then(function () {
        console.log("connection.start()");

        console.log("Inserting into SignalR Group : " + group);
        connection.invoke("JoinGroup", group).catch(err => console.error(err));
    }).catch(function (err) {
        return console.error(err.toString());
    });
}

connection.on("FeatureUpdated", function (FeatureId, Id) {
    console.log("FeatureUpdated");
    console.log("Feature ID : " + FeatureId);
    console.log("User ID : " + Id);

    CallFunctionFromJS();
});

