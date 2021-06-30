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
                        return "/ContentSourceAdmin/_IndexTemplate";
                    },
                    controller: "ContentSourceAdminController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/ContentSourceAdmin/_BreadcrumbTemplate";
                    },
                    controller: "ContentSourceAdminBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/ContentSourceAdmin/_IndexTemplate";
                    },
                    controller: "ContentSourceAdminController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/ContentSourceAdmin/_BreadcrumbTemplate";
                    },
                    controller: "ContentSourceAdminBreadcrumbController",
                }
            }
        }).state('viewContentSource', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/ContentSourceAdmin/_ContentSourceTemplate";
                    },
                    controller: "ContentSourceAdminViewController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/ContentSourceAdmin/_BreadcrumbTemplate";
                    },
                    controller: "ContentSourceAdminBreadcrumbController",
                }
            }
        });
    }
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();