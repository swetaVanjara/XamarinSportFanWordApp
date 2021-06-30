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
                        return "/Events/_IndexTemplate";
                    },
                    controller: "EventsController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Events/_BreadcrumbTemplate";
                    },
                    controller: "EventsBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Events/_IndexTemplate";
                    },
                    controller: "EventsController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Events/_BreadcrumbTemplate";
                    },
                    controller: "EventsBreadcrumbController",
                }
            }
        }).state('viewEvent', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Events/_EditTemplate";
                    },
                    controller: "EventController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Events/_BreadcrumbTemplate";
                    },
                    controller: "EventsBreadcrumbController",
                }
            }
        }).state('addNewEvent', {
            url: "/New",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Events/_EditTemplate";
                    },
                    controller: "EventController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Events/_BreadcrumbTemplate";
                    },
                    controller: "EventsBreadcrumbController",
                }
            }
            }).state('manageEvents', {
                url: "/Index",
                views: {
                    "manageview": {
                        templateUrl: (myParams: any) => {
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