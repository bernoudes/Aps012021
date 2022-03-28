//--------------------------------------------------------------------------
//-------------------------------STRUCT AREA--------------------------------
//--------------------------------------------------------------------------

function popUpStr() {
    const ShowPopUp = (htmldata) => {
        $("#popupParent").html(htmldata)
        document.getElementById("popupCenterAux").style.zIndex = 1;
        listBoxCloseClick.push(GetStringFirtIdText(htmldata))
    }
    const ClosePopUp = (boxid) => {
        document.getElementById(boxid).remove()
        document.getElementById("popupCenterAux").style.zIndex = -1;
    }

    const CloseAllPopUp = () => {
        listBoxCloseClick.forEach(x => ClosePopUpBoxById(x))
    }

    return { ShowPopUp, ClosePopUp, CloseAllPopUp}
}

//--------------------------------------------------------------------------
function Communication() {
    const SendData = async (url, data) => {
        await $.post(url, data)
            .done(function () { return "OK" })
            .catch(err => "Error")
    }

    const SendOptionData = async (url, selectOption, extradata) =>
    {
        let data = { selectOption: selectOption, extradata: extradata }
        SendData(url, data)
        popUpStr().ClosePopUp("iOptionBox")
    }

    const CallPopupBox = async (url, data) => {
        let result = ''
        await $.get(url, data)
            .done((datatwo) => {
                popUpStr().ShowPopUp(datatwo)
                result = "OK"
            })
            .fail((xhdr) => { result = 'fail' })
        return result
    }

    const CallPopupBoxJsonData = (url, data) => {
        let result = ''
        $.ajax({
            type: "GET",
            data: JSON.stringify(data),
            url: url,
            contentType: "application/json"
        });
        return result
    }

    const ReloadBox = async (url,parentId,posInBox) => {
        let result = ''
        await $.get(url)
            .done((datatwo) => {
                let element = document.createElement("div")
                element.innerHTML = datatwo

                document.getElementById(parentId).appendChild(element)
                result = "OK"
            })
            .fail((xhdr) => { result = 'fail' })
        return result
    }

    return { SendData, CallPopupBox, CallPopupBoxJsonData, ReloadBox, SendOptionData }
}

//--------------------------------------------------------------------------
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
            let result = Communication().SendData(url, data)
            check = result == "OK" ? true : false
        }
        return check
    }

    const SelectChatContact = (url, contact) => {
        let check = false
        let data = { type: 'contact', receiver: contact }
        let result = Communication().SendData(url, data)
        check = result == "OK" 
    }

    const SelectChatMeet = (url,meet) => {
        let check = false
        let result = Communication().SendData(url, meet)
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

    const InviteContact = (url,nickname) => {
        let check = false
        let data = { nickname }
        let result = Communication().SendData(url, data)
        check = result == "OK"
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

    const RefreshContacts = async (url) => {
        let result = ''
        await $.get(url)
            .done((datatwo) => {
                //pegar somente os elementos

                result = "OK"
            })
            .fail((xhdr) => { result = 'fail' })
        return result
    }
    /*receber convite (ReciveContactInvite)*/

     return { ActionStart, InviteContact, ReceiveAllContact, ReceiveContactUpdate, RefreshContacts }
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

