(() => {
    'use strict';

    angular.module("FanwordApp").controller("SportsController", sportsController);

    sportsController.$inject = ['$scope', '$http', '$state', '$uibModal'];

    function sportsController($scope: any,
        $http: ng.IHttpService,
        $state: any,
        $uibModal: ng.ui.bootstrap.IModalService) {

        function loadControllerData() {
            setScopeFunctions();
            setupControls();
        }

        $scope.showInactive = false;

        function setupControls() {
            $scope.sportOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Sports/Grid",
                            data: () => {
                                return {
                                    showInactive: $scope.showInactive
                                }
                            }
                        }
                    },
                    pageSize: 25,
                    sort: {
                        field: "name",
                        dir: "asc"
                    }
                }),
                toolbar: [
                    {
                        template:
                            "<input type='text' class='k-textbox agilx-name-lg pull-left' ng-model='searchText' ng-change='onSearchChange()' style='width:300px'  placeholder='Search by name' />" +
                                "<button type='button' class='btn btn-primary pull-right' ng-click='addNew()'>Add New</button>"
                    }
                ],
                columns: [
                    {
                        field: "iconPublicUrl",
                        title: "Profile Picture",
                        template: "<img style='width:50px; height:50px;' src='#=iconPublicUrl#' alt='' />",
                        width: "100px",
                        attributes: {
                            style: "text-align:center;"
                        },
                        sortable: false
                    }, {
                        field: "name",
                        title: "Name"
                    }, {
                        title: "Status",
                        template: "# if (isActive) { #" +
                            "Active" +
                            "# } else { #" +
                            "Inactive" +
                        "# } #",
                        field: "status"
                    }, {
                        title: "Delete",
                        width: "100px",
                        template:"<button type='button' class='btn btn-danger' ng-click='deleteSport(this.dataItem)'>Delete</button>"
                    }
                ],
                sortable: true,
                scrollable: false,
                pageable: {
                    pageSizes: ['25', '50', '75', 'all'],
                    refresh: true,
                },
                selectable: true,
                change: (e: any) => {
                    var rowData = e.sender.dataItem(e.sender.select());
                    var url = $state.href('viewSport', { id: rowData.id });
                    window.open(url, '_blank');
                }
            };
        }

        function setScopeFunctions() {
            $scope.onSearchChange = () => {
                if ($scope.searchText.length == 0) {
                    //clear filters
                    $scope.sportOptions.dataSource.filter([]);
                } else {
                    //set filters
                    $scope.sportOptions.dataSource.filter([
                        {
                            field: "name",
                            operator: "contains",
                            value: $scope.searchText
                        }
                    ]);
                }
            };
            

            $scope.deleteSport = (dataItem: any) => {
                swal({
                    title:'Are you sure?',
                    text: "Are you sure you want to delete '" + dataItem.name + "'?  This will also delete any Teams associated with this sport.",
                    showCancelButton: true,
                    confirmButtonText: 'Delete',
                    confirmButtonColor:"#d9534f",
                    showLoaderOnConfirm: true,
                    type:"question",
                    preConfirm: function () {
                        return $http.delete('/api/Sports/' + dataItem.id);
                    },
                    allowOutsideClick: false
                }).then(function () {
                    $scope.sportOptions.dataSource.read();
                    swal({
                        type: 'success',
                        text: 'Sport Removed Successfully',
                        title:"Success"
                    });
                },(cancel: any) => {
                });
            };

            $scope.addNew = () => {
                $state.go('addNewSport');
            }
        }

        $scope.reloadGrid = () => {
            $scope.sportOptions.dataSource.read();
        }

        loadControllerData();


    }
})();