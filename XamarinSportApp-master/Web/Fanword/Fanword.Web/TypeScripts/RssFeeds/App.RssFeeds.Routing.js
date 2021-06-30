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
                        return "/RssFeeds/_IndexTemplate";
                    },
                    controller: "RssFeedsController"
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/RssFeeds/_IndexTemplate";
                    },
                    controller: "RssFeedsController"
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();
//# sourceMappingURL=App.RssFeeds.Routing.js.map