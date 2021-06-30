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
                        return "/Advertising/_IndexTemplate";
                    },
                    controller: "AdvertisersController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Advertising/_BreadcrumbTemplate";
                    },
                    controller: "AdvertisingBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Advertising/_IndexTemplate";
                    },
                    controller: "AdvertisersController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Advertising/_BreadcrumbTemplate";
                    },
                    controller: "AdvertisingBreadcrumbController",
                }
            }
        }).state('viewAdvertiser', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Advertising/_AdvertiserTemplate";
                    },
                    controller: "AdvertiserController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Advertising/_BreadcrumbTemplate";
                    },
                    controller: "AdvertisingBreadcrumbController",
                }
            }
        }).state('viewCampaign', {
            url: "/View/{advertiserId:string}/Campaign/{id:string}",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Campaigns/_CampaignEditTemplate";
                    },
                    controller: "AdminCampaignController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Advertising/_BreadcrumbTemplate";
                    },
                    controller: "AdvertisingBreadcrumbController",
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();
//# sourceMappingURL=App.Advertising.Routing.js.map