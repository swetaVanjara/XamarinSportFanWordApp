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
                        return "/Rankings/_IndexTemplate";
                    },
                    controller: "RankingController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Rankings/_BreadcrumbTemplate";
                    },
                    controller: "RankingBreadcrumbController",
                },
                "menuview": {
                    templateUrl: function (myParams) {
                        return "/Rankings/_MenuTemplate";
                    },
                    controller: "RankingsMenuController"
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Rankings/_IndexTemplate";
                    },
                    controller: "RankingController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Rankings/_BreadcrumbTemplate";
                    },
                    controller: "RankingBreadcrumbController",
                },
                "menuview": {
                    templateUrl: function (myParams) {
                        return "/Rankings/_MenuTemplate";
                    },
                    controller: "RankingsMenuController"
                }
            }
        }).state('viewRanking', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Rankings/_EditTemplate";
                    },
                    controller: "RankingController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Rankings/_BreadcrumbTemplate";
                    },
                    controller: "RankingBreadcrumbController",
                },
                "menuview": {
                    templateUrl: function (myParams) {
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
//# sourceMappingURL=App.Rankings.Routing.js.map