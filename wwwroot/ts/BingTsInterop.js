var bingMap;
var BingMap = /** @class */ (function () {
    function BingMap() {
        this.map = new Microsoft.Maps.Map('#myMap', {
            center: new Microsoft.Maps.Location(20.213382, -87.436819),
            mapTypeId: Microsoft.Maps.MapTypeId.road,
            zoom: 8
        });
    }
    return BingMap;
}());
function loadMap() {
    bingMap = new BingMap();
}
//# sourceMappingURL=BingTsInterop.js.map