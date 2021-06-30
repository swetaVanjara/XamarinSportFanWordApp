(() => {
    'use strict';

    angular.module('FanwordApp').controller('AdvertisersBreadcrumController', advertisersBreadcrumController);

    advertisersBreadcrumController.$inject = ['$scope', '$state', '$http'];

    function advertisersBreadcrumController($scope: any, $state: any, $http: ng.IHttpService) {
        $scope.crumbs = [];
        if ($state.current.name == "root" || $state.current.name == "root2") {
            defaultCrumbs();
        }
        if ($state.current.name == "viewCampaign") {
            defaultCrumbs();
            //load and set another breadcrumb
            $http.get('/api/Campaigns/' + $state.params.id).then((promise: any) => {
                $scope.crumbs.push(new Breadcrumb(promise.data.title, "/Campaigns#!/View/" + promise.data.id));
            });
        }

        if ($state.current.name == "addNewCampaign") {
            defaultCrumbs();
            $scope.crumbs.push(new Breadcrumb("New Campaign", ""));
        }

        function defaultCrumbs() {
            $scope.crumbs.push(new Breadcrumb("Home", "/Home"));
            $scope.crumbs.push(new Breadcrumb("Campaigns", "/Campaigns"));
        }
    }
})();


