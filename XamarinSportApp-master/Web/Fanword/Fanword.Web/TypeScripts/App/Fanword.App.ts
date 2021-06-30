(() => {
    'use strict';

    var app = angular.module('FanwordApp', [
        'ui.bootstrap',
        'kendo.directives',
        'ui.router',
        'angularPromiseButtons',
        'angularMoment',
        'monospaced.elastic'
    ]);

    app.value('$', $);

    app.directive('stopEvent', () => ({
        restrict: "A",
        link: (scope: any, element: any, attrs: any) => {
            element.bind('click', e => {
                e.stopPropagation();
            });
        }
    }));

})();