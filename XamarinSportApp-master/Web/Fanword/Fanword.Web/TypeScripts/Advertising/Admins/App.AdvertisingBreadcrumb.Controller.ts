(() => {
    'use strict';

    angular.module('FanwordApp').controller('AdvertisingBreadcrumbController', advertisingBreadcrumbController);

    advertisingBreadcrumbController.$inject = ['$scope', '$state', '$http'];

    function advertisingBreadcrumbController($scope: any, $state: any, $http: ng.IHttpService) {
        $scope.crumbs = [];
        if ($state.current.name == "root" || $state.current.name == "root2") {
            defaultCrumbs();
        }
        if ($state.current.name == "viewAdvertiser") {
            defaultCrumbs();
            //load and set another breadcrumb
            $http.get('/api/Advertisers/' + $state.params.id).then((promise: any) => {
                $scope.crumbs.push(new Breadcrumb(promise.data.companyName, "/Advertising#!/View/" + promise.data.id));
            });
        }

        if ($state.current.name == "viewCampaign") {
            defaultCrumbs();
            $http.get('/api/Advertisers/' + $state.params.advertiserId).then((promise: any) => {
                $scope.crumbs.push(new Breadcrumb(promise.data.companyName, "/Advertising#!/View/" + promise.data.id));
                $http.get('/api/Campaigns/' + $state.params.id).then((promise: any) => {
                    $scope.crumbs.push(new Breadcrumb(promise.data.title, "/Advertising#!/View/" + $state.params.advertiserId + "/Campaign/" + $state.params.id));
                });
            });
            
        }

        //if ($state.current.name == "addNewSchool") {
        //    defaultCrumbs();
        //    $scope.crumbs.push(new Breadcrumb("New School", ""));
        //}

        function defaultCrumbs() {
            $scope.crumbs.push(new Breadcrumb("Home", "/Home"));
            $scope.crumbs.push(new Breadcrumb("Advertisers", "/Advertising"));
        }
    }
})();


