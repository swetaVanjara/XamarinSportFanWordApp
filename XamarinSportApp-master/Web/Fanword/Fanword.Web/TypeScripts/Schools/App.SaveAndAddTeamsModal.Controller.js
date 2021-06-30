(function () {
    'use strict';
    angular.module('FanwordApp').controller('SaveAndAddTeamsModalController', saveAndAddTeamsModalController);
    saveAndAddTeamsModalController.$inject = ['$scope', '$http', '$uibModalInstance', 'schoolId', 'wasCreating'];
    function saveAndAddTeamsModalController($scope, $http, $uibModalInstance, schoolId, wasCreating) {
        $scope.wasCreating = wasCreating;
        $scope.schoolId = schoolId;
        $scope.selectedSportIds = [];
        $scope.sportOptions = {
            dataSource: new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "/api/Teams/SportsNotAssignedToTeam",
                        data: function () {
                            return {
                                schoolId: schoolId,
                            };
                        }
                    }
                }
            }),
            dataTextField: "displayName",
            dataValueField: "id",
            valuePrimitive: true,
            placeholder: "--Select a Sport--",
        };
        $scope.saveAndAddTeams = function () {
            if ($scope.selectedSportIds.length == 0) {
                swal("Error", "Please choose at least one sport", "error");
                return;
            }
            return $http.post('/api/Teams/AddFromSportAndSchool', { schoolId: schoolId, sportIds: $scope.selectedSportIds }).then(function (promise) {
                $uibModalInstance.close();
            });
        };
        $scope.justSaveSchool = function () {
            $uibModalInstance.dismiss();
        };
    }
})();
//# sourceMappingURL=App.SaveAndAddTeamsModal.Controller.js.map