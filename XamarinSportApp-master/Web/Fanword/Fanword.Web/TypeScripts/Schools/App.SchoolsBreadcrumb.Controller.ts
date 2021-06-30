(() => {
    'use strict';

    angular.module('FanwordApp').controller('SchoolsBreadcrumbController', schoolsBreadcrumbController);

    schoolsBreadcrumbController.$inject = ['$scope', '$state', '$http'];

    function schoolsBreadcrumbController($scope: any, $state: any, $http: ng.IHttpService) {
        $scope.crumbs = [];
        if ($state.current.name == "root" || $state.current.name == "root2") {
            defaultCrumbs();
        }
        if ($state.current.name == "viewSchool") {
            defaultCrumbs();
            //load and set another breadcrumb
            $http.get('/api/Schools/'+ $state.params.id).then((promise: any) => {
                $scope.crumbs.push(new Breadcrumb(promise.data.name, "/Schools#!/View/" + promise.data.id));
            });
        }

        if ($state.current.name == "addNewSchool") {
            defaultCrumbs();
            $scope.crumbs.push(new Breadcrumb("New School", ""));
        }

        function defaultCrumbs() {
            $scope.crumbs.push(new Breadcrumb("Home", "/Home"));
            $scope.crumbs.push(new Breadcrumb("Schools", "/Schools"));
        }
    }
})();


