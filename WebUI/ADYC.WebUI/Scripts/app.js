var width = $(window).width();
var tempDesc = "";

$(document).ready(function () {

    // Using Zebra tooltips
    new $.Zebra_Tooltips($('.social'));

    $(".featured").hover(function () {
        
        var desc = $(this).find('.desc');
        var hDesc = desc.height();
        
        if (width <= 800) {
             tempDesc = desc.text();             
             desc.text(desc.text().split(" ", 15).join(" ") + '...');     
             var hDesc = desc.height();
        }

        $(this)
            .find('.title')
            .animate({'bottom': hDesc + 28})
            .css({'padding-bottom' : '0'});
        $(this)
            .find('.desc')
            .fadeIn();
    }, 
    function () { 
        console.log(tempDesc);
        $(this)
            .find('.title')
            .animate({'bottom': '0'})
            .css({'padding-bottom' : '16px'});
        $(this)
            .find('.desc')            
            .fadeOut();
        if (tempDesc) {
            $(this)
            .find('.desc').text(tempDesc);
        }
        
    });

    if (width > 800) {
        // Using titl pluging instead of adipoli
        $(".hoveri").tilt({
            glare: true,
            maxGlare: 0.5
        });
    }

    $("#search").click(function () {

        $("#search").hide();
        $("#div-src").show(1, function () { 
            $("#btn-src").show(1, function() {
                $("#input-src").show();
                $("#input-src").animate({
                    width : 100 + 'px'
                });
            });
         });

         return false;
    });

    $("#close-src").click(function () { 

        $("#div-src").hide(1, function () { 
            $("#btn-src").hide(1, function() {
                $("#input-src").hide();
                $("#input-src").animate({
                    width : 380 + 'px'
                });
            });
         });

         $("#search").show();

         return false;
    });

    $("#da-slider").cslider({
        current: 0, // index of current slide        
        autoplay: true, // slideshow on / off
        interval: 4000 // time between transitions
    });

 });

 (function repair_slider() {
     width = $(window).width();
     calculate = width * 0.10;
     var style = '<style>'+
        '.da-slide-current>h2 { ' +
            'margin-left: - ' + calculate + 'px !important;' +
        '}' +
        '</style>';
    if (width <= 800) {
        $('head').append(style);
        setTimeout(function() {
            $("#slide0>h2").removeAttr('style');
        }, 5000);
    }
     console.log(calculate);
 }());