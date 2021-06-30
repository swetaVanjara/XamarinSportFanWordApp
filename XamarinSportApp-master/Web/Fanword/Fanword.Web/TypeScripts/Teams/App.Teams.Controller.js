(function () {
    'use strict';
    angular.module('FanwordApp').controller('TeamsController', teamsController);
    teamsController.$inject = ['$scope', '$http', '$state'];
    function teamsController($scope, $http, $state) {
        function loadControllerData() {
            setupScopeFunctions();
            setupControls();
        }
        $scope.showInactive = false;
        function setupControls() {
            $scope.teamOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Teams/Grid",
                            data: function () {
                                return {
                                    showInactive: $scope.showInactive
                                };
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
                        template: "<input type='text' class='k-textbox agilx-name-lg pull-left' ng-model='searchText' ng-change='onSearchChange()' style='width:300px'  placeholder='Search by name' />" +
                            "<button type='button' class='btn btn-primary pull-right' ng-click='addNew()'>Add New</button>"
                    }
                ],
                columns: [
                    {
                        field: "profilePublicUrl",
                        title: "Profile Picture",
                        template: "<img style='width:50px; height:50px;' src='#=profilePublicUrl#' alt='' />",
                        width: "100px",
                        attributes: {
                            style: "text-align:center;"
                        },
                        sortable: false
                    }, {
                        field: "name",
                        title: "Name"
                    }, {
                        field: "schoolName",
                        title: "School"
                    }, {
                        field: "sportName",
                        title: "Sport"
                    }, {
                        title: "Status",
                        template: "# if (isActive) { #" +
                            "# if (!isSchoolActive) { #" +
                            "School is inactive" +
                            " # } else if (!isSportActive) { #" +
                            "Sport is inactive" +
                            "# } else { #" +
                            "Active" +
                            "# } #" +
                            "# } else { #" +
                            "Inactive" +
                            "# } #",
                        field: "status"
                    }, {
                        title: "Delete",
                        width: "100px",
                        template: "<button type='button' class='btn btn-danger' ng-click='deleteTeam(this.dataItem)'>Delete</button>"
                    }
                ],
                sortable: true,
                scrollable: false,
                pageable: {
                    pageSizes: ['25', '50', '75', 'all'],
                    refresh: true,
                },
                selectable: true,
                change: function (e) {
                    var rowData = e.sender.dataItem(e.sender.select());
                    var url = $state.href('viewTeam', { id: rowData.id });
                    window.open(url, '_blank');
                }
            };
        }
        function setupScopeFunctions() {
            $scope.onSearchChange = function () {
                if ($scope.searchText.length == 0) {
                    //clear filters
                    $scope.teamOptions.dataSource.filter([]);
                }
                else {
                    //set filters
                    $scope.teamOptions.dataSource.filter([
                        {
                            field: "name",
                            operator: "contains",
                            value: $scope.searchText
                        }
                    ]);
                }
            };
            $scope.deleteTeam = function (dataItem) {
                swal({
                    title: 'Are you sure?',
                    text: "Are you sure you want to delete '" + dataItem.name + "'?",
                    showCancelButton: true,
                    confirmButtonText: 'Delete',
                    confirmButtonColor: "#d9534f",
                    showLoaderOnConfirm: true,
                    type: "question",
                    preConfirm: function () {
                        return $http.delete('/api/Teams/' + dataItem.id);
                    },
                    allowOutsideClick: false
                }).then(function () {
                    $scope.teamOptions.dataSource.read();
                    swal({
                        type: 'success',
                        text: 'Team Removed Successfully',
                        title: "Success"
                    });
                }, function (cancel) {
                });
            };
            $scope.addNew = function () {
                $state.go('addNewTeam');
            };
        }
        $scope.reloadGrid = function () {
            $scope.teamOptions.dataSource.read();
        };
        loadControllerData();
    }
})();
//# sourceMappingURL=App.Teams.Controller.js.map