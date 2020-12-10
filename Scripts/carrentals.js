$(document).ready(function () {
   
    $("#box").css({
        'width': '70%!important',
        'height': '70%!important',
        'margin': 'auto!important',
        'position': 'relative!important'
    });

    
    $("#toolbox").css({
        'width': '70%!important',
        'height': '70%!important',
        'margin': 'auto!important',
        'position': 'relative!important'
    });
    var box = ($(window).width() - $("#box").width()) / 2;

    $("#toolbox").css({
        'left': box + 'px',
        'top':'30px'
    });

});