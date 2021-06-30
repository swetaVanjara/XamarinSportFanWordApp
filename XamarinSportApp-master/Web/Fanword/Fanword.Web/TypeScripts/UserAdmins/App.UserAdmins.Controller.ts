(() => {
    'use strict';

    angular.module('FanwordApp').controller('UserAdminsController', userAdminsController);

    userAdminsController.$inject = ['$scope', '$http', '$state', 'moment'];

    function userAdminsController($scope: any, $http: ng.IHttpService, $state: any, moment: any) {
        $scope.vm = {};
        $scope.vm.teamSearchText = "";
        $scope.vm.schoolSearchText = "";
        $scope.vm.showPending = false;

        function setupControls() {

            $scope.teamAdminOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/UserAdmins/TeamGrid",
                            data: () => {
                                return {
                                    showPending: $scope.vm.showPending
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
                        "<input type='text' class='k-textbox agilx-name-lg pull-left' ng-model='vm.teamSearchText' ng-change='onTeamSearchChange()' style='width:300px'  placeholder='Search by name' />" +
                        "<button type='button' class='btn btn-primary pull-right' ng-click='addNew()'>Add New</button>"
                    }
                ],
                columns: [
                    {
                        field: "name",
                        title: "Contact Name",
                    }, {
                        field: "teamName",
                        title: "Team Name"
                    }, {
                        field: "status",
                        title: "Status",
                        template: "{{getStatusText(this.dataItem)}}"
                    }],
                pageable: {
                    pageSizes: ['25', '50', '75', 'all'],
                    refresh: true,
                },
                sortable: true,
                scrollable: false,
                selectable: true,
                change: (e: any) => {
                    var rowData = e.sender.dataItem(e.sender.select());
                    //console.log(rowData);
                    $state.go('viewTeamAdmin', { id: rowData.id});
                }
            };

            $scope.schoolAdminOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/UserAdmins/SchoolGrid",
                            data: () => {
                                return {
                                    showPending: $scope.vm.showPending
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
                        "<input type='text' class='k-textbox agilx-name-lg pull-left' ng-model='vm.schoolSearchText' ng-change='onSchoolSearchChange()' style='width:300px'  placeholder='Search by name' />" +
                        "<button type='button' class='btn btn-primary pull-right' ng-click='addNew()'>Add New</button>"
                    }
                ],
                columns: [
                    {
                        field: "name",
                        title: "Contact Name",
                    }, {
                        field: "schoolName",
                        title: "School Name"
                    }, {
                        field: "status",
                        title: "Status",
                        template: "{{getStatusText(this.dataItem)}}"
                    }],
                pageable: {
                    pageSizes: ['25', '50', '75', 'all'],
                    refresh: true,
                },
                sortable: true,
                scrollable: false,
                selectable: true,
                change: (e: any) => {
                    var rowData = e.sender.dataItem(e.sender.select());
                    $state.go('viewSchoolAdmin', { id: rowData.id});
                }
            };
            $scope.getStatusText = (kendoData: any) => {
                switch (kendoData.status) {
                    case AdminStatus.Approved:
                        return "Approved";
                    case AdminStatus.Denied:
                        return "Denied";
                    case AdminStatus.Pending:
                        return "Pending";
                    default:
                        return "";
                }
            };
        };

        $scope.onTeamSearchChange = () => {
            var filters = [];
            console.log($scope.vm.teamSearchText + " <- search text");
            if ($scope.vm.teamSearchText.length == 0) {
                //clear filters
            } else {
                //set filters
                filters.push(
                    {
                        logic: "or",
                        filters: [
                            {
                                field: "name",
                                operator: "contains",
                                value: $scope.vm.teamSearchText
                            }
                        ]
                    }
                );

            }
            $scope.teamAdminOptions.dataSource.filter(filters);
        };

        $scope.onSchoolSearchChange = () => {
            var filters = [];
            console.log($scope.vm.schoolSearchText + " <- school search text");
            if ($scope.vm.schoolSearchText.length == 0) {
                //clear filters
            } else {
                //set filters
                filters.push(
                    {
                        logic: "or",
                        filters: [
                            {
                                field: "name",
                                operator: "contains",
                                value: $scope.vm.schoolSearchText
                            }
                        ]
                    }
                );

            }
            $scope.schoolAdminOptions.dataSource.filter(filters);
        };
        $scope.addNew = () => {
            $state.go('addNewUserAdmin');
        }

        $scope.reinstate = (kendoData: any) => {
            swal({
                title: 'Are you sure?',
                text: "Are you sure you want to reinstate '" + kendoData.name + "'?",
                showCancelButton: true,
                confirmButtonText: 'Reinstate',
                confirmButtonColor: "#ec971f",
                showLoaderOnConfirm: true,
                type: "question",
                preConfirm: function () {
                    return $http.get('/api/Users/Reinstate?id=' + kendoData.id);
                },
                allowOutsideClick: false
            }).then(function () {
                swal({
                    type: 'success',
                    text: 'User Reinstated Successfully',
                    title: "Success"
                });
                $state.go('viewUser', { id: kendoData.id });
            }, (cancel: any) => {
            });
        }
        $scope.reloadGrid = () => {
            $scope.teamAdminOptions.dataSource.read();
            $scope.schoolAdminOptions.dataSource.read();
        }
        
        setupControls();

    }
})();