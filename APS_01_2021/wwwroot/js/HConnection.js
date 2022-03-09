
/*
 * a função de conexão com chat vai ficar na parte inicial para que todos possam acessar
    funções necessarias:
        Contatos:
            verificar se o contato está ativo
            verificar se o contato está mandando mensagem para o usuario
        Usuario: 
            verificar se alguma mensagem chegou para o usuario
            entrar na sala de conversas
        Chat:
            envio e recebimento de mensagem
 */
//--------------------------------------------------------------------------
/*UPDATERS STATUS
function SendMyOnlineStatus(connection, status) {
    connection.invoke("SendMyOnlineStatus", status)
        .then(() => "OK")
        .catch((err) => console.log(err))
}

function GetAllTheContactsAndStatus(connection) {
    connection.invoke("GetAllContactsAndStatus", function (contactnamestatus) {
        contactnamestatus.foreach((item, index) => {
            AddClassForOnlineStatus(document.getElementById(`id${contactonlinelist.nickname}onstatus`))
        })
    })
}

function ReceiveContactStatusUpdate(connection) {
    connection.on("ReceiveContactUpdate", function (contactnamestatus) {
        contactnamestatus.foreach((item, index) => {
            AddClassForOnlineStatus(document.getElementById(`id${contactonlinelist.nickname}onstatus`))
            //ADD SOME SYMBOLS FOR TYPING
        })
    })
}


function AddClassForOnlineStatus(element) {
    let newelement = element

    switch (item.status) {
        case "ONLINE":
            classe = "l-contacts-ball-online"
            break;
        case "BUSY":
            classe = "l-contacts-ball-busy"
            break;
        case "ABSENT":
            classe = "l-contacts-ball-absent"
            break;
        default:
            classe = "l-contacts-ball-offline"
    }
    classe.concat(classe, " l-contacts-ball")
    newelement.classList.add(classe);

    return newelement
}
//--------------------------------------------------------------------------
//
function OpenConnection() {
    "use strict"

    var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
    var username = "";

    console.log('StartConnection')
}

function StartChatConnection(connection) {
    SendMyOnlineStatus()
    ReceiveContactStatusUpdate()
    let contact = GetAllTheContactsAndStatus()
}













function SendMyOnlineStatus(connection, status) {
    connection.invoke("SendMyOnlineStatus", status)
        .then(() => "OK")
        .catch((err) => console.log(err))
}

function GetAllTheContactsAndStatus(connection) {
    connection.invoke("GetAllContactsAndStatus", function (contactnamestatus) {
        contactnamestatus.foreach((item, index) => {
            AddClassForOnlineStatus(document.getElementById(`id${contactonlinelist.nickname}onstatus`))
        })
    })
}

function ReceiveContactStatusUpdate(connection) {
    connection.on("ReceiveContactUpdate", function (contactnamestatus) {
        contactnamestatus.foreach((item, index) => {
            AddClassForOnlineStatus(document.getElementById(`id${contactonlinelist.nickname}onstatus`))
            //ADD SOME SYMBOLS FOR TYPING
        })
    })
}


function AddClassForOnlineStatus(element) {
    let newelement = element

    switch (item.status) {
        case "ONLINE":
            classe = "l-contacts-ball-online"
            break;
        case "BUSY":
            classe = "l-contacts-ball-busy"
            break;
        case "ABSENT":
            classe = "l-contacts-ball-absent"
            break;
        default:
            classe = "l-contacts-ball-offline"
    }
    classe.concat(classe, " l-contacts-ball")
    newelement.classList.add(classe);

    return newelement
}






*/
function OpenConnection() {
    "use strict"

    var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
    var username = "";

    return connection;
}

//--------------------------------------------------------------------------
//--------------------------------------------------------------------------
//--------------------------------------------------------------------------
function connectionOld() {
    "use strict"

    var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
    var username = "";

    //Disable send button until connection is established
    document.getElementById("SendButton").disabled = true;

    //Receive Messages
    connection.on("ReceiveMessage", function (user, message) {
        /*var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");*/

        var messagediv = document.createElement("div");
        var nameparagraph = "<p> Bernardo </p>";
        var messageparagraph = "<p>" + message + "</p>";
        var timeparagraph = "<p> horário: 04:05 - 10/05/2021 </p>";

        //firstdiv.classList.add("user");
        messagediv.classList.add("chat-message-content");

        messagediv.innerHTML = nameparagraph + messageparagraph + timeparagraph;

        document.getElementById("chat_area").appendChild(messagediv);
    })

    //WHEN TO START CONNECTION
    connection.start().then(function () {
        document.getElementById("SendButton").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    })

    //SEND MESSAGE
    document.getElementById("SendButton").addEventListener("click", function (event) {
        var message = document.getElementById("message").value;

        connection.invoke("SendMessage", username, message).then(function () {
            document.getElementById("message").value = "";
        }).catch(function (err) {
            return console.error(err.toString());
        });

        event.preventDefault();
    })
}