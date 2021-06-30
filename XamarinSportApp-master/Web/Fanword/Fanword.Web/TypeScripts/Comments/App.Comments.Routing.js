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
                        return "/Comments/_IndexTemplate";
                    },
                    controller: "CommentsController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Comments/_BreadcrumbTemplate";
                    },
                    controller: "CommentsBreadcrumbController",
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/Comments/_IndexTemplate";
                    },
                    controller: "CommentsController"
                },
                "breadcrumbview": {
                    templateUrl: function (myParams) {
                        return "/Comments/_BreadcrumbTemplate";
                    },
                    controller: "CommentsBreadcrumbController",
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();
//# sourceMappingURL=App.Comments.Routing.js.map