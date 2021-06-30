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
                        return "/UserManagement/_IndexTemplate";
                    },
                    controller: "UserManagementController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/UserManagement/_BreadcrumbTemplate";
                    },
                    controller: "UserMangementBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/UserManagement/_IndexTemplate";
                    },
                    controller: "UserManagementController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/UserManagement/_BreadcrumbTemplate";
                    },
                    controller: "UserMangementBreadcrumbController",
                }
            }
        }).state('addNewUser', {
            url: "/New",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/UserManagement/_EditTemplate";
                    },
                    controller: "UserController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/UserManagement/_BreadcrumbTemplate";
                    },
                    controller: "UserMangementBreadcrumbController",
                }
            }
        }).state('viewUser', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/UserManagement/_EditTemplate";
                    },
                    controller: "UserController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
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
//# sourceMappingURL=App.UserMangement.Routing.js.map