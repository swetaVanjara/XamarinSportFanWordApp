(() => {
    'use strict';

    angular.module("FanwordApp").controller("TeamAdminViewController", teamAdminViewController);

    teamAdminViewController.$inject = ['$scope', '$http', '$state', '$timeout'];

    function teamAdminViewController($scope: any, $http: ng.IHttpService, $state: any, $timeout: any) {


        function loadControllerData() {
            if ($state.current.name == "viewTeamAdmin") {
                var id = "";
                $http.get('/api/UserAdmins/TeamSingle?id=' + $state.params.id).then((promise: any) => {
                    $scope.userAdmin = promise.data as TeamAdmin;
                    id = $scope.userAdmin.userId;
                    $http.get('/api/Users/' + id).then((promise: any) => {
                        $scope.user = promise.data as User;
                        $scope.userAdmin.contactEmail = $scope.user.email;
                    });
                });
            }
            setupControls();
        }

        function setupControls() {
            $scope.teamOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Teams/SelectControlList"
                        }
                    }
                }),
                dataTextField: "displayName",
                dataValueField: "id",
                valuePrimitive: true,
                optionLabel: "--Select a Team--",
                filter: "contains"
            };
            $scope.statusOptions = {
                dataSource: new kendo.data.DataSource({
                    data: [
                        {
                            displayName: "Pending",
                            id: AdminStatus.Pending
                        }, {
                            displayName: "Approved",
                            id: AdminStatus.Approved,
                        }, {
                            displayName: "Denied",
                            id: AdminStatus.Denied
                        }]
                }),
                dataTextField: "displayName",
                dataValueField: "id",
                valuePrimitive: true,
            }
        }
        $scope.save = () => {
            $scope.forError = "";
            $scope.modelState = [];
                return $http.put('/api/UserAdmins/Team', $scope.userAdmin).then((promise: any) => {
                    $state.go('root');
                    swal("Saved.", "Saved successfully", "success");
                }, (error: any) => {
                    if (error && error.data && error.data.modelState) {
                        $scope.modelState = error.data.modelState;
                    }
                });
        }

        $scope.cancel = () => {
            $state.go('root');
        }

        loadControllerData();
    }


})();