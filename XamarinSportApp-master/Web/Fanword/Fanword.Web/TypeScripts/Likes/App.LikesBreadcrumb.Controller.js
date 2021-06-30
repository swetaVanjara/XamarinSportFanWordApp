(function () {
    'use strict';
    angular.module('FanwordApp').controller('LikesBreadcrumbController', likesBreadcrumbController);
    likesBreadcrumbController.$inject = ['$scope', '$state', '$http'];
    function likesBreadcrumbController($scope, $state, $http) {
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
//# sourceMappingURL=App.LikesBreadcrumb.Controller.js.map