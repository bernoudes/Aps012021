function ActUser() {
    const IsCurrentUser = (nickname) => {
        console.log(username + nickname)
        if (nickname == username) {
            return true
        }
        return false
    }

    return { IsCurrentUser }
}

function ActMessage() {
    const SendMessage = (url) => {
        let message = (document.getElementById("message-box").value)
        let result = Messages().SendMessage(url,message)
        document.getElementById("message-box").value = "";
    }

    const ShowMessage = (data) => {
        var messagediv = document.createElement("div");
        var nameparagraph = `<p> ${data.whoSendMessage} </p>`;
        var messageparagraph = `<p>${data.message}</p>`;
        var timeparagraph = `<p> horário: ${data.time} </p>`;

        messagediv.classList.add("chat-message-content");

        if (ActUser().IsCurrentUser(data.whoSendMessage)) {
            messagediv.classList.add("user");
            messagediv.innerHTML = messageparagraph + timeparagraph;
        } else {
            messagediv.innerHTML = nameparagraph + messageparagraph + timeparagraph;
        }

        document.getElementById("chat_area").appendChild(messagediv);
    }

    const ShowAllConversation = (data) => {
        var chatarea = document.getElementById("chat_area");
        while (chatarea.firstChild) {
            chatarea.removeChild(chatarea.firstChild)
        };

        for (let i = 0; i < data.length; i++) {
            ShowMessage(data[i])
        }
    }


    return { SendMessage, ShowMessage, ShowAllConversation  }
}

function ActContactMeet() {
    const GetSelectedContactMeet = () => {
 
        let room = { type: "contact", receiver: contactmeetselected.substring(7) }
        return room
    }

    const SelectContactMeet = (type, contactmeetid,url) => {
        if ( contactmeetselected != contactmeetid ) {
            let element = document.getElementById(contactmeetid)
            if (contactmeetselected != '') {
                let selected = document.getElementById(contactmeetselected)
                selected.classList.remove('selected')
            }
            element.classList.add('selected')
        }

        contactmeetselected = contactmeetid

        if (type == 'contact') {
            Messages().SelectChatContact(url,contactmeetid.substring(7))
        } else {
            Messages().SelectChatMeet(url,contactmeetid.substring(7))
        }
    }

    return { GetSelectedContactMeet, SelectContactMeet }
}


