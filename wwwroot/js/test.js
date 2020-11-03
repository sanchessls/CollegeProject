//"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/clock").build();

//Disable send button until connection is established

connection.on("ShowTime", function (datetime) {
    console.log("ShowTime");
    console.log(datetime);

});

connection.start().then(function () {
    //document.getElementById("sendButton").disabled = false;
    console.log("passou aqui no start");
}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});