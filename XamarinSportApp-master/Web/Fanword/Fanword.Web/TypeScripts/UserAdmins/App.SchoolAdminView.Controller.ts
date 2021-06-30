(() => {
    'use strict';

    angular.module("FanwordApp").controller("SchoolAdminViewController", schoolAdminViewController);

    schoolAdminViewController.$inject = ['$scope', '$http', '$state', '$timeout'];

    function schoolAdminViewController($scope: any, $http: ng.IHttpService, $state: any, $timeout: any) {


        function loadControllerData() {
            if ($state.current.name == "viewSchoolAdmin") {
                var id = "";
                $http.get('/api/UserAdmins/SchoolSingle?id=' + $state.params.id).then((promise: any) => {
                    $scope.userAdmin = promise.data as SchoolAdmin;
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
            return $http.put('/api/UserAdmins/School', $scope.userAdmin).then((promise: any) => {
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