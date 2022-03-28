function FrontActUpdate(data) {
    switch (data.whoCtrl)
    {
        case "Contact":
            FrontActUpdContact(data)
            break
        case "ChatMessage":
            FrontActUpdMesssage(data)
            break
    }
}

//********************MESSAGE**********************
function FrontActUpdMesssage(data) {
    console.log(data)
    switch (data.actSolicited) {
        case "ReceiveMessage":
            ActMessage().ShowMessage(JSON.parse(data.extraData))
            break
        case "ReceiveFullChat":
            ActMessage().ShowAllConversation(JSON.parse(data.extraData))
            break
    }
}

//********************CONTACT**********************
function FrontActUpdContact(data) {
    let dataj = JSON.parse(data.extraData)

    switch (data.actSolicited)
    {
        case "AddListContact":
            ActContactMeet().AddContacts(dataj)
            break
        case "UpdateStatusConn":
            ActContactMeet().UpdateStatusConn(dataj)
            break

    }
}

//********************INVITECONTACT**********************
function FrontActCall(data) {
    switch (data.whoCtrl) {
        case "InviteContact":
            FrontActCallInviteContact(data)
            break
    }
}

function FrontActCallInviteContact(data) {
    switch (data.actSolicited) {
        case "CallInviteContactOpt":
            ActContactMeet().CallInviteContactBox(`/${data.whoCtrl}/GetReceive`, data.extraData.replaceAll('"', ''))
            break
    }
}