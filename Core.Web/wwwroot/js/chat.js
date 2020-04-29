"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/hub").build();
var channel = "abc";
//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on(channel, function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = "用户：" + user + " 发送消息： " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {

    console.log("连接服务成功.....")

    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {

    console.error("连接服务失败,自动重新连接......" + err.toString());

    setTimeout(() => start(), 5000);
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessageArr", channel, [user, message]).catch(function (err) {
        console.error("发送消息失败：" + err.toString());
    });
    event.preventDefault();
});