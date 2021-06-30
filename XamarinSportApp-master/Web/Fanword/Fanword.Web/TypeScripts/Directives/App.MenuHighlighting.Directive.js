(function () {
    'use strict';
    angular.module('FanwordApp').directive('highlightMenu', function () { return ({
        restrict: "A",
        link: function (scope, element, attrs) {
            var documentUrl = window.location.pathname;
            $('.page-sidebar-menu').find(".nav-item").each(function (index, el) {
                var $this = $('.page-sidebar-menu').find('.nav-item')[index];
                var thisUrl = $($this).find('.nav-link').attr('href');
                if (thisUrl === "/" && documentUrl === "/") {
                    $($this).addClass('active');
                }
                else if (thisUrl.indexOf(documentUrl) === 0 && documentUrl !== "/") {
                    $($this).addClass('active');
                }
            });
            $('.page-sidebar-menu').find('li:active').parents('li').addClass('active open');
        }
    }); });
})();
//# sourceMappingURL=App.MenuHighlighting.Directive.js.map