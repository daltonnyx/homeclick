/*
	Flaunt.js v1.0.0
	by Todd Motto: http://www.toddmotto.com
	Latest version: https://github.com/toddmotto/flaunt-js
	
	Copyright 2013 Todd Motto
	Licensed under the MIT license
	http://www.opensource.org/licenses/mit-license.php

	Flaunt JS, stylish responsive navigations with nested click to reveal.
*/
; (function ($) {

    // DOM ready
    $(function () {

        // Append the mobile icon nav
        //$('.nav').append($('<div class="nav-mobile"></div>'));

        // Add a <span> to every .nav-item that has a <ul> inside
        $('.nav-item').has('ul').prepend('<span class="nav-click"><i class="nav-arrow"></i></span>');

        // Click to reveal the nav
        $('.nav-mobile').click(function () {
            $(this).toggleClass('nav-toggle');
            $('.nav-list').slideToggle();
        });

        // Dynamic binding to on 'click'
        $('.nav-list').on('click', '.nav-click', function () {
            var temp = $('.sub-toggle');
            var handler = this;
            $.each(temp, function (i, object) {
                if (object != handler) {
                    $(object).siblings('.nav-submenu').slideToggle();
                    $(object).children('.nav-arrow').toggleClass('nav-rotate');
                    $(object).toggleClass('sub-toggle');
                }
            });

            // Toggle the nested nav
            $(this).toggleClass('sub-toggle');
            $(this).siblings('.nav-submenu').slideToggle();

            // Toggle the arrow using CSS3 transforms
            $(this).children('.nav-arrow').toggleClass('nav-rotate');

        });

    });

    $(window).resize(function () {
        var _width = $(this).width();
        if (_width < 767) {
            var temp = $('.sub-toggle');
            $.each(temp, function (i, object) {
                if (object != handler) {
                    $(object).siblings('.nav-submenu').slideToggle();
                    $(object).children('.nav-arrow').toggleClass('nav-rotate');
                    $(object).toggleClass('sub-toggle');
                }
            });

            temp = $('.nav-toggle');
            temp.toggleClass('nav-toggle');

            $('.nav-list').css('display', 'none');
        }
        else {
            $('.nav-list').css('display', 'flex');
        }
    });

})(jQuery);