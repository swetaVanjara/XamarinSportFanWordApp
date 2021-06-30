(function () {
    'use strict';
    angular.module("FanwordApp").controller("ContentSourceAdminViewController", contentSourceAdminViewController);
    contentSourceAdminViewController.$inject = ['$scope', '$http', '$state', '$timeout', '$uibModal'];
    function contentSourceAdminViewController($scope, $http, $state, $timeout, $uibModal) {
        resetLoading();
        $scope.logoImageSource = "https://fanword.blob.core.windows.net/appimages/placeholder.png";
        $scope.searchThings = {
            searchText: "",
        };
        function loadControllerData() {
            if ($state.current.name == "viewContentSource") {
                $scope.searchText = "";
                $http.get('/api/ContentSources/GetById?id=' + $state.params.id).then(function (promise) {
                    $scope.contentSource = promise.data;
                    $scope.logoImageSource = $scope.contentSource.logoUrl;
                });
            }
        }
        function setupControls() {
            $scope.logoFileUpload = {
                multiple: false,
                async: {
                    saveUrl: "/Uploads/UploadContentSourceLogo",
                    autoUpload: false,
                },
                complete: function (e) {
                    //clear files
                    $timeout(function () {
                        $('#logoFile').data('kendoUpload').clearAllFiles();
                        //save advertiser
                        saveContentSource();
                    });
                },
                success: function (e) {
                    $scope.$apply(function () {
                        $scope.contentSource.logoUrl = e.response.Url;
                        $scope.contentSource.logoBlob = e.response.Blob;
                        $scope.contentSource.logoContainer = e.response.Container;
                    });
                },
                select: function (e) {
                    var fileReader = new FileReader();
                    fileReader.onload = function (ev) {
                        $timeout(function () {
                            var mapImage = ev.target.result;
                            $scope.logoImageSource = mapImage;
                        });
                    };
                    fileReader.readAsDataURL(e.files[0].rawFile);
                }
            };
            $scope.rssFeedOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/RssFeeds/SingleGrid/" + $state.params.id,
                        }
                    },
                    pageSize: 25,
                }),
                toolbar: [
                    {
                        template: "<input type='text' class='k-textbox agilx-name-lg pull-left' ng-model='searchThings.searchText' ng-change='onSearchChange()' style='width:300px'  placeholder='Search by title or profile' />" +
                            "<select kendo-drop-down-list k-options='statusOptions' k-ng-model='searchThings.statusValue' style='margin-left:15px; width:250px;'></select>"
                    }
                ],
                columns: [
                    {
                        field: "name",
                        title: "Name",
                        width: "150px;"
                    }, {
                        field: "rssFeedStatus",
                        title: "Status",
                        template: "{{getStatusText(this.dataItem)}}",
                        width: "150px;"
                    }
                ],
                sortable: true,
                scrollable: false,
                selectable: true,
                pageable: {
                    pageSizes: ['25', '50', '75', 'all'],
                    refresh: true,
                },
                change: function (e) {
                    var rowData = e.sender.dataItem(e.sender.select());
                    loadModal(rowData.id, true);
                }
            };
            $scope.statusOptions = {
                dataSource: new kendo.data.DataSource({
                    data: [
                        {
                            displayName: "Pending",
                            id: RssFeedStatus.Pending
                        }, {
                            displayName: "Approved",
                            id: RssFeedStatus.Approved,
                        }, {
                            displayName: "Denied",
                            id: RssFeedStatus.Denied
                        }
                    ]
                }),
                change: function (e) {
                    $scope.onSearchChange();
                },
                optionLabel: "--Select a Status--",
                dataTextField: "displayName",
                dataValueField: "id",
                valuePrimitive: true,
            };
        }
        $scope.getStatusText = function (kendoData) {
            switch (kendoData.rssFeedStatus) {
                case RssFeedStatus.Approved:
                    return "Approved";
                case RssFeedStatus.Denied:
                    return "Denied";
                case RssFeedStatus.Pending:
                    return "Pending";
                default:
                    return "";
            }
        };
        $scope.onSearchChange = function () {
            var filters = [];
            if ($scope.searchThings.searchText.length == 0) {
                //clear filters
            }
            else {
                //set filters
                filters.push({
                    logic: 'or',
                    filters: [{
                            field: "title",
                            operator: "contains",
                            value: $scope.searchThings.searchText
                        }, {
                            field: "profiles",
                            operator: "contains",
                            value: $scope.searchThings.searchText
                        }]
                });
            }
            console.log($scope.searchThings);
            if ($scope.searchThings.statusValue != undefined && $scope.searchThings.statusValue !== '') {
                filters.push({
                    field: "rssFeedStatus",
                    operator: "eq",
                    value: $scope.searchThings.statusValue,
                });
            }
            $scope.rssFeedOptions.dataSource.filter(filters);
        };
        //$scope.onSearchChange = () => {
        //    if ($scope.searchText.length == 0) {
        //        //clear filters
        //        $scope.campaignOptions.dataSource.filter([]);
        //    } else {
        //        //set filters
        //        $scope.campaignOptions.dataSource.filter([
        //            {
        //                field: "title",
        //                operator: "contains",
        //                value: $scope.searchText
        //            }
        //        ]);
        //    }
        //}
        function loadModal(feedId, isAdmin) {
            var instance = $uibModal.open({
                templateUrl: "/RssFeeds/FeedModal",
                controller: "RssFeedModalController",
                backdrop: 'static',
                windowTopClass: "mt-30",
                resolve: {
                    feedId: function () {
                        return feedId;
                    },
                    isAdmin: function () {
                        return isAdmin;
                    }
                },
                size: 'lg'
            });
            instance.result.then(function (closeResult) {
                $scope.rssFeedOptions.dataSource.read();
            }, function (dissmissResult) {
            });
        }
        function resetLoading() {
            $scope.isSaving = false;
            $scope.saveText = "Save";
        }
        $scope.openLogoUpload = function () {
            $('#logoFile').trigger('click');
        };
        $scope.saveContentSource = function () {
            $scope.isSaving = true;
            if ($('#logoFile').data('kendoUpload').getFiles().length == 0) {
                $scope.saveText = "Saving...";
                saveContentSource();
            }
            else {
                $scope.saveText = "Uploading Logo...";
                $('#logoFile').data('kendoUpload').upload();
            }
        };
        function saveContentSource() {
            $scope.modelState = {};
            $http.put('/api/ContentSources', $scope.contentSource).then(function (promise) {
                $state.go('root');
                swal("Saved.", $scope.contentSource.contentSourceName + " was saved successfully", "success");
            }, function (error) {
                if (error && error.data && error.data.modelState) {
                    $scope.modelState = error.data.modelState;
                }
            });
        }
        loadControllerData();
        setupControls();
    }
})();
//# sourceMappingURL=App.ContentSourceAdminView.Controller.js.map