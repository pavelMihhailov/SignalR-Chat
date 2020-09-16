"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatroom").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = `[${user}] написа ${msg}`;
    var li = document.createElement("li");
    li.classList.add("list-group-item");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("Update", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var li = document.createElement("li");
    li.classList.add("list-group-item");
    li.textContent = msg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;

    if ($("#chatRoom") !== null) {
        var roomName = $(".roomName")[0].innerText;
        connection.invoke("AddToGroup", roomName).catch(function (err) {
            return console.error(err.toString());
        });
    }
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    var roomId = $("#roomId").val();
    connection.invoke("SendMessage", roomId, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});