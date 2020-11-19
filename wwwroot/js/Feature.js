"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/feature").build();

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

connection.on("FeatureUpdated", function (FeatureId,Id) {
    console.log("FeatureUpdated");
    console.log("Feature ID : " + FeatureId);
    console.log("User ID : " + Id);   

    CallFunctionFromJS();
});
