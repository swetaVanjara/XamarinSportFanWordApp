(function () {
    'use strict';
    angular.module('FanwordApp').controller('DashboardController', dashboardController);
    dashboardController.$inject = ['$scope', '$http', '$state', '$timeout'];
    function dashboardController($scope, $http, $state, $timeout) {
        $scope.vm = {};
        $scope.vm.userCountDay = 0;
        $scope.vm.userCountWeek = 0;
        $scope.vm.userCountMonth = 0;
        $scope.vm.userCountTotal = 0;
        $scope.vm.contentSourceCount = 0;
        $scope.vm.schoolAdminCount = 0;
        $scope.vm.teamAdminCount = 0;
        $scope.vm.athleteCount = 0;
        $scope.vm.advertiserCount = 0;
        $scope.vm.campaignCount = 0;
        $scope.vm.campaignPendingCount = 0;
        $scope.vm.newsNotificationPendingCount = 0;
        $scope.vm.rssFeedPendingCount = 0;
        $scope.vm.contentSourcePendingCount = 0;
        $scope.vm.userAdminPendingCount = 0;
        $scope.vm.athletePendingCount = 0;
        function loadData() {
            $http.get('/api/Users/Count/' + 0).then(function (promise) {
                $scope.vm.userCountTotal = promise.data;
            });
            $http.get('/api/Users/Count/' + 1).then(function (promise) {
                $scope.vm.userCountDay = promise.data;
            });
            $http.get('/api/Users/Count/' + 2).then(function (promise) {
                $scope.vm.userCountWeek = promise.data;
            });
            $http.get('/api/Users/Count/' + 3).then(function (promise) {
                $scope.vm.userCountMonth = promise.data;
            });
            $http.get('/api/ContentSources/Count').then(function (promise) {
                $scope.vm.contentSourceCount = promise.data;
            });
            $http.get('/api/UserAdmins/CountSchool').then(function (promise) {
                $scope.vm.schoolAdminCount = promise.data;
            });
            $http.get('/api/UserAdmins/CountTeam').then(function (promise) {
                $scope.vm.teamAdminCount = promise.data;
            });
            $http.get('/api/Users/CountAthletes').then(function (promise) {
                $scope.vm.athleteCount = promise.data;
            });
            $http.get('/api/Advertisers/Count').then(function (promise) {
                $scope.vm.advertiserCount = promise.data;
            });
            $http.get('/api/Campaigns/Count').then(function (promise) {
                $scope.vm.campaignCount = promise.data;
            });
            $http.get('/api/Campaigns/PendingCount').then(function (promise) {
                $scope.vm.campaignPendingCount = promise.data;
            });
            $http.get('/api/NewsNotifications/PendingCount').then(function (promise) {
                $scope.vm.newsNotificationPendingCount = promise.data;
            });
            $http.get('/api/RssFeeds/PendingCount').then(function (promise) {
                $scope.vm.rssFeedPendingCount = promise.data;
            });
            $http.get('/api/ContentSources/PendingCount').then(function (promise) {
                $scope.vm.contentSourcePendingCount = promise.data;
            });
            $http.get('/api/UserAdmins/PendingCount').then(function (promise) {
                $scope.vm.userAdminPendingCount = promise.data;
            });
            $http.get('/api/Users/PendingCountAthletes').then(function (promise) {
                $scope.vm.athletePendingCount = promise.data;
            });
        }
        loadData();
    }
})();
//# sourceMappingURL=App.Dashboard.Controller.js.map