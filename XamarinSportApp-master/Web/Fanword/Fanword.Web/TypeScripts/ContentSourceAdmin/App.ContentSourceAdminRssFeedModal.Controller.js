(function () {
    'use strict';
    angular.module("FanwordApp").controller("RssFeedModalController", rssFeedModalController);
    rssFeedModalController.$inject = ['$scope', '$http', '$uibModalInstance', 'feedId', 'isAdmin'];
    function rssFeedModalController($scope, $http, $uibModalInstance, feedId, isAdmin) {
        $scope.isAdmin = true;
        //$scope.newKeywords = [];
        if (feedId === "NEW") {
            //do new stuff
            $scope.modalTitle = "New Feed";
            $scope.feed = new Feed();
        }
        else {
            //load it
            $http.get('/api/RssFeeds/' + feedId).then(function (promise) {
                $scope.feed = promise.data;
                console.log($scope.feed);
                if ($scope.feed.teamId != undefined) {
                    $scope.forType = "team";
                }
                if ($scope.feed.schoolId != undefined) {
                    $scope.forType = "school";
                }
                if ($scope.feed.sportId != undefined) {
                    $scope.forType = "sport";
                }
            });
            $scope.modalTitle = "Edit Feed";
        }
        function setupControls() {
            $scope.forOptions = {
                dataSource: new kendo.data.DataSource({
                    data: [
                        {
                            displayName: "School",
                            id: "school"
                        }, {
                            displayName: "Team",
                            id: "team"
                        }, {
                            displayName: "Sport",
                            id: "sport"
                        }
                    ]
                }),
                dataTextField: "displayName",
                dataValueField: "id",
                valuePrimitive: true,
                optionLabel: "--Select a Type--",
                change: function () {
                }
            };
            $scope.teamOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Teams/SelectControlList"
                        }
                    }
                }),
                dataTextField: "displayName",
                dataValueField: "id",
                valuePrimitive: true,
                optionLabel: "--Select a Team--",
                filter: "contains"
            };
            $scope.schoolOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Schools/SelectControlList"
                        }
                    }
                }),
                dataTextField: "displayName",
                dataValueField: "id",
                valuePrimitive: true,
                optionLabel: "--Select a School--",
                filter: "contains"
            };
            $scope.sportOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Sports/SelectControlList"
                        }
                    }
                }),
                dataTextField: "displayName",
                dataValueField: "id",
                valuePrimitive: true,
                optionLabel: "--Select a Sport--",
                filter: "contains"
            };
            $scope.mappingOptionList = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/RssFeeds/MappingOptions",
                            data: function () {
                                return {
                                    url: $scope.feed.url,
                                };
                            }
                        }
                    }
                }),
                dataValueField: "name",
                dataTextField: "sample",
                valuePrimitive: true
            };
        }
        $scope.keywordTypeList = {
            dataSource: new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "/api/RssKeywordTypes/Grid",
                    }
                }
            }),
            dataValueField: "id",
            dataTextField: "name",
            index: 1
        };
        $scope.addKeyword = function () {
            if ($scope.feed.newType == null || $scope.feed.newKeyword == null) {
                swal({
                    title: "Error",
                    text: "Please include both a keyword and type"
                });
                return;
            }
            $scope.newKeyword = new RssKeyword("", $scope.feed.newKeyword, $scope.feed.newType, feedId, 1);
            $scope.feed.rssKeywords.push($scope.newKeyword);
            console.log($scope.feed.rssKeywords);
            //$scope.newKeywords.push($scope.newKeyword);
            //console.log($scope.newKeywords);
            //reset
            $scope.feed.newKeyword = "";
            $scope.feed.newType = null;
            //swal("Success!", "A new keyword has been added", "success");
        };
        $scope.deleteKeyword = function (rssKeyword) {
            console.log(rssKeyword);
            rssKeyword.isActive = false;
            $scope.feed.rssKeywords.splice($scope.feed.rssKeywords.indexOf(rssKeyword), 1);
        };
        //$scope.rssKeywordOptions = {
        //    dataSource: new kendo.data.DataSource({
        //        transport: {
        //            read: {
        //                url: "/api/RssKeywords/Grid/" + feedId,
        //                data: () => {
        //                    return {
        //                    }
        //                }
        //            }
        //        },
        //        pageSize: 5
        //    }),
        //    columns: [
        //        {
        //            field: "name",
        //            title: "Name"
        //        },
        //        {
        //            field: "type",
        //            title: "Type"
        //        }
        //    ],
        //    scrollable: true,
        //    pageable: {
        //        pageSizes: ['25', '50', '75', 'all'],
        //        refresh: true,
        //    }
        //};
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
                    }
                ]
            }),
            dataTextField: "displayName",
            dataValueField: "id",
            valuePrimitive: true,
        };
        $scope.syncNow = function () {
            return $http.get("/api/RssFeeds/" + $scope.feed.id + "/Sync").then(function (promise) {
                swal("Success!", "Syncing process started.  Posts from this feed will start appearing shortly.", "success");
            });
        };
        $scope.onFeedUrlChange = function () {
            $scope.mappingOptionList.dataSource.read();
        };
        //$scope.addNewKeywords = (keyword) => {
        //    $http.post("/api/RssKeywords/Add", keyword).then(((promise: any) => {
        //    }));
        //};
        $scope.deleteFeed = function () {
            var d = new Date();
            $scope.feed.dateDeletedUtc = d.toUTCString();
            $http.put('/api/RssFeeds', $scope.feed).then(function (promise) {
                $uibModalInstance.close();
            });
        };
        $scope.save = function () {
            //if ($scope.newKeywords.length != 0) {
            //    for (var i = 0; i < $scope.newKeywords.length; i++) {
            //        $scope.addNewKeywords($scope.newKeywords[i]);
            //    }
            //}
            if ($scope.feed.id === "NEW") {
                return $http.post('/api/RssFeeds', $scope.feed).then(function (promise) {
                    swal({
                        title: "Success!",
                        text: "Feed added successfully",
                        type: "success"
                    });
                    $uibModalInstance.close();
                }, function (error) {
                    if (error && error.data && error.data.modelState) {
                        $scope.modelState = error.data.modelState;
                    }
                });
            }
            return $http.put('/api/RssFeeds', $scope.feed).then(function (promise) {
                swal({
                    title: "Success!",
                    text: "Feed saved successfully",
                    type: "success"
                });
                $uibModalInstance.close();
            }, function (error) {
                if (error && error.data && error.data.modelState) {
                    $scope.modelState = error.data.modelState;
                }
            });
        };
        $scope.enableDisable = function () {
            var currentState = $scope.feed.isActive;
            $http.get('/api/RssFeeds/' + $scope.feed.id + "/EnableDisable").then(function (promise) {
                var string = "Feed successfully " + (currentState ? "disabled" : "enabled") + ".";
                swal("Success!", string, "success");
                $scope.contentSourceOptions.dataSource.read();
                $uibModalInstance.close();
            });
        };
        $scope.cancel = function () {
            $uibModalInstance.dismiss();
        };
        setupControls();
    }
})();
//# sourceMappingURL=App.ContentSourceAdminRssFeedModal.Controller.js.map