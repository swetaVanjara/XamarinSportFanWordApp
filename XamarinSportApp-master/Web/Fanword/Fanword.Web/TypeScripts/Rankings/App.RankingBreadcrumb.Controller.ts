(() => {
    'use strict';

    angular.module('FanwordApp').controller('RankingBreadcrumbController', rankingBreadcrumbController);

    rankingBreadcrumbController.$inject=['$scope','$http', '$state','$rootScope'];

    function rankingBreadcrumbController($scope: any, $http: ng.IHttpService,$state:any,$rootScope:any) {
        $scope.crumbs = [];
        if ($state.current.name == "root" || $state.current.name == "root2") {
            defaultCrumbs();
            $rootScope.$emit('setMenuItem', null);
        }
        if ($state.current.name == "viewRanking") {
            defaultCrumbs();
            //load and set another breadcrumb
            $http.get('/api/Rankings/BySport/' + $state.params.id).then((promise: any) => {
                $scope.crumbs.push(new Breadcrumb(promise.data.sportName, "/Rankings#!/View/" + promise.data.id));
            });
        }

        function defaultCrumbs() {
            $scope.crumbs.push(new Breadcrumb("Home", "/Home"));
            $scope.crumbs.push(new Breadcrumb("Rankings", "/Rankings"));
        }
    }
})();