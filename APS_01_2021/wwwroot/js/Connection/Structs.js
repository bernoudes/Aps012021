//--------------------------------------------------------------------------
//-------------------------------STRUCT AREA--------------------------------
//--------------------------------------------------------------------------
function Messages(connection) {
    const ActionStart = (functionAction) => {
        let check = false
        connection.start()
            .then(functionAction())
            .catch(err => check = false)
        return check
    }

    const SendMessage = (message) => {
        let check = false
        let room = ActContact().GetSelectedChat()
        console.log(room)
        // room { type : if is contact or meet, key : in contact nickname / in meet idMeeting}
        //type: user, and key: user for user
        
        if (room != null) {
            connection.invoke("SendMessage", room.type, room.key, message)
                .then(check = true)
                .catch(err => check = false)
                console.log(check)

        }
        return check
    }

    const ReceiveMessage = (functionAction) => {
        let check = false
        connection.on("ReceiveMessage", functionAction())
            .then(check = true)
            .catch(err => check = false)
        return check
    }

    const SelectChatContact = (contact) => {
        let check = true
        connection.invoke("SelectChatContact", contact)
            .catch(err => check = false)
    }

    const SelectChatMeet = (meet) => {
        let check = true
        connection.invoke("SelectChatMeet", meet)
            .catch(err => check = false)
    }

    const ReceiveAllChat = (functionAction) => {
        let check = true
        connection.invoke("ReceiveAllChat", functionAction)
            .then()
    }

    return { ActionStart, SendMessage, ReceiveMessage, ReceiveAllChat, SelectChatMeet, SelectChatContact }
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

