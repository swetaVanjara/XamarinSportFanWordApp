(function () {
    'use strict';
    angular.module('FanwordApp').controller('TeamsBreadcrumbController', teamsBreadcrumbController);
    teamsBreadcrumbController.$inject = ['$scope', '$state', '$http'];
    function teamsBreadcrumbController($scope, $state, $http) {
        $scope.crumbs = [];
        if ($state.current.name == "root" || $state.current.name == "root2") {
            defaultCrumbs();
        }
        if ($state.current.name == "viewTeam") {
            defaultCrumbs();
            //load and set another breadcrumb
            $http.get('/api/Teams/' + $state.params.id).then(function (promise) {
                $scope.crumbs.push(new Breadcrumb(promise.data.nickname, "/Teams#!/View/" + promise.data.id));
            });
        }
        if ($state.current.name == "addNewTeam") {
            defaultCrumbs();
            $scope.crumbs.push(new Breadcrumb("New Team", ""));
        }
        function defaultCrumbs() {
            $scope.crumbs.push(new Breadcrumb("Home", "/Home"));
            $scope.crumbs.push(new Breadcrumb("Teams", "/Teams"));
        }
    }
})();
//# sourceMappingURL=App.TeamsBreacrumb.Controller.js.map