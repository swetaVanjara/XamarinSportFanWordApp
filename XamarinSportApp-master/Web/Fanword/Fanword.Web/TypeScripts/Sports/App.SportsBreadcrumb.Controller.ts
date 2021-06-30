(() => {
    'use strict';

    angular.module('FanwordApp').controller('SportsBreadcrumbController', sportsBreadcrumbController);

    sportsBreadcrumbController.$inject = ['$scope', '$state', '$http'];

    function sportsBreadcrumbController($scope: any, $state: any, $http: ng.IHttpService) {
        $scope.crumbs = [];
        if ($state.current.name == "root" || $state.current.name == "root2") {
            defaultCrumbs();
        }
        if ($state.current.name == "viewSport") {
            defaultCrumbs();
            //load and set another breadcrumb
            $http.get('/api/Sports/' + $state.params.id).then((promise: any) => {
                $scope.crumbs.push(new Breadcrumb(promise.data.name, "/Sports#!/View/" + promise.data.id));
            });
        }

        if ($state.current.name == "addNewSport") {
            defaultCrumbs();
            $scope.crumbs.push(new Breadcrumb("New Sport", ""));
        }

        function defaultCrumbs() {
            $scope.crumbs.push(new Breadcrumb("Home", "/Home"));
            $scope.crumbs.push(new Breadcrumb("Sports", "/Sports"));
        }
    }
})();