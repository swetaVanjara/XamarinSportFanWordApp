﻿(() => {
    'use strict';

    angular.module('FanwordApp').controller('EventsBreadcrumbController', eventsBreadcrumbController);

    eventsBreadcrumbController.$inject = ['$scope','$http','$state'];

    function eventsBreadcrumbController($scope: any, $http: ng.IHttpService, $state:any) {
        $scope.crumbs = [];
        if ($state.current.name == "root" || $state.current.name == "root2") {
            defaultCrumbs();
        }

        if ($state.current.name == "addNewEvent") {
            defaultCrumbs();
            $scope.crumbs.push(new Breadcrumb("New Event", "/Events"));
        }

        if ($state.current.name == "viewEvent") {
            defaultCrumbs();
            //load and set another breadcrumb
            $http.get('/api/Events/' + $state.params.id).then((promise: any) => {
                $scope.crumbs.push(new Breadcrumb(promise.data.name, "/Events#!/View/" + promise.data.id));
            });
        }

        function defaultCrumbs() {
            $scope.crumbs.push(new Breadcrumb("Home", "/Home"));
            $scope.crumbs.push(new Breadcrumb("Events", "/Events"));
        }
    }
})();