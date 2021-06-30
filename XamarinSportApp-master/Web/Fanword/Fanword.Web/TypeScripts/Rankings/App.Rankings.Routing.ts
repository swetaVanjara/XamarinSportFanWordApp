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
                        return "/Rankings/_IndexTemplate";
                    },
                    controller: "RankingController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Rankings/_BreadcrumbTemplate";
                    },
                    controller: "RankingBreadcrumbController",
                },
                "menuview": {
                    templateUrl: (myParams: any) => {
                        return "/Rankings/_MenuTemplate";
                    },
                    controller: "RankingsMenuController"
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Rankings/_IndexTemplate";
                    },
                    controller: "RankingController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Rankings/_BreadcrumbTemplate";
                    },
                    controller: "RankingBreadcrumbController",
                },
                "menuview": {
                    templateUrl: (myParams: any) => {
                        return "/Rankings/_MenuTemplate";
                    },
                    controller: "RankingsMenuController"
                }
            }
        }).state('viewRanking', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Rankings/_EditTemplate";
                    },
                    controller: "RankingController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Rankings/_BreadcrumbTemplate";
                    },
                    controller: "RankingBreadcrumbController",
                },
                "menuview": {
                    templateUrl: (myParams: any) => {
                        return "/Rankings/_MenuTemplate";
                    },
                    controller: "RankingsMenuController"
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();