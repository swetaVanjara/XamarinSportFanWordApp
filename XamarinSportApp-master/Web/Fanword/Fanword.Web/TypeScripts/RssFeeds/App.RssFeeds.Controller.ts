(() => {
    'use strict';

    angular.module("FanwordApp").controller("RssFeedsController", rssFeedsController);

    rssFeedsController.$inject = ['$scope', '$http', '$uibModal', '$state', '$window'];

    function rssFeedsController($scope: any, $http: ng.IHttpService, $uibModal: ng.ui.bootstrap.IModalService, $state: any, $window: any) {


        $scope.showInactive = false;
        if ($window.location.search == '?true') {
            $scope.showPending = true;
        }
         else {
            $scope.showPending = false;
        }

        function setupControls() {

            $scope.feedOptions = {
                toolbar: [
                    {
                        template:
                            "<input type='text' class='k-textbox agilx-name-lg pull-left' ng-model='searchText' ng-change='onSearchChange()' style='width:300px'  placeholder='Search by name or team' />" +
                                "<button type='button' class='btn btn-primary pull-right' ng-click='addNew()'>Add New</button>"
                    }
                ],
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/RssFeeds/Grid",
                            data:() => {
                                return {
                                    showInactive: $scope.showInactive,
                                    showPending: $scope.showPending
                                }
                            }
                        }
                    },
                    pageSize:25
                }),
                columns: [
                    {
                        field: "rssFeedStatus",
                        title: "Status",
                        template: "# if (rssFeedStatus == 0) { #" +
                        "<span data-content=''>Pending</span>" +
                        "# } else if (rssFeedStatus == 1) { #" +
                        "<span data-content=''>Approved</span>" +
                        "# } else if (rssFeedStatus == 2) { #" +
                        "<span data-content=''>Denied</span>" + 
                        "# } #"

                    }, {
                        field: "name",
                        title: "Name"
                    }, {
                        field: "associatedSchoolOrTeam",
                        title: "Team/School/Sport",
                    }, {
                        field: "createdBy",
                        title: "Created By",
                        template: "# if (createdBy == null) { #" +
                        "<span data-content=''>Admin</span>" +
                        "# } else { #" +
                        "<span data-content=''>#:createdBy# </span>" +
                        "# } #"
                    }
                ],
                sortable: {
                    mode: "single",
                },
                scrollable: false,
                pageable: {
                    pageSizes: ['25', '50', '75', 'all'],
                    refresh: true,
                },
                selectable: true,
                change: (e: any) => {
                    var rowData = e.sender.dataItem(e.sender.select());
                    loadModal(rowData.id, false);
                }

            };
        }

        $scope.reloadGrid = () => {
            $scope.feedOptions.dataSource.read();
        }


        function loadModal(feedId: string, isAdmin: boolean) {
            var instance = $uibModal.open({
                templateUrl: "/RssFeeds/FeedModal",
                controller: "RssFeedModalController",
                backdrop: 'static',
                windowTopClass: "mt-30",
                resolve: {
                    feedId: () => {
                        return feedId;
                    },
                    isAdmin: () => {
                        return isAdmin;
                    }
                },
                size:'lg'
            });

            instance.result.then((closeResult: any) => {
                $scope.feedOptions.dataSource.read();
            },(dissmissResult: any) => {

            });
        }

        $scope.addNew = () => {
            loadModal("NEW", false);
        }

        $scope.onSearchChange = () => {
            if ($scope.searchText.length == 0) {
                //clear filters
                $scope.feedOptions.dataSource.filter([]);
            } else {
                //set filters
                $scope.feedOptions.dataSource.filter([
                    {
                        logic: "or",
                        filters:[
                            {
                                field:"name",
                                operator: "contains",
                                value: $scope.searchText
                            },
                            {
                                field: "associatedSchoolOrTeam",
                                operator: "contains",
                                value: $scope.searchText
                            }]
                    }
                ]);
            }
        }

        setupControls();

    }
})();