(function () {
    'use strict';
    var portalApp = angular.module('FanwordApp');
    var configFunction = function ($stateProvider, $httpProvider, $locationProvider, $urlMatcherFactoryProvider) {
        $locationProvider.html5Mode(false);
        $urlMatcherFactoryProvider.caseInsensitive(true);
        $stateProvider.state('root', {
            url: "",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/NewsNotifications/_IndexTemplate";
                    },
                    controller: "NewsNotificationController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/NewsNotifications/_BreadcrumbTemplate";
                    },
                    controller: "NewsNotificationBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/NewsNotifications/_IndexTemplate";
                    },
                    controller: "NewsNotificationController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/NewsNotifications/_BreadcrumbTemplate";
                    },
                    controller: "NewsNotificationBreadcrumbController",
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();
//# sourceMappingURL=App.NewsNotifications.Routing.js.map