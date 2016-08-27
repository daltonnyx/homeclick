(function($) {
	"use strict";
	$(document).ready(function () {
	    //    FaQS
	    $('.faq_question').click(function () {

	        if ($(this).parent().is('.open')) {
	            $(this).closest('.faq').find('.faq_answer_container').animate({ 'height': '0' }, 500);
	            $(this).closest('.faq').removeClass('open');

	        } else {
	            var newHeight = $(this).closest('.faq').find('.faq_answer').height() + 30 + 'px';
	            $(this).closest('.faq').find('.faq_answer_container').animate({ 'height': newHeight }, 500);
	            $(this).closest('.faq').addClass('open');
	        }

	    });

	    $('.tabs-container .tab-content:not(:first)').hide();
	    $('.tabs li a').click(function () {
	        $('.tabs-container .tab-content').hide();
	        $('.tabs li').removeClass('active');
	        $(this).parent().addClass('active');
	        var id = $(this).attr('href');
	        $(id).show();
	        return false;
	    })
	    $('.title-keotha-chiphi').click(function (e) {
	        e.preventDefault();
	        $(this).parent(".keotha-chiphi-item").toggleClass("active");
	        $('.keotha-chiphi-item.active .chiphi-child').slideToggle();
	    })
	    //Slider single product



        //--------------slider home--------------//

        if(jQuery().owlCarousel != undefined) {

            $("#gallery_01").owlCarousel({// slider home
                items: 4,
                //transitionStyle : "owl-fade-in",
                slideSpeed: 300,
                paginationSpeed: 400,
                itemsDesktop: [1199, 3],
                itemsDesktopSmall: [979, 3],
                itemsTablet: [767, 2],
                itemsMobile: false,
                navigation: true,
                navigationText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"]
            });

            $("#thietkemau-slider").owlCarousel({

                navigation: true, // Show next and prev buttons
                slideSpeed: 300,
                paginationSpeed: 400,
                pagination: true,

                items: 1,
                itemsDesktop: false,
                itemsDesktopSmall: false,
                itemsTablet: false,
                itemsMobile: false,
                navigationText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"]
            });
        }
       


        $('.layout-built-item li').click(function () {
            $('.layout-built-item li').removeClass("active");
            $(this).addClass("active");
            return false;
        });

	    //------quality single product

        if ($('.quantity').length > 0) {
            var form_cart = $('form .quantity')
            var minus = form_cart.find($('.minus'));
            var plus = form_cart.find($('.plus'));

            minus.on('click', function () {
                var qty = $(this).parent().find('.qty');
                if (qty.val() <= 1) {
                    qty.val(1);
                } else {
                    qty.val((parseInt(qty.val(), 10) - 1));
                }
            });
            plus.on('click', function () {
                var qty = $(this).parent().find('.qty');
                qty.val((parseInt(qty.val(), 10) + 1));
            });
        }





        //--------------Toogle--------------//
		$('.category-product-detail').hide();
            $('.table-section-header').unbind('click');
			
            $('.table-section-header').click(function(){
				$(this).toggleClass("active-show");
				$(this).parent('.product-list-section').addClass("list-show");
				$('.list-show .category-product-detail').slideToggle("slow");
                //$('.menu-home >.menu-main').slideToggle("slow");
                return false;
            });
			$('.main-menu ul li a').click(function(){
				$('.main-menu ul li a').parent().removeClass('active');
				$(this).parent().addClass("active");
				 
			});
			
//Click  item menu
		var pgurl = window.location.href.substr(window.location.href.lastIndexOf("/")+1);	
         $(".list-category ul li a").each(function(){
			var icon=$(this).attr("href");
			var hre=icon.split("/");
			var acti=hre[hre.length - 1];
              if(acti == pgurl || acti == '' )
              $(this).addClass("active");
         });
		 $(".main-menu ul li a").each(function(){
			var icon=$(this).attr("href");
			var hre=icon.split("/");
			var acti=hre[hre.length - 1];
              if(acti == pgurl || acti == '' )
              $(this).addClass("active");
			$(this).parents('.sub-menu').addClass("active");
			
         });	
		//$(".main-menu ul li ul.sub-menu.active").each(function(){
		//	$(this).parent('.menu-item').addClass("active");
		//});		 
			
        //------------menu mobie------------//

        $('.mobimenu').click(function(){
            $('.main-menu >ul').slideToggle();
            return false;
        });

        

        //--------------back to top--------------//

        // browser window scroll (in pixels) after which the "back to top" link is shown
        var offset = 300,
        //browser window scroll (in pixels) after which the "back to top" link opacity is reduced
            offset_opacity = 1000,
        //duration of the top scrolling animation (in ms)
            scroll_top_duration = 700,
        //grab the "back to top" link
            $back_to_top = $('.back-to-top');

        //hide or show the "back to top" link
        $(window).scroll(function(){
            ( $(this).scrollTop() > offset ) ? $back_to_top.addClass('cd-is-visible') : $back_to_top.removeClass('cd-is-visible cd-fade-out');
            if( $(this).scrollTop() > offset_opacity ) {
                $back_to_top.addClass('cd-fade-out');
            }
        });
        //smooth scroll to top
        $back_to_top.on('click', function(event){
            event.preventDefault();
            $('body,html').animate({
                    scrollTop: 0
                }, scroll_top_duration
            );
        });

	});
})(jQuery);

