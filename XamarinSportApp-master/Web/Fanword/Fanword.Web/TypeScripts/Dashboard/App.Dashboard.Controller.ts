(() => {
    'use strict';

    angular.module('FanwordApp').controller('DashboardController', dashboardController);

    dashboardController.$inject = ['$scope', '$http', '$state', '$timeout'];

    function dashboardController($scope: any, $http: ng.IHttpService, $state: any, $timeout: any) {
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

            $http.get('/api/Users/Count/'+ 0).then((promise: any) => {
                $scope.vm.userCountTotal = promise.data;
            });

            $http.get('/api/Users/Count/' + 1).then((promise: any) => {
                $scope.vm.userCountDay = promise.data;
            });

            $http.get('/api/Users/Count/' + 2).then((promise: any) => {
                $scope.vm.userCountWeek = promise.data;
            });

            $http.get('/api/Users/Count/' + 3).then((promise: any) => {
                $scope.vm.userCountMonth = promise.data;
            });

            $http.get('/api/ContentSources/Count').then((promise: any) => {
                $scope.vm.contentSourceCount = promise.data;
            });

            $http.get('/api/UserAdmins/CountSchool').then((promise: any) => {
                $scope.vm.schoolAdminCount = promise.data;
            });

            $http.get('/api/UserAdmins/CountTeam').then((promise: any) => {
                $scope.vm.teamAdminCount = promise.data;
            });

            $http.get('/api/Users/CountAthletes').then((promise: any) => {
                $scope.vm.athleteCount = promise.data;
            });

            $http.get('/api/Advertisers/Count').then((promise: any) => {
                $scope.vm.advertiserCount = promise.data;
            });

            $http.get('/api/Campaigns/Count').then((promise: any) => {
                $scope.vm.campaignCount = promise.data;
            });

            $http.get('/api/Campaigns/PendingCount').then((promise: any) => {
                $scope.vm.campaignPendingCount = promise.data;
            });

            $http.get('/api/NewsNotifications/PendingCount').then((promise: any) => {
                $scope.vm.newsNotificationPendingCount = promise.data;
            });

            $http.get('/api/RssFeeds/PendingCount').then((promise: any) => {
                $scope.vm.rssFeedPendingCount = promise.data;
            });

            $http.get('/api/ContentSources/PendingCount').then((promise: any) => {
                $scope.vm.contentSourcePendingCount = promise.data;
            });

            $http.get('/api/UserAdmins/PendingCount').then((promise: any) => {
                $scope.vm.userAdminPendingCount = promise.data;
            });

            $http.get('/api/Users/PendingCountAthletes').then((promise: any) => {
                $scope.vm.athletePendingCount = promise.data;
            });
        }


        loadData();
    }
})()