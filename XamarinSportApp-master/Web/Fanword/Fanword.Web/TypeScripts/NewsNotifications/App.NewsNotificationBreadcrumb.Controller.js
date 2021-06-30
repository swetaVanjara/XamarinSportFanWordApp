(function () {
    'use strict';
    angular.module('FanwordApp').controller('NewsNotificationBreadcrumbController', newsNotificationBreadcrumbController);
    newsNotificationBreadcrumbController.$inject = ['$scope', '$http', '$state'];
    function newsNotificationBreadcrumbController($scope, $http, $state) {
        $scope.crumbs = [];
        if ($state.current.name == "root" || $state.current.name == "root2") {
            defaultCrumbs();
        }
        function defaultCrumbs() {
            $scope.crumbs.push(new Breadcrumb("Home", "/Home"));
            $scope.crumbs.push(new Breadcrumb("News Notifications", "/NewsNotifications"));
        }
    }
})();
//# sourceMappingURL=App.NewsNotificationBreadcrumb.Controller.js.map