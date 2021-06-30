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
                        return "/Teams/_IndexTemplate";
                    },
                    controller: "TeamsController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Teams/_BreadcrumbTemplate";
                    },
                    controller: "TeamsBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Teams/_IndexTemplate";
                    },
                    controller: "TeamsController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Teams/_BreadcrumbTemplate";
                    },
                    controller: "TeamsBreadcrumbController",
                }
            }
        }).state('addNewTeam', {
            url: "/New",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Teams/_EditTemplate";
                    },
                    controller: "TeamController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Teams/_BreadcrumbTemplate";
                    },
                    controller: "TeamsBreadcrumbController",
                }
            }
        }).state('viewTeam', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Teams/_EditTemplate";
                    },
                    controller: "TeamController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
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
//# sourceMappingURL=App.Teams.Routing.js.map