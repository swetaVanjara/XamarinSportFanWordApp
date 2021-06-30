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
                        return "/UserAdmins/_IndexTemplate";
                    },
                    controller: "UserAdminsController"
                }
            }
        }).state('root2', {
            url: "/",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/UserAdmins/_IndexTemplate";
                    },
                    controller: "UserAdminsController"
                }
            }
        }).state('addNewUserAdmin', {
            url: "/New",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/UserAdmins/_AddTemplate";
                    },
                    controller: "NewUserAdminController"
                }
            }
        }).state('viewSchoolAdmin', {
            url: "/ViewSchoolAdmin/{id:string}",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/UserAdmins/_ViewSchoolTemplate";
                    },
                    controller: "SchoolAdminViewController"
                }
            }
        }).state('viewTeamAdmin', {
            url: "/ViewTeamAdmin/{id:string}",
            views: {
                "mainview": {
                    templateUrl: function (myParams) {
                        return "/UserAdmins/_ViewTeamTemplate";
                    },
                    controller: "TeamAdminViewController"
                }
            }
        });
    };
    configFunction.$inject = ["$stateProvider", "$httpProvider", "$locationProvider", '$urlMatcherFactoryProvider'];
    portalApp.config(configFunction);
})();
//# sourceMappingURL=App.UserAdmins.Routing.js.map