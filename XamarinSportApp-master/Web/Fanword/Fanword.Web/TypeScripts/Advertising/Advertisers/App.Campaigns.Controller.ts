(() => {
    'use strict';

    angular.module('FanwordApp').controller('CampaignsController', campaignsController);

    campaignsController.$inject = ['$scope', '$http', '$state'];

    function campaignsController($scope: any, $http: ng.IHttpService, $state: any) {
        $scope.searchText = "";
        function setupControls() {
            $scope.campaignOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Campaigns/Grid",
                        }
                    },
                    schema:{
                        model: {
                            fields: {
                                startUtc: { type: "date" },
                                endUtc:{type:"date"}
                            }
                        }
                    },
                    pageSize: 25,
                }),
                toolbar: [
                    {
                        template:
                            "<input type='text' class='k-textbox agilx-name-lg pull-left' ng-model='searchText' ng-change='onSearchChange()' style='width:300px'  placeholder='Search by title or profile' />" +
                                "<select kendo-drop-down-list k-options='statusOptions' k-ng-model='statusValue' style='margin-left:15px; width:250px;'></select>" +
                                "<button type='button' class='btn btn-primary pull-right' ng-click='addNew()'>Add New</button>"
                    }],
                columns: [
                    {
                        field: "imageUrl",
                        title: " ",
                        template: "<img src='#=imageUrl#' alt='' style='width:50px; height:50px;' />",
                        width: "100px;",
                        attributes: {
                            style:"text-align:center;"
                        },
                        filterable: false,
                        groupable: false,
                        sortable:false
                    }, {
                        field: "title",
                        title: "Title",
                        width: "150px;"
                    }, {
                        field: "weight",
                        title: "Frequency",
                        template: "# if(weight == 1){#Normal#} if(weight == 2){#High#}if(weight == 4){#Highest#} #",
                        width: "150px;"
                    }, {
                        field: "startUtc",
                        title: "Start",
                        template: "#= kendo.toString(startUtc,'MM/dd/yyyy') #",
                        width: "150px;"
                    }, {
                        field: "endUtc",
                        title: "End",
                        template: "#= kendo.toString(endUtc,'MM/dd/yyyy') #",
                        width: "150px;"
                    }, {
                        field: "profiles",
                        title: "Profiles",
                        sortable: false,
                        filterable: false,
                        groupable: false
                    }, {
                        field: "campaignStatus",
                        title: "Status",
                        template: "{{getStatusText(this.dataItem)}}",
                        width:"150px;"
                    }],
                sortable: true,
                scrollable: false,
                selectable: true,
                pageable: {
                    pageSizes: ['25', '50', '75', 'all'],
                    refresh: true,
                },
                change: (e: any) => {
                    var rowData = e.sender.dataItem(e.sender.select());
                    $state.go("viewCampaign", { id: rowData.id });
                    
                }
            }

            $scope.statusOptions = {
                dataSource: new kendo.data.DataSource({
                    data: [
                        {
                            displayName: "Pending",
                            id: CampaignStatus.Pending
                        }, {
                            displayName: "Approved",
                            id: CampaignStatus.Approved,
                        }, {
                            displayName: "Denied",
                            id: CampaignStatus.Denied
                        }]
                }),
                change:(e: any) => {
                    $scope.onSearchChange();
                },
                optionLabel:"--Select a Status--",
                dataTextField: "displayName",
                dataValueField: "id",
                valuePrimitive: true,
            }
        };

        $scope.addNew =() => {
            $state.go('addNewCampaign');
        }

        $scope.onSearchChange = () => {
            var filters = [];
            if ($scope.searchText.length == 0) {
                //clear filters
                
            } else {
                //set filters
                filters.push({
                    logic: 'or',
                    filters: [{
                        field: "title",
                        operator: "contains",
                        value: $scope.searchText
                    }, {
                            field: "profiles",
                        operator: "contains",
                        value: $scope.searchText
                    }]
                });
            }
            if ($scope.statusValue != undefined && $scope.statusValue !== '') {
                
                filters.push({
                    field: "campaignStatus",
                    operator: "eq",
                    value: $scope.statusValue,
                });
            }

            $scope.campaignOptions.dataSource.filter(filters);
        }


        $scope.getStatusText = (kendoData: any) => {
            switch (kendoData.campaignStatus) {
                case CampaignStatus.Approved:
                    return "Approved";
                case CampaignStatus.Denied:
                    return "Denied";
                case CampaignStatus.Pending:
                    return "Pending";
                default:
                    return "";
            }
        }

        setupControls();
    }
})();