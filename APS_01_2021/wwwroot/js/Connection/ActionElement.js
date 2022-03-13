function ActUser() {
    const IsCurrentUser = (nickname) => {
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
        let messagediv = document.createElement("div");
        let chatareadiv = document.getElementById("chat_area");
        let nameparagraph = `<p> ${data.whoSendMessage} </p>`;
        let messageparagraph = `<p>${data.message}</p>`;
        let timeparagraph = `<p> horário: ${data.time} </p>`;
        let isCurrentUser = ActUser().IsCurrentUser(data.whoSendMessage)
        let showMessage = false

        if (isCurrentUser || ActContactMeet().GetSelectedContactMeet().receiver == data.whoSendMessage) {
            console.log("chegando aqui")
            showMessage = true
        }

        if (showMessage) {
            messagediv.classList.add("chat-message-content");

            if (ActUser().IsCurrentUser(data.whoSendMessage)) {
                messagediv.classList.add("user");
                messagediv.innerHTML = messageparagraph + timeparagraph;
            } else {
                messagediv.innerHTML = nameparagraph + messageparagraph + timeparagraph;
            }

            chatareadiv.appendChild(messagediv);
            chatareadiv.scrollTop = chatareadiv.scrollHeight;
        }
    }

    const ShowAllConversation = (data) => {
        let chatarea = document.getElementById("chat_area");
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

    const SelectContactMeet = (type, contactmeetid, url) => {
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

        document.getElementById("message-box").value = "";
    }

    const InviteContact = (url) => {
        let inviteContact = document.getElementById("iInviteContactInput").value

        if (inviteContact != null) {
            Contacts().InviteContact(url, inviteContact)
        }

        document.getElementById("iInviteContactInput").value = ""
    }

    return { GetSelectedContactMeet, SelectContactMeet, InviteContact }
}


