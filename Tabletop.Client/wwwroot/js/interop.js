function isChatScrolledToBottom() {
    var scroller = document.getElementById("chat-scroller");
    return scroller.scrollHeight - scroller.scrollTop - scroller.clientHeight < 1;
}

function scrollChatToBottom() {
    var scroller = document.getElementById("chat-scroller");
    scroller.scrollTop = scroller.scrollHeight;
}