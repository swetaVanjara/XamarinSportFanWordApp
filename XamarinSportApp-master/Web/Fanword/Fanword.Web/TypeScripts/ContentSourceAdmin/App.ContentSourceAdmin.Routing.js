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
                        return "/ContentSourceAdmin/_IndexTemplate";
                    },
                    controller: "ContentSourceAdminController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/ContentSourceAdmin/_BreadcrumbTemplate";
                    },
                    controller: "ContentSourceAdminBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/ContentSourceAdmin/_IndexTemplate";
                    },
                    controller: "ContentSourceAdminController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/ContentSourceAdmin/_BreadcrumbTemplate";
                    },
                    controller: "ContentSourceAdminBreadcrumbController",
                }
            }
        }).state('viewContentSource', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/ContentSourceAdmin/_ContentSourceTemplate";
                    },
                    controller: "ContentSourceAdminViewController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/ContentSourceAdmin/_BreadcrumbTemplate";
                    },
                    controller: "ContentSourceAdminBreadcrumbController",
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();
//# sourceMappingURL=App.ContentSourceAdmin.Routing.js.map