var sidebarWidth = 0;
$(window).resize(function(){
    var _width = $(this).width();

    if (_width < 767) {

    }
    else {
        if (true) {
            var temp = $('#sidebar').find('.box-content');
            if (temp) {
                if (temp.css('display') == 'none') {
                    temp.slideToggle();
                }
            }
        }
    }

    sidebarWidth = $('#sidebar').width();
    $('.sidebar-box').width(sidebarWidth);
});

$(document).ready(function ($) {
    sidebarWidth = $('#sidebar').width();
    $('.sidebar-box').width(sidebarWidth);
    if (jQuery().affix != undefined) {
        $(".list-category").affix({
            offset: {
                top: $("#header").height() + 22,
                bottom: $("#footer").height() + 40
            }
        });
    }

    if (typeof $.prototype.masonry != 'undefined') {
        $(".masonry-wrapper").masonry({
            itemSelector: '.grid-item',
            percentPosition: true,
            columnWidth: '.grid-item',
            gutter: 5
        });
    }

    //-------------------------- Boxes -----------------------------//
    $('.box .box-tool > a').click(function (e) {
        if ($(this).data('action') == undefined) {
            return;
        }
        var action = $(this).data('action');
        var btn = $(this);
        switch (action) {
            case 'collapse':
                $(btn).children('i').addClass('anim-turn180');
                $(this).parents('.box').children('.box-content').slideToggle(500, function () {
                    if ($(this).is(":hidden")) {
                        //$(btn).children('i').attr('class', 'fa fa-chevron-down');
                        $(btn).children('i').css('background', 'url(/Upload/Images/assets/drop.svg) no-repeat center center;');
                    } else {
                        $(btn).children('i').css('background', 'url(/Upload/Images/assets/drop.svg) no-repeat center center;');
                    }
                    $(btn).children('i').removeClass('anim-turn180');
                });
                break;
            case 'close':
                $(this).parents('.box').fadeOut(500, function () {
                    $(this).parent().remove();
                })
                break;
            case 'config':
                $('#' + $(this).data('modal')).modal('show');
                break;
        }
        e.preventDefault();
    });
});

//Jquery Extend
$.extend({
    getJsonFromUrl: function (url, urlData) {
        var result = null;
        $.ajax({
            type: "GET",
            url: url,
            data: urlData,
            dataType: "json",
            async: false,
            success: function (jsonResult) {
                result = jsonResult;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Failed to retrieve data.');
            }
        })
        return result;
    }
});


   // $(function() {
      //   var pgurl = window.location.href.substr(window.location.href
   // .lastIndexOf("/")+1);
    //     $(".list-category ul li a").each(function(){
         //     if($(this).attr("href") == pgurl || $(this).attr("href") == '' )
         //     $(this).addClass("active");
        // })
// });
