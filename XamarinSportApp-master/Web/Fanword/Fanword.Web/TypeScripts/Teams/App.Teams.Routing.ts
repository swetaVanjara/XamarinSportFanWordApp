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
                        return "/Teams/_IndexTemplate";
                    },
                    controller: "TeamsController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Teams/_BreadcrumbTemplate";
                    },
                    controller: "TeamsBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Teams/_IndexTemplate";
                    },
                    controller: "TeamsController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Teams/_BreadcrumbTemplate";
                    },
                    controller: "TeamsBreadcrumbController",
                }
            }
        }).state('addNewTeam', {
            url: "/New",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Teams/_EditTemplate";
                    },
                    controller: "TeamController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Teams/_BreadcrumbTemplate";
                    },
                    controller: "TeamsBreadcrumbController",
                }
            }
        }).state('viewTeam', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Teams/_EditTemplate";
                    },
                    controller: "TeamController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Teams/_BreadcrumbTemplate";
                    },
                    controller: "TeamsBreadcrumbController",
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();