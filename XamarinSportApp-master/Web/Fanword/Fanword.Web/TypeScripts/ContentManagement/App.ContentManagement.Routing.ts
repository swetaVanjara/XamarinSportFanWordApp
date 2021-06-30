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
                        return "/ContentManagement/_IndexTemplate";
                    },
                    controller: "ContentManagementController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/ContentManagement/_BreadcrumbTemplate";
                    },
                    controller: "ContentManagementBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/ContentManagement/_IndexTemplate";
                    },
                    controller: "ContentManagementController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/ContentManagement/_BreadcrumbTemplate";
                    },
                    controller: "ContentManagementBreadcrumbController",
                }
            }
        }).state('viewPost', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/ContentManagement/_EditTemplate";
                    },
                    controller: "PostController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/ContentManagement/_BreadcrumbTemplate";
                    },
                    controller: "ContentManagementBreadcrumbController",
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();