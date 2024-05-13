function loadChat(name) {
    $(".chat-wrapper.shown").removeClass("shown");
    $("#select-chat-div").hide();

    $chat = $('*[data-recipient="' + name + '"]');
    $chat.addClass("shown");

    $("#notification_" + name).text(0);
    scrollToBottom();
}

$(".write input").keyup(function (event) {
    if (event.keyCode === 13) {
        $("#btn-send").click();
    }
});