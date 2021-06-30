(() => {
    'use strict';

    angular.module('FanwordApp').controller('UserMangementBreadcrumbController', userMangementBreadcrumbController);

    userMangementBreadcrumbController.$inject = ['$scope', '$state', '$http'];

    function userMangementBreadcrumbController($scope: any, $state: any, $http: ng.IHttpService) {
        $scope.crumbs = [];
        if ($state.current.name == "root" || $state.current.name == "root2") {
            defaultCrumbs();
        }
        if ($state.current.name == "viewUser") {
            defaultCrumbs();
            //load and set another breadcrumb
            $http.get('/api/Users/' + $state.params.id).then((promise: any) => {
                $scope.crumbs.push(new Breadcrumb(promise.data.firstName + " " +promise.data.lastName, "/UserManagement/#!/View/" + promise.data.id));
            });
        }

        if ($state.current.name == "addNewUser") {
            defaultCrumbs();
            $scope.crumbs.push(new Breadcrumb("New User", ""));
        }

        function defaultCrumbs() {
            $scope.crumbs.push(new Breadcrumb("Home", "/Home"));
            $scope.crumbs.push(new Breadcrumb("Users", "/UserManagement"));
        }
    }
})();