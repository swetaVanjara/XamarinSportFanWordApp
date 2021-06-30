(() => {
    'use strict';

    angular.module('FanwordApp').controller('LikesBreadcrumbController', likesBreadcrumbController);

    likesBreadcrumbController.$inject = ['$scope', '$state', '$http'];
    
    function likesBreadcrumbController($scope: any, $state: any, $http: ng.IHttpService) {
        $scope.crumbs = [];
        if ($state.current.name == "root" || $state.current.name == "root2") {
            defaultCrumbs();
        }

        function defaultCrumbs() {
            $scope.crumbs.push(new Breadcrumb("Home", "/Home"));
            $scope.crumbs.push(new Breadcrumb("Comments", "/Comments"));
        }
    }
})();


