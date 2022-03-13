//--------------------------------------------------------------------------
//-------------------------------STRUCT AREA--------------------------------
//--------------------------------------------------------------------------
function Communication(url, data) {
    const SendData = async () => {
        await $.post(url, data)
            .done(function () { return "OK" })
            .catch(err => "Error")
    }
    return { SendData }
}


function Messages() {
    const ActionStart = (functionAction) => {
        let check = false
        connection.start()
            .then(functionAction())
            .catch(err => check = false)
        return check
    }

    const SendMessage = (url,message) => {
        let check = false
        let room = ActContactMeet().GetSelectedContactMeet()
        let data = { type: room.type, receiver: room.receiver, message: message }
        // room { type : if is contact or meet, key : in contact nickname / in meet idMeeting}
        //type: user, and key: user for user
        
        if (room != null) {
            let result = Communication(url, data).SendData()
            check = result == "OK" ? true : false
        }
        return check
    }

    const SelectChatContact = (url, contact) => {
        let check = false
        let data = { type: 'contact', receiver: contact }
        let result = Communication(url, data).SendData()
        check = result == "OK" 
    }

    const SelectChatMeet = (url,meet) => {
        let check = false
        let result = Communication(url, meet).SendData()
        check = result == "OK"
    }


    return { ActionStart, SendMessage, SelectChatMeet, SelectChatContact }
}

//--------------------------------------------------------------------------
function Contacts(connection) {
    const ActionStart = (functionAction) => {
        let check = true
        connection.start()
            .then(functionAction())
            .catch(err => check = false)
        return check
    }

    const InviteContact = (nickname) => {
        let check = true
        connection.invoke("InviteContact", nickname)
            .then(check = true)
            .catch(err => check = false)
        return check
    }

    const ReceiveAllContact = (functionAction) => {
        let check = true
        connection.on("ReceiveContact", functionAction())
            .catch(err => check = false)
        return check
    }

    const ReceiveContactUpdate = (functionAction) => {
        let check = true
        connection.on("ReceiveContactUpdate", functionAction())
            .catch(err => check = false)
        return check
    }
    /*receber convite (ReciveContactInvite)*/

    return { ActionStart, InviteContact, ReceiveAllContact, ReceiveContactUpdate }
}

//--------------------------------------------------------------------------

function Meet(connection) {
    const ActionStart = (functionAction) => {
        let check = true
        connection.start()
            .then(functionAction())
            .catch(err => check = false)
        return check
    }

    const CreateMeet = (meetInfo) => {
        let check = true
        connection.invoke("CreateMeet", meetInfo)
            .catch(err => check = false)
        return check
    }

    const RemoveMeet = (meetInfo) => { // -> Somente Adm
        let check = true
        connection.invoke("RemoveMeet", meetInfo)
            .catch(err => false)
        return check
    }

    const MeetUpdate = (meetInfo) => {
        let check = true
        connection.invoke("UpdateMeet", meetInfo)
            .catch(err => false)
        return check
    }

    const ReceitveInviteMeet = (functionAction) => {
        let check = true
        connection.on("ReceiveInviteMeet", functionAction())
            .then(check = true)
            .catch(err => check = false)
    }

    const LeaveMeet = (meetinfo) => {
        let check = true
        connection.invoke("UpdateMeet", meetInfo)
            .catch(err => false)
        return check
    }

    return { ActionStart, CreateMeet, RemoveMeet, MeetUpdate, ReceitveInviteMeet, LeaveMeet }
}

