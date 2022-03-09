function ActMessage() {
    const SendMessage = () => {
        let message = (document.getElementById("message-box").value)
        let result = Messages(conn).SendMessage(message)
        document.getElementById("message-box").value = "";
    }

    const ShowMessage = (IsPrincipalUser, username, datatime, message) => {
        var messagediv = document.createElement("div");
        var nameparagraph = `<p> ${username} </p>`;
        var messageparagraph = `<p>${message}</p>`;
        var timeparagraph = `<p> horário: ${datatime} </p>`;

        if (IsPrincipalUser) {
            firstdiv.classList.add("user");
        }
        messagediv.classList.add("chat-message-content");

        messagediv.innerHTML = nameparagraph + messageparagraph + timeparagraph;

        document.getElementById("chat_area").appendChild(messagediv);
    }
    return { SendMessage, ShowMessage }
}

function ActContact() {
    const GetSelectedChat = () => {
        let room = { type : "contact", key: "Nuydia"}
        return room
    }

    return { GetSelectedChat }
}
