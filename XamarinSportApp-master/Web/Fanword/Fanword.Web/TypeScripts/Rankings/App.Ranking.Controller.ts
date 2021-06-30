(() => {
    'use strict';

    angular.module('FanwordApp').controller('RankingController', rankingController);

    rankingController.$inject = ['$scope', '$http', '$state', '$rootScope'];

    function rankingController($scope: any, $http: ng.IHttpService, $state: any, $rootScope: any) {
        function setupControls() {
            if ($state.current.name == "viewRanking") {
                $scope.teamOptions = {
                    dataSource: new kendo.data.DataSource({
                        transport: {
                            read: {
                                url: "/api/Teams/SelectControlList"
                            }
                        },
                        filter: [
                            {
                                field: "sportId",
                                operator: "contains",
                                value: $state.params.id,
                            }]
                    }),
                    dataTextField: "displayName",
                    dataValueField: "id",
                    valuePrimitive: true,
                    optionLabel: {
                        displayName: "--Select a Team--",
                        id: null
                    },
                    filter: 'contains'
                }
            }
        };

        function loadControllerData() {
            if ($state.params.id == undefined) return;
            $http.get('/api/Rankings/BySport/' + $state.params.id).then((promise: any) => {
                $scope.ranking = promise.data as Ranking;
                $rootScope.$emit('setMenuItem', { id: $state.params.id });
            });
        }

        $scope.save = () => {
            $scope.modelState = [];
            return $http.put('/api/Rankings', $scope.ranking).then((promise: any) => {
                swal("Success", $scope.ranking.sportName + " was updated successfully", "success");
                $state.go('root');
            }, (error: any) => {
                if (error && error.data && error.data.modelState) {
                    $scope.modelState = error.data.modelState;
                }
            });
        }

        setupControls();
        loadControllerData();
    }
})();