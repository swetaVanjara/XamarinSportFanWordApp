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
                        return "/Likes/_IndexTemplate";
                    },
                    controller: "LikesController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Likes/_BreadcrumbTemplate";
                    },
                    controller: "LikesBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Likes/_IndexTemplate";
                    },
                    controller: "LikesController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Likes/_BreadcrumbTemplate";
                    },
                    controller: "LikesBreadcrumbController",
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();
//# sourceMappingURL=App.Likes.Routing.js.map