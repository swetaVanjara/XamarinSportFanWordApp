(function () {
    'use strict';
    angular.module('FanwordApp').controller('SchoolsController', schoolsController);
    schoolsController.$inject = ['$scope', '$http', '$state'];
    function schoolsController($scope, $http, $state) {
        function loadControllerData() {
            setupScopeFunctions();
            setupControls();
        }
        $scope.showInactive = false;
        function setupControls() {
            $scope.schoolOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Schools/Grid",
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
                        title: "Status",
                        template: "# if (isActive) { #" +
                            "Active" +
                            "# } else { #" +
                            "Inactive" +
                            "# } #",
                        field: "status"
                    }, {
                        field: "numberOfTeams",
                        title: "Teams"
                    }, {
                        field: "numberOfFollowers",
                        title: "Followers"
                    }, {
                        field: "numberOfPosts",
                        title: "Posts"
                    }, {
                        field: "numberOfAthletes",
                        title: "Athletes"
                    }, {
                        title: "Delete",
                        width: "100px",
                        template: "<button type='button' class='btn btn-danger' ng-click='deleteSchool(this.dataItem)'>Delete</button>"
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
                    var url = $state.href('viewSchool', { id: rowData.id });
                    window.open(url, '_blank');
                }
            };
        }
        function setupScopeFunctions() {
            $scope.onSearchChange = function () {
                if ($scope.searchText.length == 0) {
                    //clear filters
                    $scope.schoolOptions.dataSource.filter([]);
                }
                else {
                    //set filters
                    $scope.schoolOptions.dataSource.filter([
                        {
                            field: "name",
                            operator: "contains",
                            value: $scope.searchText
                        }
                    ]);
                }
            };
            $scope.deleteSchool = function (dataItem) {
                swal({
                    title: 'Are you sure?',
                    text: "Are you sure you want to delete '" + dataItem.name + "'? This will also delete any Teams associated with this school.",
                    showCancelButton: true,
                    confirmButtonText: 'Delete',
                    confirmButtonColor: "#d9534f",
                    showLoaderOnConfirm: true,
                    type: "question",
                    preConfirm: function () {
                        return $http.delete('/api/Schools/' + dataItem.id);
                    },
                    allowOutsideClick: false
                }).then(function () {
                    $scope.schoolOptions.dataSource.read();
                    swal({
                        type: 'success',
                        text: 'School Removed Successfully',
                        title: "Success"
                    });
                }, function (cancel) {
                });
            };
            $scope.addNew = function () {
                $state.go('addNewSchool');
            };
        }
        $scope.reloadGrid = function () {
            $scope.schoolOptions.dataSource.read();
        };
        loadControllerData();
    }
})();
//# sourceMappingURL=App.Schools.Controller.js.map