var map;
var visorImagen;

function initMap(points) {
    // pointInfo = {
    //     "Titulo":"",
    //     "Subtitulo":"",
    //     "Latitud":0,
    //     "Longitud":0,
    //     "IdOficina":0
    // }

    //iniciarGoogleMaps(desc, lat, lon);
    iniciarBingMaps(points);

    // Inicializar visor de imagen
    visorImagen = document.getElementById("visorImg");
    visorImagen.addEventListener("click", visorImagenClick); 
}
function iniciarGoogleMaps(titulo, lat, lon, subtitulo){
    var latlng = new google.maps.LatLng(lat, lon);
    var options = {
        zoom: 19,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    const map = new google.maps.Map(document.getElementById ("map"), options);

    const markPosition = { lat: lat, lng: lon};
    new google.maps.Marker({
        position: markPosition,
        map,
        title: titulo,
    });
}
function iniciarBingMaps(points){
    var firstPoint = points[0];

    var initZoom = 19;
    if(points.length > 1){
        initZoom = 16;
    }
    // Inicializar mapa
    map = new Microsoft.Maps.Map(document.getElementById('map'), {
        center: new Microsoft.Maps.Location(firstPoint.Latitud, firstPoint.Longitud),
        mapTypeId: Microsoft.Maps.MapTypeId.aerial,
        zoom: initZoom
    });
    Microsoft.Maps.Events.addHandler(map, 'viewrendered', function () {
        ajustarPosisionVisorImagen();
    });

    // Agregar los pushpin
    points.forEach(point => {
        var pushpin = new Microsoft.Maps.Pushpin(
            new Microsoft.Maps.Location(point.Latitud, point.Longitud),
            {
                title: point.Titulo,
                subTitle: point.Subtitulo,
                color:'#009bc1'
            }
        );
        map.entities.push(pushpin);
        // Agregar evento de click a los pushpin
        Microsoft.Maps.Events.addHandler(pushpin, 'click', function () { pushpinClick(point); });
    });
}


function moverMapa(lat, lon, zoom){
    map.setView({
        center: new Microsoft.Maps.Location(lat, lon),
        zoom: zoom
    });
}
function ajustarPosisionVisorImagen(){
    var mapElement = document.getElementById("map");
    var left = mapElement.offsetLeft;
    var top = mapElement.offsetTop;
    
    visorImagen.style.left = (left + 10) + "px";
    visorImagen.style.top = (top + 10) + "px";
}
function modificarVisorVisible(visible){
    if(visible){
        visorImagen.style.display = "block";
    }else{
        visorImagen.style.display = "none";
    }
}
function pushpinClick(point){

    // Remover imagen anterior
    var imagenTag = document.getElementById("imagenSicem");
    imagenTag.src = "";

    var idCuenta = point.Titulo;
    var idOficina = point.IdOficina;

    //Obtener idImagen de la cuenta
    var request = new XMLHttpRequest();
    request.open('GET', "http://arquos.capa.gob.mx/api/ListaImagenes/" + idOficina + "/" + idCuenta);
    request.send();
    request.onerror = function(){
        modificarVisorVisible(false);
    }
    request.onload = function(){
        var listaImagenes = JSON.parse(request.response);
        console.dir(listaImagenes);
        if(listaImagenes.length > 0 ){
            var idImagen = listaImagenes[0];
            imagenTag.src = "http://arquos.capa.gob.mx/api/Documento/" + idOficina + "/" + idImagen;
            modificarVisorVisible(true);
        }else{
            modificarVisorVisible(false);
        }
    }
}

function visorImagenClick(){
    visorImagen.classList.toggle("agrandar");
}

