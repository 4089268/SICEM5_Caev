
function animacionLineaNerus(){
    $(".desarrollador").mouseenter(()=>{
        $("#nerus_line").animate({width:'120px',height:'1px'},600);
    });

    $(".desarrollador").mouseleave(()=>{
        $("#nerus_line").animate({width:'0px',height:'0px'},100);
    });
}

function fadeIn(target, time){

    setTimeout(() => {
        $(target).animate({
            opacity: 1,
            padding:"0"
          }, time);
    }, 200);

    $(".desarrollador").mouseenter(()=>{
        $("#nerus_line").animate({width:'120px',height:'1px'},600);
    });

}

function shake(target) {
    $(target).effect('shake', {}, 500, null);
}