(function () {
    'use strict';
    angular.module("FanwordApp").controller("RankingsMenuController", rankingsMenuController);
    rankingsMenuController.$inject = ['$scope', '$http', '$state', '$rootScope'];
    function rankingsMenuController($scope, $http, $state, $rootScope) {
        $scope.menuItems = [];
        $scope.activeMenuItem = {};
        $scope.getActiveClass = function (item) {
            if ($scope.activeMenuItem == undefined)
                return "";
            if (item.id == $scope.activeMenuItem.id) {
                return 'active';
            }
            return '';
        };
        $rootScope.$on('refreshMenu', function () {
            loadControllerData();
        });
        $rootScope.$on('setMenuItem', function (event, data) {
            if (data == undefined) {
                $scope.activeMenuItem = {};
            }
            else {
                $scope.activeMenuItem = Enumerable.From($scope.menuItems).FirstOrDefault(null, function (x) { return x.id == data.id; });
            }
        });
        $scope.menuFilter = function (item) {
            if (item == undefined)
                return true;
            if ($scope.searchText == undefined || $scope.searchText == '')
                return true;
            return item.name.toLowerCase().indexOf($scope.searchText.toLowerCase()) >= 0;
        };
        $scope.viewRanking = function (item) {
            if ($scope.activeMenuItem.id == item.id) {
                $state.go('root');
                $scope.activeMenuItem = {};
            }
            else {
                $state.go('viewRanking', { id: item.id });
            }
        };
        function loadControllerData() {
            $http.get('/api/Sports/SelectControlList').then(function (promise) {
                $scope.menuItems = Enumerable.From(promise.data).Select(function (x) { return new MenuItem(x.id, x.displayName); }).ToArray();
            });
        }
        loadControllerData();
    }
})();
//# sourceMappingURL=App.RankingsMenu.Controller.js.map