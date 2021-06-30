(function () {
    'use strict';
    angular.module("FanwordApp").controller("NewUserAdminController", newUserAdminController);
    newUserAdminController.$inject = ['$scope', '$http', '$state', '$timeout'];
    function newUserAdminController($scope, $http, $state, $timeout) {
        function loadModalData() {
            $scope.schoolAdmin = new SchoolAdmin();
            $scope.teamAdmin = new TeamAdmin();
            $scope.email = "";
            $scope.forType = 'team';
        }
        function setupControls() {
            $scope.userOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Users/Dropdown"
                        }
                    }
                }),
                dataTextField: "displayName",
                dataValueField: "id",
                optionLabel: "--Select a User--",
                valuePrimitive: true,
                select: onSelect,
                filter: "contains"
            };
            $scope.forTypeOptions = {
                dataSource: new kendo.data.DataSource({
                    data: [{
                            displayName: "School",
                            id: 'school'
                        }, {
                            displayName: 'Team',
                            id: 'team'
                        }
                    ]
                }),
                dataTextField: "displayName",
                dataValueField: "id",
                optionLabel: "--Select a Type--",
                valuePrimitive: true
            };
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
            $scope.schoolOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Schools/SelectControlList"
                        }
                    }
                }),
                dataTextField: "displayName",
                dataValueField: "id",
                valuePrimitive: true,
                optionLabel: "--Select a School--",
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
                        }
                    ]
                }),
                dataTextField: "displayName",
                dataValueField: "id",
                valuePrimitive: true,
            };
            function onSelect(e) {
                $scope.email = e.dataItem.displayName;
            }
            ;
        }
        $scope.saveSchool = function () {
            $scope.schoolAdmin.ContactEmail = $scope.email;
            console.log($scope.schoolAdmin);
            $scope.forError = "";
            $scope.modelState = [];
            return $http.post('/api/UserAdmins/NewSchool', $scope.schoolAdmin).then(function (promise) {
                $state.go('root');
                swal("Saved.", "Saved successfully", "success");
            }, function (error) {
                if (error && error.data && error.data.modelState) {
                    $scope.modelState = error.data.modelState;
                }
            });
        };
        $scope.saveTeam = function () {
            $scope.teamAdmin.ContactEmail = $scope.email;
            console.log($scope.teamAdmin);
            $scope.forError = "";
            $scope.modelState = [];
            return $http.post('/api/UserAdmins/NewTeam', $scope.teamAdmin).then(function (promise) {
                $state.go('root');
                swal("Saved.", "Saved successfully", "success");
            }, function (error) {
                if (error && error.data && error.data.modelState) {
                    $scope.modelState = error.data.modelState;
                }
            });
        };
        $scope.cancel = function () {
            $state.go('root');
        };
        loadModalData();
        setupControls();
    }
})();
//# sourceMappingURL=App.NewUserAdmin.Controller.js.map