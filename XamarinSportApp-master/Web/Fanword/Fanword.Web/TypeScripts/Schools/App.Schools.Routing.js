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
                        return "/Schools/_IndexTemplate";
                    },
                    controller: "SchoolsController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Schools/_BreadcrumbTemplate";
                    },
                    controller: "SchoolsBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Schools/_IndexTemplate";
                    },
                    controller: "SchoolsController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Schools/_BreadcrumbTemplate";
                    },
                    controller: "SchoolsBreadcrumbController",
                }
            }
        }).state('addNewSchool', {
            url: "/New",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Schools/_EditTemplate";
                    },
                    controller: "SchoolController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Schools/_BreadcrumbTemplate";
                    },
                    controller: "SchoolsBreadcrumbController",
                }
            }
        }).state('viewSchool', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Schools/_EditTemplate";
                    },
                    controller: "SchoolController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
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
//# sourceMappingURL=App.Schools.Routing.js.map