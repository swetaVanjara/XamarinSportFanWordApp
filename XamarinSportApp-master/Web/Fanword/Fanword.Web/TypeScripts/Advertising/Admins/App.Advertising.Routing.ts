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
                        return "/Advertising/_IndexTemplate";
                    },
                    controller: "AdvertisersController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Advertising/_BreadcrumbTemplate";
                    },
                    controller: "AdvertisingBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Advertising/_IndexTemplate";
                    },
                    controller: "AdvertisersController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Advertising/_BreadcrumbTemplate";
                    },
                    controller: "AdvertisingBreadcrumbController",
                }
            }
        }).state('viewAdvertiser', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Advertising/_AdvertiserTemplate";
                    },
                    controller: "AdvertiserController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Advertising/_BreadcrumbTemplate";
                    },
                    controller: "AdvertisingBreadcrumbController",
                }
            }
        }).state('viewCampaign', {
            url: "/View/{advertiserId:string}/Campaign/{id:string}",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Campaigns/_CampaignEditTemplate";
                    },
                    controller: "AdminCampaignController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
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