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
        ActContactMeet().PutContactFirst(ActContactMeet().GetSelectedContactMeet().receiver)
    }

    const ShowMessage = (data) => {
        let isCurrentUser = ActUser().IsCurrentUser(data.WhoSendMessage)
        let showMessage = false

        if (isCurrentUser || ActContactMeet().GetSelectedContactMeet().receiver == data.WhoSendMessage) {
            showMessage = true
        }

        if (showMessage) {
            let messagediv = document.createElement("div");
            let chatareadiv = document.getElementById("chat_area");
            let nameparagraph = `<p> ${data.WhoSendMessage} </p>`;
            let messageparagraph = `<p>${data.Message}</p>`;
            let timeparagraph = `<p> horário: ${data.Time} </p>`;

            messagediv.classList.add("chat-message-content");

            if (ActUser().IsCurrentUser(data.WhoSendMessage)) {
                messagediv.classList.add("user");
                messagediv.innerHTML = messageparagraph + timeparagraph;
            } else {
                messagediv.innerHTML = nameparagraph + messageparagraph + timeparagraph;
            }

            chatareadiv.appendChild(messagediv);
            chatareadiv.scrollTop = chatareadiv.scrollHeight;
        }
        else {
            ActContactMeet().PutContactFirst(data.WhoSendMessage)
            ActContactMeet().MarkMessageNotRead(data.WhoSendMessage)
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
    const StatusConnList = [
        "l-contacts-ball-online",
        "l-contacts-ball-busy",
        "l-contacts-ball-absent",
        "l-contacts-ball-offline"
    ]


    const GetSelectedContactMeet = () => {
 
        let room = { type: "contact", receiver: contactmeetselected.substring(7) }
        return room
    }

    const SelectContactMeet = (type, contactmeetid, url) => {
        if (contactmeetselected != contactmeetid) {
            let element = document.getElementById(contactmeetid)
            if (contactmeetselected != '') {
                let selected = document.getElementById(contactmeetselected)
                selected.classList.remove('selected')
            }
            element.classList.add('selected')
            element.classList.remove('l-message-not-read')

            contactmeetselected = contactmeetid

            SendSeeTheMessage()
        }


        if (type == 'contact') {
            Messages().SelectChatContact(url,contactmeetid.substring(7))
        } else {
            Messages().SelectChatMeet(url,contactmeetid.substring(7))
        }

        document.getElementById("message-box").value = "";
    }

    const SendSeeTheMessage = () => {
        let url = "/ChatMessage/UserSeeTheMessage"
        Communication().SendData(url, GetSelectedContactMeet())
    }

    const SendInviteContact = (url) => {
        let inviteContact = document.getElementById("iInviteContactInput").value
        if (inviteContact != null) {
            Contacts().InviteContact(url, inviteContact)
        }

        document.getElementById("iInviteContactInput").value = ""
    }

    const CallInviteContactBox = (url, data) => {
        let ndata = { inviterNickName: data }
        let boxcontent = Communication().CallPopupBox(url, ndata)
    }

    const CallMeetCreateBox = () => {
        let url = "/Meet/Create"
        let boxcontent = Communication().CallPopupBox(url, null)
    }

    const CallInviteAcceptBox = () => {
        let boxcontent = Communication().CallPopupBox("/cvc")
        if (boxcontent != "fail") {
        }
    }

    const CallDeleteContactBox = (url) => {
        let boxcontent = Communication().CallPopupBox(url, GetSelectedContactMeet())
    }

    const ReceiveInviteContact = (url, data) => {
        let boxcontent = Communication().CallPopupBoxJsonData(url, data)
        if (boxcontent != "fail") {
            
        }
    }

    const ReturnClassStatus = (status) => {
        let result = "offline"

        switch (status) {
            case "ONLINE":
                result = StatusConnList[0]
                break
            case "BUSY":
                result = StatusConnList[1]
                break
            case "ABSENT":
                result = StatusConnList[2]
                break
            default:
                result = StatusConnList[3]
        }
        return result
    }

    const AddContacts = (data) => {
        //data = {statusconn, contact, messageNotRead}
        let elementlistContact = document.getElementById("il-contact-content")

        if (elementlistContact.firstElementChild.getAttribute("id") == null) {
            elementlistContact.innerHTML = ""
        }

        let element = document.createElement("div")
        let innerElement = document.createElement("div")
        let text = document.createTextNode(`${data.contact}`)
        let classStatus = ReturnClassStatus(data.statusconn)
        let classMessageNotRead = data.messageNotRead == true ? "l-message-not-read" : ""

        innerElement.setAttribute("class", `l-contacts-ball ${classStatus} `)
        element.setAttribute("id", `idCont-${data.contact}`)
        element.setAttribute("class", `l-contacts-line ${classMessageNotRead}`)
        element.setAttribute("onclick", "onclickAux(this.id)")
        element.appendChild(innerElement)
        element.appendChild(text)
        elementlistContact.appendChild(element) 
    }

    const UpdateStatusConn = (data) => {
        //data = {statusconn, nickname}
        let ids = `idCont-${data.nickname}`
        let classStatus = ReturnClassStatus(data.status)
        let element = ActAuxFunc().ChildrenWithOutTextNode(document.getElementById(ids))[0]

        for (let i = 0; i < StatusConnList.length; i++) {
            element.classList.remove(StatusConnList[i])
        }
        element.classList.add(classStatus)
    }

    const PutContactFirst = (data) => {
        let elementlistContact = document.getElementById("il-contact-content")
        let elementchild = ActAuxFunc().ChildrenWithOutTextNode(elementlistContact)
        let newElementChildList = []

        if (elementchild.length > 1) {
            for (let i = 0; i < elementchild.length; i++) {
                if (elementchild[i].id == `idCont-${data}`) {
                    newElementChildList.unshift(elementchild[i])
                }
                else {
                    newElementChildList.push(elementchild[i])
                }
            }
            elementlistContact.innerHTML = ""

            for (let i = 0; i < elementchild.length; i++) {
                elementlistContact.appendChild(newElementChildList[i])
            }
        }
    }

    const MarkMessageNotRead = (data) => {
        let elementlistContact = document.getElementById("il-contact-content")
        let elementchild = ActAuxFunc().ChildrenWithOutTextNode(elementlistContact)

       for (let i = 0; i < elementchild.length; i++) {
           if (elementchild[i].id == `idCont-${data}`) {
               elementchild[i].classList.add("l-message-not-read")
           }
        }
    }
    

    return {
        GetSelectedContactMeet, SelectContactMeet, SendInviteContact, CallDeleteContactBox, PutContactFirst, SendSeeTheMessage,
        MarkMessageNotRead, CallInviteContactBox, ReceiveInviteContact, CallInviteAcceptBox, AddContacts, UpdateStatusConn, CallMeetCreateBox
    }
}

function ActInviters() {
    const InviteAccept = (contact) => {
        InviteAJ(contact, true)
    }

    const InviteReject = (contact) => {
        InviteAJ(contact, false)
    }

    const InviteAJ = (contact, chose) => {
        let url = "/InviteContact/RunAccept"
        let ids = `ibts-invite-${contact}`
        let element = document.getElementById(ids)
        let parentElement = document.getElementById(ids).parentElement
        let data = { contactNickName: contact, chose: chose }

        Communication().SendData(url, data)
        element.style = "display: none;"
        if (!ActAuxFunc().haveChildrenDisplayNotNone(parentElement)) {
            ShowContentNotInviteByElement(parentElement)
        }
    }

    const ShowContentNotInvite = (idelement) => {
        let element = document.getElementById(idelement)
        ShowContentNotInviteByElement(element)
    }

    const ShowContentNotInviteByElement = (element) => {
        let inner = "<bold>Você não tem convites</bold>"
        let newElement = document.createElement("p")

        newElement.innerHTML = inner
        element.classList.remove("l-contact-content")
        element.classList.add("invite-content")
        element.classList.add("d-flex")
        element.classList.add("align-items-center")
        element.classList.add("justify-content-center")

        element.appendChild(newElement)
    }

    return { InviteAccept, InviteReject, ShowContentNotInvite }
}

function ActAuxFunc() {

    const haveChildren = (element) => {
        return element.childElementCount > 0 ? true : false
    }

    const haveChildrenDisplayNotNone = (element) => {
        let childs = [];
        for (let i = 0; i < element.childNodes.length; i++) {
            if (element.childNodes.item(i).nodeName != "#text" && element.childNodes.item(i).style.display != "none")
                childs.push(element.childNodes.item(i))
        }
        return childs.length > 0 ? true : false
    }

    const ChildrenWithOutTextNode = (element) => {
        let childs = [];
        for (let i = 0; i < element.childNodes.length; i++) {
            if (element.childNodes.item(i).nodeName != "#text" && element.childNodes.item(i).style.display != "none")
                childs.push(element.childNodes.item(i))
        }
        return childs
    }

    return { ChildrenWithOutTextNode, haveChildrenDisplayNotNone, haveChildren}
}