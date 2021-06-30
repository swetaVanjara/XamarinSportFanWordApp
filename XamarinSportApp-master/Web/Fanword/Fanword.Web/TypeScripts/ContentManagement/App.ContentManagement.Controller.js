(function () {
    'use strict';
    angular.module('FanwordApp').controller('ContentManagementController', contentManagementController);
    contentManagementController.$inject = ['$scope', '$http', '$state'];
    function contentManagementController($scope, $http, $state) {
        $scope.searchText = "";
        function setupControls() {
            $scope.postOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Posts/Grid",
                        }
                    },
                    schema: {
                        model: {
                            fields: {
                                dateCreatedUtc: { type: "date" }
                            }
                        }
                    },
                    pageSize: 25,
                    sort: {
                        field: "dateCreatedUtc",
                        dir: "desc"
                    }
                }),
                toolbar: [
                    {
                        template: "<input type='text' class='k-textbox agilx-name-lg pull-left' ng-model='searchText' ng-change='onSearchChange()' style='width:300px'  placeholder='Search content or author' />" +
                            "<span style='margin-left:15px;'>Posted Between:</span><input type='date' kendo-date-picker k-ng-model='startDate' style='margin-left:5px;' k-options='dateOptions'/> - <input type='date' kendo-date-picker k-ng-model='endDate' k-options='dateOptions' />"
                    }
                ],
                columns: [{
                        title: "Preview",
                        template: "# if(postSource =='Image') { #" +
                            "<img src='#=contentSourceUrl#' alt='' style='height:150px; width:150px;'/>" +
                            "# } else if (postSource == 'Video') { #" +
                            "<video style='height:150px; width:150px;'>" +
                            "<source src='#=contentSourceUrl#' />" +
                            "</video>" +
                            "# } else if (postSource == 'Link' && contentSourceUrl != null) { #" +
                            "<img src='#=contentSourceUrl#' alt='' style='height:150px; width:150px;'/>" +
                            "# } #"
                    },
                    {
                        field: "content",
                        title: "Content",
                        sortable: false,
                    }, {
                        field: "createdByName",
                        title: "Original Author",
                        headerAttributes: {
                            style: "text-align:center"
                        },
                        attributes: {
                            style: "text-align:center"
                        },
                        width: "175px;"
                    }, {
                        field: "dateCreatedUtc",
                        title: "Posted On",
                        headerAttributes: {
                            style: "text-align:center;"
                        },
                        attributes: {
                            style: "text-align:center"
                        },
                        template: "#=kendo.toString(dateCreatedUtc,'MM/dd/yyyy h:mm tt') #",
                        width: "175px"
                    },
                    {
                        title: " ",
                        template: "<button type='button' class='btn btn-danger' ng-click='deletePost(this.dataItem)'><i class='fa fa-trash'></i></button>",
                        width: "75px"
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
                    $state.go('viewPost', { id: rowData.id });
                }
            };
        }
        $scope.dateOptions = {
            change: function (e) {
                $scope.onSearchChange();
            }
        };
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
                            field: "content",
                            operator: "contains",
                            value: $scope.searchText
                        },
                        {
                            field: "createdByName",
                            operator: "contains",
                            value: $scope.searchText
                        }
                    ]
                });
                //$scope.postOptions.dataSource.filter();
            }
            var dateFilter = {
                logic: "and",
                filters: [],
            };
            if ($scope.startDate != undefined) {
                var filter1 = {
                    field: "dateCreatedUtc",
                    operator: "gte",
                    value: new Date($scope.startDate)
                };
                dateFilter.filters.push(filter1);
            }
            if ($scope.endDate != undefined) {
                var endOfDate = new Date($scope.endDate);
                endOfDate.setHours(23, 59, 59);
                var filter2 = {
                    field: "dateCreatedUtc",
                    operator: "lte",
                    value: endOfDate,
                };
                dateFilter.filters.push(filter2);
            }
            if (dateFilter.filters.length > 0) {
                filters.push(dateFilter);
            }
            //this will clear if none since filters will be empty array
            $scope.postOptions.dataSource.filter(filters);
        };
        $scope.deletePost = function (kendoData) {
            swal({
                title: 'Are you sure?',
                text: "Are you sure you want to delete this post?",
                showCancelButton: true,
                confirmButtonText: 'Delete',
                confirmButtonColor: "#d9534f",
                showLoaderOnConfirm: true,
                type: "question",
                preConfirm: function () {
                    return $http.delete('/api/Posts/' + kendoData.id);
                },
                allowOutsideClick: false
            }).then(function () {
                $scope.postOptions.dataSource.read();
                swal({
                    type: 'success',
                    text: 'Post Removed Successfully',
                    title: "Success"
                });
            }, function (cancel) {
            });
        };
        setupControls();
    }
})();
//# sourceMappingURL=App.ContentManagement.Controller.js.map