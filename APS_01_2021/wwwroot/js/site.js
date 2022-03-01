﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var listBoxCloseClick = [];

window.addEventListener('click', function (e) {
    listBoxCloseClick.forEach(x => {
        if (!document.getElementById(x).contains(e.target)) {
            ClosePopUpBoxById(x);
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
}

function sendInfoComponentPopUp(e, url, idmessage) {
    e.preventDefault()
    document.getElementById(idmessage).innerHTML = `<p>Espere...</p>`
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
}

function CloseAllPopUpBox()
{
    listBoxCloseClick.forEach(x => ClosePopUpBoxById(x))
}

function GetStringFirtIdText(text)
{
    var Tratamento01 = text.split("id=\"")
        .filter((element, index) => index == 1)
    var Tratamento02 = Tratamento01[0].split("\">")
    return Tratamento02[0]
}