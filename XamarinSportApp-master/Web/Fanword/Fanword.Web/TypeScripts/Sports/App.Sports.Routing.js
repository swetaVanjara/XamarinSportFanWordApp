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
                        return "/Sports/_IndexTemplate";
                    },
                    controller: "SportsController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Sports/_BreadcrumbTemplate";
                    },
                    controller: "SportsBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Sports/_IndexTemplate";
                    },
                    controller: "SportsController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Sports/_BreadcrumbTemplate";
                    },
                    controller: "SportsBreadcrumbController",
                }
            }
        }).state('addNewSport', {
            url: "/New",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Sports/_EditTemplate";
                    },
                    controller: "SportController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Sports/_BreadcrumbTemplate";
                    },
                    controller: "SportsBreadcrumbController",
                }
            }
        }).state('viewSport', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Sports/_EditTemplate";
                    },
                    controller: "SportController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
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
//# sourceMappingURL=App.Sports.Routing.js.map