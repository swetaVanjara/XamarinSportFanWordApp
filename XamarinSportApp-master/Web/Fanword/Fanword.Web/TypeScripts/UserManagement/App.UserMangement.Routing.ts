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
                        return "/UserManagement/_IndexTemplate";
                    },
                    controller: "UserManagementController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/UserManagement/_BreadcrumbTemplate";
                    },
                    controller: "UserMangementBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/UserManagement/_IndexTemplate";
                    },
                    controller: "UserManagementController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/UserManagement/_BreadcrumbTemplate";
                    },
                    controller: "UserMangementBreadcrumbController",
                }
            }
        }).state('addNewUser', {
            url: "/New",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/UserManagement/_EditTemplate";
                    },
                    controller: "UserController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/UserManagement/_BreadcrumbTemplate";
                    },
                    controller: "UserMangementBreadcrumbController",
                }
            }
        }).state('viewUser', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/UserManagement/_EditTemplate";
                    },
                    controller: "UserController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/UserManagement/_BreadcrumbTemplate";
                    },
                    controller: "UserMangementBreadcrumbController",
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();