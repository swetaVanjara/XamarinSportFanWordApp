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
                        return "/Comments/_IndexTemplate";
                    },
                    controller: "CommentsController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Comments/_BreadcrumbTemplate";
                    },
                    controller: "CommentsBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Comments/_IndexTemplate";
                    },
                    controller: "CommentsController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Comments/_BreadcrumbTemplate";
                    },
                    controller: "CommentsBreadcrumbController",
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();