(function () {
    'use strict';
    angular.module('FanwordApp').controller('ContentSourceAdminController', contentSourceAdminController);
    contentSourceAdminController.$inject = ['$scope', '$http', '$state'];
    function contentSourceAdminController($scope, $http, $state) {
        $scope.searchText = "";
        function setupControls() {
            $scope.contentSourceOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/ContentSources/Grid"
                        }
                    },
                    pageSize: 25,
                }),
                toolbar: [
                    {
                        template: "<input type='text' class='k-textbox agilx-name-lg pull-left' ng-model='searchText' ng-change='onSearchChange()' style='width:300px'  placeholder='Search name' />" +
                            "<div class='md-checkbox pull-right'><input type='checkbox' id= 'checkbox2' class='md-check' ng-click='onSearchChange()' ng-model='pendingOnly'><label for='checkbox2'><span class='inc'></span><span class='check' > </span><span class='box' > </span>Show Pending Only</label></div>" +
                            "<div style='margin-right: 5px'class='md-checkbox pull-right'><input type='checkbox' id= 'checkbox3' class='md-check' ng-click='onSearchChange()' ng-model='unapprovedOnly'><label for='checkbox3'><span class='inc'></span><span class='check'> </span><span class='box' > </span>Show Unapproved Only</label></div>"
                    }
                ],
                columns: [
                    {
                        field: "contentSourceName",
                        title: "Content Source Name"
                    }, {
                        title: "Rss Feeds",
                        template: "# if(numberPendingRssFeeds >0) {#" +
                            "#=numberPendingRssFeeds# pending" +
                            "# } #",
                        width: "175px"
                    }, {
                        title: "Approved?",
                        width: "175px",
                        template: "# if(isApproved == true){#" + "Approved" + "#} if(isApproved == false){#" + "Not Approved" + "#}#",
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
                    $state.go('viewContentSource', { id: rowData.id });
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
                            field: "contentSourceName",
                            operator: "contains",
                            value: $scope.searchText
                        }
                    ]
                });
                //$scope.postOptions.dataSource.filter();
            }
            if ($scope.pendingOnly) {
                filters.push({
                    field: "numberPendingRssFeeds",
                    operator: "gte",
                    value: 1,
                });
            }
            if ($scope.unapprovedOnly) {
                filters.push({
                    field: "isApproved",
                    operator: "eq",
                    value: false,
                });
            }
            //this will clear if none since filters will be empty array
            $scope.contentSourceOptions.dataSource.filter(filters);
        };
        setupControls();
    }
})();
//# sourceMappingURL=App.ContentSourceAdmin.Controller.js.map