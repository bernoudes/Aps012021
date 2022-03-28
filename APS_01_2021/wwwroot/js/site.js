// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const listBoxCloseClick = [];

window.addEventListener('click', function (e) {
    listBoxCloseClick.forEach((item, index, object) => {
        if (!document.getElementById(item).contains(e.target)) {
            ClosePopUpBoxById(item);
            object.splice(index, 1);
        }
    })
})

function OpenComponentPopUp(UrlAction, idresultBox) {
    $.ajax({
        method: 'GET',
        url: UrlAction
    }).done(function (data, statusText, xhdr) {
        $(idresultBox).html(data);
        listBoxCloseClick.push(GetStringFirtIdText(data))
    }).fail(function (xhdr, statusText, errorText) {
        $(idresultBox).text(JSON.stringify(xhdr));
    });
    document.getElementById("popupCenterAux").style.zIndex = 1;
}

async function SendInfoComponentPopUpReturnData(url) {
    var message = "none"
    await $.ajax({
        method: 'POST',
        url: url
    }).done(function (data, statusText, xhdr) {
        if (data.message != null) {
            message = data.message;
        }
    }).fail(function (xhdr, statusText, errorText) {
        $(idresultBox).text(JSON.stringify(xhdr));
    });
    return message;
}

function sendInfoComponentPopUp(e, url, idmessage) {
    if (e != null) {
        e.preventDefault()
    }
    if (idmessage != null) {
        document.getElementById(idmessage).innerHTML = `<p>Espere...</p>`
    }
    $.ajax({
        method: 'POST',
        url: url
    }).done(function (data, statusText, xhdr) {
        if (idmessage != null && data.message != null) {
            document.getElementById(idmessage).innerHTML = `<p>${data.message}</p>`
        }
    }).fail(function (xhdr, statusText, errorText) {
        $(idresultBox).text(JSON.stringify(xhdr));
    });
}

function ClosePopUpBoxById(boxid)
{
    document.getElementById(boxid).remove()
    document.getElementById("popupCenterAux").style.zIndex = -1;
}

function CloseAllPopUpBox()
{
    console.log("passando aqui no close")
    listBoxCloseClick.forEach(x => ClosePopUpBoxById(x))
    document.getElementById("popupCenterAux").style.zIndex = -1;
    console.log("passando aqui no close")
}

function GetStringFirtIdText(text)
{
    var Tratamento01 = text.split("id=\"")
        .filter((element, index) => index == 1)
    var Tratamento02 = Tratamento01[0].split("\">")
    return Tratamento02[0]
}