/**
 * @typedef {Object} MapMark
 * @property {number} latitude
 * @property {number} longitude
 * @property {string} descripcion
 * @property {string} subtitulo
 * @property {number} zoom
 */

/**
 * @typedef {Object} MapPoint
 * @property {number} latitude
 * @property {number} longitude
 */

const MAPCONTEXT = {
    pushpinColors: {
        "color1": "#29BC5F",
        "color2": "#fdd835",
        "color3": "#C63621",
    },
    map: null,
    markers: [],
    initZoom: 14
};

/**
 * 
 * @param {any} dotNetHelper 
 * @param {String} elementId 
 * @param {MapPoint} mapPoint 
 */
export function initialize(dotNetHelper, elementId, mapPoint) {
    
    const initZoom = MAPCONTEXT.initZoom;
    
    if( MAPCONTEXT.map != null ) {
        MAPCONTEXT.map.remove();
    }

    // initialize map
    MAPCONTEXT.map = L.map(elementId).setView([mapPoint.latitude, mapPoint.longitude], mapPoint.zoom ?? initZoom);
    
    // set the layer of streetmaps
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(MAPCONTEXT.map);


    // load data panel
    dotNetHelper.invokeMethodAsync('MapLoaded');

}

/**
 * 
 * @param {any} dotNetHelper 
 * @param {MapPoint} point 
 * @param {Number} zoom 
 */
export function moveMap(dotNetHelper, point, zoom) {
    
    MAPCONTEXT.map.setView([point.latitude, point.longitude], zoom !== null && zoom !== void 0 ? zoom : 17);
    //var marker = MAPCONTEXT.markers[point.idCuenta.toString()];
    var marker = MAPCONTEXT.markers.find( item => item.descripcion == point.descripcion);
    if (marker != null) {
        marker.openPopup();
    }
}

/**
 * 
 * @param {any} dotNetHelper 
 * @param {Array<MapMark>} marks 
 */
export function updateMarks(dotNetHelper, marks) {
    
    // clear the markers
    MAPCONTEXT.markers.forEach(m => {
        MAPCONTEXT.map.removeLayer(m);
    });
    MAPCONTEXT.markers = [];

    // Add pushpins
    MAPCONTEXT.points = marks;
    marks.forEach(point => {
        var marker = L.marker([point.latitude, point.longitude ], {
            opacity: 0.75,
            riseOnHover: true
        });
        marker.bindPopup(`<b>${point.descripcion}</b> <br/> ${point.subtitulo}`).openPopup();
        
        // added the event
        marker.on('click', function (e) {
            dotNetHelper.invokeMethodAsync('PushpinClick', point);
        });
        
        // save the reference of the marker
        MAPCONTEXT.markers.push(marker);
        
        // add the marker to the map
        MAPCONTEXT.map.addLayer(marker);

    });

}
