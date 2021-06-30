(function () {
    'use strict';
    angular.module("FanwordApp").controller("NewsNotificationController", newsNotificationController);
    newsNotificationController.$inject = ['$scope', '$http', '$uibModal'];
    function newsNotificationController($scope, $http, $uibModal) {
        function setupControls() {
            $scope.sentOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/NewsNotifications/Grid",
                            data: function () {
                                return {};
                            }
                        }
                    },
                    schema: {
                        model: {
                            fields: {
                                pushDateUtc: { type: "date" }
                            }
                        }
                    },
                    filter: [
                        {
                            logic: 'and',
                            filters: [{
                                    field: "pushDateUtc",
                                    operator: "lt",
                                    value: new Date()
                                }, {
                                    field: "newsNotificationStatus",
                                    operator: "eq",
                                    value: NewsNotificationStatus.Approved
                                }]
                        }
                    ],
                    pageSize: 25,
                }),
                columns: [
                    {
                        field: "title",
                        title: "Title"
                    }, {
                        field: "content",
                        title: "Content"
                    }, {
                        field: "pushDateUtc",
                        title: "Push Date",
                        template: "#=kendo.toString(pushDateUtc,'MM/dd/yyyy')#",
                        width: "200px"
                    }, {
                        field: "createdBy",
                        title: "Created By",
                        width: "200px"
                    }, {
                        field: "newsNotificationStatus",
                        title: "Status",
                        template: "{{getStatusText(this.dataItem)}}",
                        width: "150px;"
                    }
                ],
                scrollable: false,
                pageable: {
                    refresh: true,
                    pageSizes: ['25', '50', '75', 'all']
                }
            };
            $scope.getStatusText = function (kendoData) {
                switch (kendoData.newsNotificationStatus) {
                    case NewsNotificationStatus.Approved:
                        return "Approved";
                    case NewsNotificationStatus.Denied:
                        return "Denied";
                    case NewsNotificationStatus.Pending:
                        return "Pending";
                    default:
                        return "";
                }
            };
            $scope.upcomingOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/NewsNotifications/Grid",
                        }
                    },
                    schema: {
                        model: {
                            fields: {
                                pushDateUtc: { type: "date" }
                            }
                        }
                    },
                    filter: [
                        {
                            logic: 'or',
                            filters: [{
                                    field: "pushDateUtc",
                                    operator: "gt",
                                    value: new Date()
                                }, {
                                    field: "newsNotificationStatus",
                                    operator: "neq",
                                    value: NewsNotificationStatus.Approved
                                }]
                        }
                    ],
                    pageSize: 25,
                }),
                columns: [
                    {
                        field: "title",
                        title: "Title"
                    }, {
                        field: "content",
                        title: "Content"
                    }, {
                        field: "pushDateUtc",
                        title: "Push Date",
                        template: "#=kendo.toString(pushDateUtc,'MM/dd/yyyy')#",
                        width: "200px"
                    }, {
                        field: "createdBy",
                        title: "Created By",
                        //template: "# if (contentSource == null) { #" +
                        //"<span data-content=''>Admin</span>" +
                        //"# } else { #" +
                        //"<span data-content=''>#:contentSource# </span>" +
                        //"# } #",
                        width: "200px"
                    }, {
                        field: "newsNotificationStatus",
                        title: "Status",
                        template: "{{getStatusText(this.dataItem)}}",
                        width: "150px;"
                    }, {
                        title: " ",
                        template: "<button type='button' ng-click='deleteNotification(this.dataItem)' class='btn btn-danger'>Delete</button>",
                        width: "100px"
                    }
                ],
                scrollable: false,
                pageable: {
                    refresh: true,
                    pageSizes: ['25', '50', '75', 'all']
                },
                selectable: true,
                change: function (e) {
                    var rowData = e.sender.dataItem(e.sender.select());
                    loadModal(rowData.id);
                }
            };
        }
        $scope.deleteNotification = function (kendoData) {
            swal({
                title: "Are you sure?",
                text: "Are you sure you want to delete the notification titled '" + kendoData.title + "'?",
                type: "question",
                preConfirm: function () {
                    return $http.delete('/api/NewsNotifications/' + kendoData.id);
                },
                confirmButtonColor: "#d9534f",
                showCancelButton: true,
                cancelButtonText: "Cancel",
                confirmButtonText: "Delete",
                showLoaderOnConfirm: true
            }).then(function (promise) {
                swal("Success!", "Notification deleted successfully", "success");
                $scope.sentOptions.dataSource.read();
                $scope.upcomingOptions.dataSource.read();
            }, function (canceled) {
            });
        };
        function loadModal(id) {
            var instance = $uibModal.open({
                templateUrl: '/NewsNotifications/AddNewModal',
                controller: "NewsNotificationModalController",
                backdrop: 'static',
                windowTopClass: "mt-30",
                resolve: {
                    id: function () {
                        return id;
                    }
                },
                size: 'lg'
            });
            instance.result.then(function (closeResult) {
                swal({
                    title: "Success!",
                    text: "Notification Saved",
                    type: "success"
                });
                $scope.sentOptions.dataSource.read();
                $scope.upcomingOptions.dataSource.read();
            }, function (dissmissResult) {
            });
        }
        $scope.addNew = function () {
            var instance = $uibModal.open({
                templateUrl: '/NewsNotifications/AddNewModal',
                controller: "NewsNotificationModalController",
                backdrop: 'static',
                windowTopClass: "mt-30",
                resolve: {
                    id: function () {
                        return ("");
                    }
                }
            });
            instance.result.then(function (promise) {
                swal({
                    title: "Success!",
                    text: "Notification Saved",
                    type: "success"
                });
                $scope.sentOptions.dataSource.read();
                $scope.upcomingOptions.dataSource.read();
            }, function (dismissal) {
            });
        };
        setupControls();
    }
})();
//# sourceMappingURL=App.NewsNotifications.Controller.js.map