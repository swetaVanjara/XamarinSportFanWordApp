
(() => {
    'use strict';

    angular.module('FanwordApp').controller('UserManagementController', userManagementController);

    userManagementController.$inject = ['$scope', '$http','$state','moment'];

    function userManagementController($scope: any, $http: ng.IHttpService, $state: any,moment:any) {
        $scope.showDeleted = false;
        $scope.showInactive = false;
        $scope.showPending = false;
        function setupControls() {
            $scope.userGridOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Users/Grid",
                            data:() => {
                                return{
                                    showDeleted: $scope.showDeleted,
                                    showInactive: $scope.showInactive,
                                    showPending: $scope.showPending
                                }
                            }
                        }
                    },
                    pageSize: 25,
                    schema: {
                        model: {
                            fields: {
                                dateCreatedUtc: { type: 'date' }
                            }
                        }
                    },
                    sort: {
                        field: "name",
                        dir: "asc"
                    }
                }),
                toolbar: [
                    {
                        template:
                            "<input type='text' class='k-textbox agilx-name-lg pull-left' ng-model='searchText' ng-change='onSearchChange()' style='width:300px'  placeholder='Search by name or email' />" +
                                "<button type='button' class='btn btn-primary pull-right' ng-click='addNew()'>Add New</button>"
                    }
                ],
                columns: [
                    {
                        title: "Profile Picture",
                        template: "<img src='#=profilePictureUrl#' alt='' style='width:50px; height:50px; border-radius: 25px;'/>",
                        width: "100px",
                        attributes: {
                            style: "text-align:center;"
                        }
                    }, {
                        field: "name",
                        title: "Name",
                    }, {
                        field: "email",
                        title: "Email"
                    }, {
                        field: "isStudentAthlete",
                        title: "Student-Athlete",
                        template:"# if(isStudentAthlete) { #" +
                            "Yes" +
                            "# } else { #" +
                            "No" +
                            "# } #"
                    }, {
                        field: "dateCreatedUtc",
                        title: "Date Added",
                        template:"#= kendo.toString(dateCreatedUtc,'MMM dd, yyyy h:mm tt')#"
                    }, {
                        field: "followers",
                        title: "Followers",
                    }, {
                        field: "posts",
                        title: "Posts"
                    }, {
                        field: "contentSource",
                        title: "Content Source"
                    }, {
                        title: " ",
                        template: "# if (isDeleted) { #" +
                            "<button type='button' class='btn btn-warning' ng-click='reinstate(this.dataItem)'>Reinstate</button>" +
                            "# } else { #" +
                            "<button type='button' class='btn btn-danger' ng-click='deleteUser(this.dataItem)'>Delete</button>" +
                            "# } #"
                    }],
                pageable: {
                    pageSizes: ['25', '50', '75', 'all'],
                    refresh: true,
                },
                sortable: true,
                scrollable: false,
                selectable: true,
                change:(e: any) => {
                    var rowData = e.sender.dataItem(e.sender.select());
                    $state.go('viewUser', { id: rowData.id });
                }
            };
        };

        $scope.reloadGrid = () => {
            $scope.userGridOptions.dataSource.read();
        }

        


        $scope.onSearchChange = () => {
            if ($scope.searchText.length == 0) {
                //clear filters
                $scope.userGridOptions.dataSource.filter([]);
            } else {
                //set filters
                $scope.userGridOptions.dataSource.filter([
                    {
                        logic: "or",
                        filters: [{
                            field: "name",
                            operator: "contains",
                            value: $scope.searchText
                        },
                            {
                                field: "email",
                                operator: "contains",
                                value: $scope.searchText
                            }]
                    }
                ]);
            }
        };

        $scope.addNew = () => {
            $state.go("addNewUser");
        };

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

        $scope.deleteUser = (kendoData: any) => {
            swal({
                title: 'Are you sure?',
                text: "Are you sure you want to delete '" + kendoData.name + "'?",
                showCancelButton: true,
                confirmButtonText: 'Delete',
                confirmButtonColor: "#d9534f",
                showLoaderOnConfirm: true,
                type: "question",
                preConfirm: function () {
                    return $http.delete('/api/Users/' + kendoData.id);
                },
                allowOutsideClick: false
            }).then(function () {
                $scope.userGridOptions.dataSource.read();
                swal({
                    type: 'success',
                    text: 'User Removed Successfully',
                    title: "Success"
                });
            }, (cancel: any) => {
            });
        }


        setupControls();

    }
})();