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
                        return "/Campaigns/_CampaignTemplate";
                    },
                    controller: "CampaignsController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Campaigns/_BreadcrumbTemplate";
                    },
                    controller: "AdvertisersBreadcrumController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Campaigns/_CampaignTemplate";
                    },
                    controller: "CampaignsController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Campaigns/_BreadcrumbTemplate";
                    },
                    controller: "AdvertisersBreadcrumController",
                }
            }
        }).state('viewCampaign', {
            url: "/View/{id:string}",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Campaigns/_CampaignEditTemplate";
                    },
                    controller: "CampaignController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Campaigns/_BreadcrumbTemplate";
                    },
                    controller: "AdvertisersBreadcrumController",
                }
            }
        }).state('addNewCampaign', {
            url: "/New",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Campaigns/_CampaignEditTemplate";
                    },
                    controller: "CampaignController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
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
//# sourceMappingURL=App.Advertising.Routing.js.map