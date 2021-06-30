(function () {
    'use strict';
    angular.module('FanwordApp').controller('ContentManagementBreadcrumbController', contentManagementBreadcrumbController);
    contentManagementBreadcrumbController.$inject = ['$scope', '$state', '$http'];
    function contentManagementBreadcrumbController($scope, $state, $http) {
        $scope.crumbs = [];
        if ($state.current.name == "root" || $state.current.name == "root2") {
            defaultCrumbs();
        }
        if ($state.current.name == "viewPost") {
            defaultCrumbs();
            //load and set another breadcrumb
            //$http.get('/api/Sports/' + $state.params.id).then((promise: any) => {
            //    $scope.crumbs.push(new Breadcrumb(promise.data.name, "/Sports#!/View/" + promise.data.id));
            //});
        }
        //if ($state.current.name == "addNewSport") {
        //    defaultCrumbs();
        //    $scope.crumbs.push(new Breadcrumb("New Sport", ""));
        //}
        function defaultCrumbs() {
            $scope.crumbs.push(new Breadcrumb("Home", "/Home"));
            $scope.crumbs.push(new Breadcrumb("Content Management", "/ContentManagement"));
        }
    }
})();
//# sourceMappingURL=App.ContentManagementBreadcrumb.Controller.js.map