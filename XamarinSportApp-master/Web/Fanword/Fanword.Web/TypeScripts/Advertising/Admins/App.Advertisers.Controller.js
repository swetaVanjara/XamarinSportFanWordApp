(function () {
    'use strict';
    angular.module('FanwordApp').controller('AdvertisersController', advertisersController);
    advertisersController.$inject = ['$scope', '$http', '$state'];
    function advertisersController($scope, $http, $state) {
        $scope.searchText = "";
        function setupControls() {
            $scope.advertiserOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Advertisers/Grid"
                        }
                    },
                    pageSize: 25,
                }),
                toolbar: [
                    {
                        template: "<input type='text' class='k-textbox agilx-name-lg pull-left' ng-model='searchText' ng-change='onSearchChange()' style='width:300px'  placeholder='Search name' />" +
                            "<div class='md-checkbox pull-right'><input type='checkbox' id= 'checkbox2' class='md-check' ng-click='onSearchChange()' ng-model='pendingOnly'><label for='checkbox2'><span class='inc'></span><span class='check' > </span><span class='box' > </span>Show Pending Only</label></div>"
                    }
                ],
                columns: [
                    {
                        field: "name",
                        title: "Advertiser Name"
                    }, {
                        title: "Campaigns",
                        template: "# if(numberPendingCampaigns >0) {#" +
                            "#=numberPendingCampaigns# pending" +
                            "# } #",
                        width: "175px"
                    }
                ],
                scrollable: false,
                pageable: {
                    pageSizes: ['25', '50', '75', 'all'],
                    refresh: true,
                },
                selectable: true,
                change: function (e) {
                    var rowData = e.sender.dataItem(e.sender.select());
                    $state.go('viewAdvertiser', { id: rowData.id });
                }
            };
        }
        ;
        $scope.onSearchChange = function () {
            var filters = [];
            if ($scope.searchText.length == 0) {
                //clear filters
            }
            else {
                //set filters
                filters.push({
                    logic: "or",
                    filters: [
                        {
                            field: "name",
                            operator: "contains",
                            value: $scope.searchText
                        }
                    ]
                });
                //$scope.postOptions.dataSource.filter();
            }
            if ($scope.pendingOnly) {
                filters.push({
                    field: "numberPendingCampaigns",
                    operator: "gte",
                    value: 1,
                });
            }
            //this will clear if none since filters will be empty array
            $scope.advertiserOptions.dataSource.filter(filters);
        };
        setupControls();
    }
})();
//# sourceMappingURL=App.Advertisers.Controller.js.map