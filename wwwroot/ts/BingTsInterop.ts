let bingMap: BingMap;

class BingMap {
    map: Microsoft.Maps.Map;

    constructor() {
        this.map = new Microsoft.Maps.Map('#myMap', {
            center: new Microsoft.Maps.Location(20.213382, -87.436819),
            mapTypeId: Microsoft.Maps.MapTypeId.road,
            zoom: 8
        });
    }
}

function loadMap(): void {
    bingMap = new BingMap();
}