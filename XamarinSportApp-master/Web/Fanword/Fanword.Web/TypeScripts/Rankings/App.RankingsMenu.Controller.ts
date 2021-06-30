(() => {
    'use strict';

    angular.module("FanwordApp").controller("RankingsMenuController", rankingsMenuController);

    rankingsMenuController.$inject = ['$scope', '$http', '$state', '$rootScope'];

    function rankingsMenuController($scope: any, $http: ng.IHttpService, $state: any, $rootScope: any) {

        $scope.menuItems = [];

        $scope.activeMenuItem = {};

        $scope.getActiveClass = (item: MenuItem) => {
            if ($scope.activeMenuItem == undefined) return "";

            if (item.id == $scope.activeMenuItem.id) {
                return 'active';
            }
            return '';
        };

        $rootScope.$on('refreshMenu', () => {
            loadControllerData();
        });

        $rootScope.$on('setMenuItem', (event: any, data: any) => {
            if (data == undefined) {
                $scope.activeMenuItem = {};
            } else {
                $scope.activeMenuItem = Enumerable.From($scope.menuItems).FirstOrDefault(null, (x: MenuItem) => { return x.id == data.id });    
            }
            
        });

        $scope.menuFilter = (item: MenuItem) => {
            if (item == undefined) return true;
            if ($scope.searchText == undefined || $scope.searchText == '') return true;
            return item.name.toLowerCase().indexOf($scope.searchText.toLowerCase()) >= 0;
        }

        $scope.viewRanking = (item: MenuItem) => {
            if ($scope.activeMenuItem.id == item.id) {
                $state.go('root');
                $scope.activeMenuItem = {};
            } else {
                $state.go('viewRanking', { id: item.id });
            }
        }

        function loadControllerData() {
            $http.get('/api/Sports/SelectControlList').then((promise: any) => {
                $scope.menuItems = Enumerable.From(promise.data).Select((x: any) => new MenuItem(x.id, x.displayName)).ToArray() as MenuItem[];
            });
        }

        loadControllerData();

    }
})();