"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build(),
    connectionId;
//Disable send button until connection is established
$("#sendButton").disabled = true;
connection.start().then(function () {
    $("#sendButton").disabled = false;
    connection.invoke('getConnectionId', $('#myUsername').val())
        .then(function (Id) {
            connectionId = Id;
        });
}).catch(function (err) {
    return console.error(err.toString());
});
connection.on("RecieveMessage", function (message, time, to, countRecipient) {
    $('.chat-wrapper.shown .chat').append('<div class="bubble other"><span class="message-text">' + message + ' </span>'
        + '<span class="message-time">' + getTimeNow() + '</span></div>');

    $("#preview_" + to).text(message).removeClass("preview").addClass("name");
    $("#time_" + to).text(getTimeNow());
    $("#notification_" + to).removeAttr("hidden");    
    $("#notification_" + to).text(countRecipient);
    scrollToBottom();
})

var sendMessage = function (recipient) {
    event.preventDefault();

    var $text = $("#message-text").val();

    if ($text.trim() === "") {
        return; 
    }

    $.ajax({
        type: 'POST',
        url: '/Chat/SendMessage',
        data: { to: recipient, text: $text },
        cache: false,
        success: function () {
            $("#preview_"+recipient).text($text);
            $("#time_"+ recipient).text(getTimeNow());
            $(".write textarea").val('');
            $(".chat-wrapper.shown .chat").append('<div class="bubble me"><span class="message-text">' + $text + ' </span>'
                + '<span class="message-time">' + getTimeNow() + '</span></div>');

            $("#preview_" + recipient).removeClass("name").addClass("preview");
            scrollToBottom();
        },
        error: function () {
            alert("Failed to send message!");
        }
    });
}
function getTimeNow() {
    return new Date().toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit', hour12: true });
}


function scrollToBottom() {
    $(".chat-wrapper.shown .chat").animate({ scrollTop: $('.chat-wrapper.shown .chat').prop("scrollHeight") }, 500);
}