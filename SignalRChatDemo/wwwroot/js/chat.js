const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

const sendButton = document.getElementById("sendButton");
sendButton.disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    const li = document.createElement("li");
    li.textContent = `${user}: ${message}`;
    document.getElementById("messagesList").appendChild(li);
});

connection.start()
    .then(function () {
        sendButton.disabled = false;
        console.log("Connected to SignalR hub.");
    })
    .catch(function (err) {
        console.error(err.toString());
    });

sendButton.addEventListener("click", function () {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;

    connection.invoke("SendMessage", user, message)
        .catch(function (err) {
            console.error(err.toString());
        });
});