(() => {
    'use strict';
    var portalApp = angular.module('FanwordApp');

    var configFunction = ($stateProvider: any, $httpProvider: any, $locationProvider: any, $urlMatcherFactoryProvider: any) => {
        $locationProvider.html5Mode(false);
        $urlMatcherFactoryProvider.caseInsensitive(true);

        $stateProvider.state('root', {
            url: "",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/NewsNotifications/_IndexTemplate";
                    },
                    controller: "NewsNotificationController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/NewsNotifications/_BreadcrumbTemplate";
                    },
                    controller: "NewsNotificationBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/NewsNotifications/_IndexTemplate";
                    },
                    controller: "NewsNotificationController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
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