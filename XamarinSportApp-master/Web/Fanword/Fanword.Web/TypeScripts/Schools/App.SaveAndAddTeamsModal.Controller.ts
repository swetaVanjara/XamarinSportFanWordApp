(() => {
    'use strict';

    angular.module('FanwordApp').controller('SaveAndAddTeamsModalController', saveAndAddTeamsModalController);

    saveAndAddTeamsModalController.$inject = ['$scope', '$http', '$uibModalInstance', 'schoolId','wasCreating'];

    function saveAndAddTeamsModalController($scope: any,
        $http: ng.IHttpService,
        $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
        schoolId: string,
        wasCreating:any) {
        $scope.wasCreating = wasCreating;
        $scope.schoolId = schoolId;
        $scope.selectedSportIds = [];
        $scope.sportOptions = {
            dataSource: new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "/api/Teams/SportsNotAssignedToTeam",
                        data: () => {
                            return {
                                schoolId: schoolId,
                            }
                        }
                    }
                }
            }),
            dataTextField: "displayName",
            dataValueField: "id",
            valuePrimitive: true,
            placeholder: "--Select a Sport--",
        }


        $scope.saveAndAddTeams = () => {
            if ($scope.selectedSportIds.length == 0) {
                swal("Error", "Please choose at least one sport", "error");
                return;
            }
            return $http.post('/api/Teams/AddFromSportAndSchool',{schoolId:schoolId,sportIds:$scope.selectedSportIds}).then((promise: any) => {
                $uibModalInstance.close();
            });
        };

        $scope.justSaveSchool =() => {
            $uibModalInstance.dismiss();
        }
    }
})();