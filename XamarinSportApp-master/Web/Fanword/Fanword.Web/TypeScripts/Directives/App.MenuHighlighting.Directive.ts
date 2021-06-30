(() => {
    'use strict';

    angular.module('FanwordApp').directive('highlightMenu', () => ({
        restrict: "A",
        link: (scope: any, element: any, attrs: any) => {

            var documentUrl: string = window.location.pathname;
            $('.page-sidebar-menu').find(".nav-item").each((index: number, el: any) => {
                var $this = $('.page-sidebar-menu').find('.nav-item')[index];
                var thisUrl = $($this).find('.nav-link').attr('href');
                if (thisUrl === "/" && documentUrl === "/") {
                    $($this).addClass('active');
                } else if (thisUrl.indexOf(documentUrl) === 0 && documentUrl !== "/") {
                    $($this).addClass('active');
                }
            });

            $('.page-sidebar-menu').find('li:active').parents('li').addClass('active open');
        }
    }));
})();