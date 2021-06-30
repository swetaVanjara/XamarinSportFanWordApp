(function () {
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
    app.directive('stopEvent', function () { return ({
        restrict: "A",
        link: function (scope, element, attrs) {
            element.bind('click', function (e) {
                e.stopPropagation();
            });
        }
    }); });
})();
//# sourceMappingURL=Fanword.App.js.map