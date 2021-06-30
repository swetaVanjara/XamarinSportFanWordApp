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
                        return "/Sports/_IndexTemplate";
                    },
                    controller: "SportsController"
                },
                "breadcrumbview": {
                    templateUrl:(myParams: any) => {
                        return "/Sports/_BreadcrumbTemplate";
                    },
                    controller:"SportsBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Sports/_IndexTemplate";
                    },
                    controller: "SportsController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Sports/_BreadcrumbTemplate";
                    },
                    controller: "SportsBreadcrumbController",
                }
            }
        }).state('addNewSport', {
            url: "/New",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Sports/_EditTemplate";
                    },
                    controller: "SportController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Sports/_BreadcrumbTemplate";
                    },
                    controller: "SportsBreadcrumbController",
                }
            }
        }).state('viewSport', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Sports/_EditTemplate";
                    },
                    controller: "SportController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Sports/_BreadcrumbTemplate";
                    },
                    controller: "SportsBreadcrumbController",
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();