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
                        return "/ContentManagement/_IndexTemplate";
                    },
                    controller: "ContentManagementController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/ContentManagement/_BreadcrumbTemplate";
                    },
                    controller: "ContentManagementBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/ContentManagement/_IndexTemplate";
                    },
                    controller: "ContentManagementController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/ContentManagement/_BreadcrumbTemplate";
                    },
                    controller: "ContentManagementBreadcrumbController",
                }
            }
        }).state('viewPost', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/ContentManagement/_EditTemplate";
                    },
                    controller: "PostController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
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
//# sourceMappingURL=App.ContentManagement.Routing.js.map