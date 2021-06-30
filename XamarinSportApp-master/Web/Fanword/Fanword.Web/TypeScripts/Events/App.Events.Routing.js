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
                        return "/Events/_IndexTemplate";
                    },
                    controller: "EventsController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Events/_BreadcrumbTemplate";
                    },
                    controller: "EventsBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Events/_IndexTemplate";
                    },
                    controller: "EventsController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Events/_BreadcrumbTemplate";
                    },
                    controller: "EventsBreadcrumbController",
                }
            }
        }).state('viewEvent', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Events/_EditTemplate";
                    },
                    controller: "EventController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Events/_BreadcrumbTemplate";
                    },
                    controller: "EventsBreadcrumbController",
                }
            }
        }).state('addNewEvent', {
            url: "/New",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Events/_EditTemplate";
                    },
                    controller: "EventController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Events/_BreadcrumbTemplate";
                    },
                    controller: "EventsBreadcrumbController",
                }
            }
        }).state('manageEvents', {
            url: "/Index",
            views: {
                "manageview": {
                    templateUrl: function (myParams) {
                        return "/Events/_ManageTemplate";
                    },
                    controller: "ManageEventsController"
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();
//# sourceMappingURL=App.Events.Routing.js.map