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
                        return "/Likes/_IndexTemplate";
                    },
                    controller: "LikesController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Likes/_BreadcrumbTemplate";
                    },
                    controller: "LikesBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Likes/_IndexTemplate";
                    },
                    controller: "LikesController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Likes/_BreadcrumbTemplate";
                    },
                    controller: "LikesBreadcrumbController",
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();