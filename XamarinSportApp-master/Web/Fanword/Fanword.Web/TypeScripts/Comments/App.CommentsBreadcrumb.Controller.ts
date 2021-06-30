(() => {
    'use strict';

    angular.module('FanwordApp').controller('CommentsBreadcrumbController', commentsBreadcrumbController);

    commentsBreadcrumbController.$inject = ['$scope', '$state', '$http'];

    function commentsBreadcrumbController($scope: any, $state: any, $http: ng.IHttpService) {
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


