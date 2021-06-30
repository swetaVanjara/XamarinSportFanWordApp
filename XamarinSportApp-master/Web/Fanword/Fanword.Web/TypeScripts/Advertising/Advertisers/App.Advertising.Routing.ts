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
                        return "/Campaigns/_CampaignTemplate";
                    },
                    controller: "CampaignsController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Campaigns/_BreadcrumbTemplate";
                    },
                    controller: "AdvertisersBreadcrumController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Campaigns/_CampaignTemplate";
                    },
                    controller: "CampaignsController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Campaigns/_BreadcrumbTemplate";
                    },
                    controller: "AdvertisersBreadcrumController",
                }
            }
        }).state('viewCampaign', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Campaigns/_CampaignEditTemplate";
                    },
                    controller: "CampaignController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Campaigns/_BreadcrumbTemplate";
                    },
                    controller: "AdvertisersBreadcrumController",
                }
            }
        }).state('addNewCampaign', {
            url: "/New",
            views: {
                "mainview": {
                    templateUrl: (myParams: any) => {
                        return "/Campaigns/_CampaignEditTemplate";
                    },
                    controller: "CampaignController"
                },
                "breadcrumbview": {
                    templateUrl: (myParams: any) => {
                        return "/Campaigns/_BreadcrumbTemplate";
                    },
                    controller: "AdvertisersBreadcrumController",
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();