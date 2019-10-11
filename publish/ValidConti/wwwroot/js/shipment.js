"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/shipmentHub").build();
let rmbr = 0;
var arreglo = [''];
connection.on("ReceivePallet", function (succes, data) {
    var obj = JSON.parse(data);

    let str = succes;
    if (succes == 1) {

        str =
            "<p>PALLET VALIDO " + obj.continentalpartnumber+ "</p> " +
            "<p class='text-align-center'>" +
            "<img src='../images/ok.png' width='600' height='600'>" +
            "</p > ";
        rmbr = 1;

    } else if (succes == 2) {

        str = "<p>NO VALIDO</p><p class='text-align-center'>No. de Parte: " + obj.continentalpartnumber + "; Leído: " + obj.Reading + "</p><p class='text-align-center'><img src='../images/cancel.png' width='600' height='600' /></p>";
        rmbr = 2;


    } else if (succes == 3) {

        str = "<p>EMBARQUE TERMINADO</p><p class='text-align-center'>" + "" + "</p><p class='text-align-center'><img src='../images/terminado.png' width='600' height='600'  /></p>";
        rmbr = 3;
    } else if (succes == 4) {

        str = "<p>EMBARQUE TERMINADO</p><p class='text-align-center'>No. de Parte: " + obj.continentalpartnumber + " No pertenece al embarque. " + "" + "</p><p class='text-align-center'><img src='../images/precaucion.png' width='600' height='600' /></p>";
        rmbr = 4;


    } else if (succes == 5) {

        str = "<p>Esperando</p>" +
            "<p class='text-align-center'>" +
            "<img src='../images/esperando.png' width='600' height='600'>" +
            "</p>";
        rmbr = 5;
    }
    $("#dataShip").html(str);
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error("start" + err.toString());
});

// Metodo que cada 2 décimas se ejecuta y manda llamar el metodo Pallets() en ChatHub
setInterval(function () {
    console.log("set interval", rmbr);
    var portal = document.getElementById("portal").value;
    var embarque = document.getElementById("embarque").value;
    connection.invoke("Pallets", portal, embarque, rmbr).catch(function (err) {
        return console.error("Pallet" + err.toString());
    });
}, 200);

// 1  Valido
// 2  No valido
// 3  Embarque terminado
// 4  No pertenece al embarque ademas de estar finalizado
// 5  Esperando a iniciar el embarque