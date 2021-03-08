/*
Name: 			Barber
Written by: 	Okler Themes - (http://www.okler.net)
Theme Version:	7.5.0
*/

(function( $ ) {

	'use strict';

    // Instagram Feed
    var clientId    = '3b45f8860dfc4bb2b8a15c3057746304',
        accessToken = '10868028576.3b45f88.ba467930afe0456aade54ae16322a405';

    if( $('#instafeedCarouselDemoBarber').get(0) ) {
        // Instagram Feed Carousel
        var feed = new Instafeed({
            target: 'instafeedCarouselDemoBarber',
            get: 'user',
            userId: 'self',
            clientId: clientId,
            accessToken: accessToken,
            resolution: 'standard_resolution',
            limit: 12,
            template: 
                '<div>' +
                    '<a target="_blank" href="{{link}}">' +
                        '<img src="{{image}}" class="img-fluid" alt="{{caption}}" />' +
                    '</a>' +
                '</div>',
            after: function(){
                var $wrapper = $('#instafeedCarouselDemoBarber');

                $wrapper.addClass('owl-carousel mb-0').owlCarousel({
                    responsive: {
                        0: {
                            items: 1
                        },
                        575: {
                            items: 2
                        },
                        767: {
                            items: 3
                        },
                        991: {
                            items: 5
                        },
                        1440: {
                            items: 7
                        }
                    },
                    nav: false,
                    dots: false,
                    loop: true,
                    navText: [],
                    autoplay: true,
                    autoplayTimeout: 6000,
                    rtl: ( $('html').attr('dir') == 'rtl' ) ? true : false
                });
            }
        });

        // Init Instafeed Carousel
        feed.run();
    }

}).apply( this, [ jQuery ]);