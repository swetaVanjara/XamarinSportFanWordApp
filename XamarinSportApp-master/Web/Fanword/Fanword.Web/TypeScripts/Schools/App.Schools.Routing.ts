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
                        return "/Schools/_IndexTemplate";
                    },
                    controller: "SchoolsController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Schools/_BreadcrumbTemplate";
                    },
                    controller: "SchoolsBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Schools/_IndexTemplate";
                    },
                    controller: "SchoolsController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Schools/_BreadcrumbTemplate";
                    },
                    controller: "SchoolsBreadcrumbController",
                }
            }
        }).state('addNewSchool', {
            url: "/New",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Schools/_EditTemplate";
                    },
                    controller: "SchoolController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Schools/_BreadcrumbTemplate";
                    },
                    controller: "SchoolsBreadcrumbController",
                }
            }
        }).state('viewSchool', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Schools/_EditTemplate";
                    },
                    controller: "SchoolController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Schools/_BreadcrumbTemplate";
                    },
                    controller: "SchoolsBreadcrumbController",
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();