function setStorage(key, valor){
    window.localStorage.setItem(key, valor);
}
function getStorage(key){
    return window.localStorage.getItem(key);
}


function ConfigurateDragg(element) {
    setTimeout(function () {
        $("#draggable").draggable({ containment: ".busy-control", scroll: false, handle: ".SicemDialog-header" });
        console.log("iniciando draggable");
    }, 800);
}
function ConfigurateDragg2(element) {
    setTimeout(function () {
        $("#draggable2").draggable({ containment: "#busy-control2", scroll: false, handle: ".SicemDialog-header" });
    }, 1000);
}

function ConfigurateDraggImagen(element) {
    console.log(">> Iniciando draggable imagen");
    console.dir(element);
    setTimeout(function () {
        $(element).draggable();
    }, 1000);
}

function ConfigurarVentanaDragg(target, container, handle ) {
    setTimeout(function () {
        /*$(target).draggable({ containment: container, scroll: false, handle: handle });*/
        var xParent = $(target).parent();
        $(target).draggable({ containment: xParent , scroll: false, handle: ".vs-header"});
        console.log(">> Iniciando ventana draggable target:" + target);
    }, 500);
}

function ObtenerTamanolEmento(element) {
    //Obtiene el ancho y alto de un elemento HTML
    var res = "";

    res += $(element).width();
    res += ";";
    res += $(element).height();

    return res;
}

function MostarLog(log) {
    console.dir(log);
}
function OpenNewTabUrl(targetUrl) {
    window.open(targetUrl, '_blank');
}
function downloadFromUrl(options){
    const anchorElement = document.createElement('a');
    anchorElement.href = options.url;
    anchorElement.download = options.fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}
function DownloadFileFromBytes(options){
    // options:{byteArray: string,  fileName: string,  contentType: string }
      
    // Convert base64 string to numbers array.
    const numArray = atob(options.byteArray).split('').map(c => c.charCodeAt(0));
    
    // Convert numbers array to Uint8Array object.
    const uint8Array = new Uint8Array(numArray);
    
    // Wrap it by Blob object.
    const blob = new Blob([uint8Array], { type: options.contentType });
    
    // Create "object URL" that is linked to the Blob object.
    const url = URL.createObjectURL(blob);
    
    // Invoke download helper function that implemented in 
    // the earlier section of this article.
    downloadFromUrl({ url: url, fileName: options.fileName });
    
    // At last, release unused resources.
    URL.revokeObjectURL(url);
      
}
function DownloadFileFromBase64(options){
    // options:{content: string,  fileName: string,  contentType: string }
    console.dir(options);
    var element = document.createElement('a');
    element.setAttribute('href','data:' + options.contentType +';charset=utf-8,' + encodeURIComponent(options.content));
    element.download = options.fileName ?? 'predios.kml';
    document.body.appendChild(element);
    element.click();
}

function FocusElement(id) {
    $("#" + id).focus();
}

function AgregarCSSClass(condition, clase_css){
    $(condition).addClass(clase_css);
}
function RemoverCSSClass(condition, clase_css){
    $(condition).removeClass(clase_css);
}

function ActiveToggleMenu(condition,condition2){
    $(condition).addClass("show");

    $(condition2).focusout( function(e){
        setTimeout(() => {
            $(condition).removeClass("show");            
        }, 200);

    });
}

function SetCss(target, cssMap) {
    console.log("Modificando css de " + target);
    console.dir(cssMap);

    $(target).css(cssMap);
}