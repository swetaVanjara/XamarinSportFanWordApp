(() => {
    'use strict';

    angular.module("FanwordApp").controller("AdvertiserController", advertiserController);

    advertiserController.$inject = ['$scope', '$http', '$state', '$timeout'];

    function advertiserController($scope: any, $http: ng.IHttpService, $state: any, $timeout: any) {
        resetLoading();
        $scope.logoImageSource = "https://fanword.blob.core.windows.net/appimages/placeholder.png";
        $scope.searchThings = {
            searchText: "",
        };

        function loadControllerData() {
            if ($state.current.name == "viewAdvertiser") {
                $scope.searchText = "";
                $http.get('/api/Advertisers/' + $state.params.id).then((promise: any) => {
                    $scope.advertiser = promise.data as Advertiser;
                    $scope.logoImageSource = $scope.advertiser.logoUrl;
                });
            }
        }

        function setupControls() {

            $scope.logoFileUpload = {
                multiple: false,
                async: {
                    saveUrl: "/Uploads/UploadAdvertiserLogo",
                    autoUpload: false,
                },
                complete: (e: any) => {
                    //clear files
                    $timeout(() => {
                        $('#logoFile').data('kendoUpload').clearAllFiles();
                        //save advertiser
                        saveAdvertiser();
                    });
                },
                success: (e: any) => {
                    $scope.$apply(() => {
                        $scope.advertiser.logoUrl = e.response.Url;
                        $scope.advertiser.logoBlob = e.response.Blob;
                        $scope.advertiser.logoContainer = e.response.Container;
                    });

                },
                select: (e: any) => {
                    var fileReader = new FileReader();
                    fileReader.onload = (ev: any) => {
                        $timeout(() => {
                            var mapImage = ev.target.result;
                            $scope.logoImageSource = mapImage;
                        });
                    }
                    fileReader.readAsDataURL(e.files[0].rawFile);
                }
            }

            $scope.campaignOptions= {
                dataSource:new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Campaigns/AdminGrid",
                            data:() => {
                                return {
                                    advertiserId: $state.params.id
                                }
                            }
                        }
                    },
                    schema: {
                        model: {
                            fields: {
                                startUtc: { type: "date" },
                                endUtc: { type: "date" }
                            }
                        }
                    },
                    pageSize:25,
                }),
                toolbar: [
                    {
                        template:
                            "<input type='text' class='k-textbox agilx-name-lg pull-left' ng-model='searchThings.searchText' ng-change='onSearchChange()' style='width:300px'  placeholder='Search by title or profile' />" +
                                "<select kendo-drop-down-list k-options='statusOptions' k-ng-model='searchThings.statusValue' style='margin-left:15px; width:250px;'></select>"
                    }],
                columns: [
                    {
                        field: "imageUrl",
                        title: " ",
                        template: "<img src='#=imageUrl#' alt='' style='width:50px; height:50px;' />",
                        width: "100px;",
                        attributes: {
                            style: "text-align:center;"
                        },
                        filterable: false,
                        groupable: false,
                        sortable: false
                    }, {
                        field: "title",
                        title: "Title",
                        width: "150px;"
                    }, {
                        field: "weight",
                        title: "Frequency",
                        template: "# if(weight == 1){#Normal#} if(weight == 2){#High#}if(weight == 4){#Highest#} #",
                        width: "150px;"
                    }, {
                        field: "startUtc",
                        title: "Start",
                        template: "#= kendo.toString(startUtc,'MM/dd/yyyy') #",
                        width: "150px;"
                    }, {
                        field: "endUtc",
                        title: "End",
                        template: "#= kendo.toString(endUtc,'MM/dd/yyyy') #",
                        width: "150px;"
                    }, {
                        field: "profiles",
                        title: "Profiles",
                        sortable: false,
                        filterable: false,
                        groupable: false
                    }, {
                        field: "campaignStatus",
                        title: "Status",
                        template: "{{getStatusText(this.dataItem)}}",
                        width: "150px;"
                    }],
                sortable: true,
                scrollable: false,
                selectable: true,
                pageable: {
                    pageSizes: ['25', '50', '75', 'all'],
                    refresh: true,
                },
                change: (e: any) => {
                    var rowData = e.sender.dataItem(e.sender.select());
                    $state.go("viewCampaign", { id: rowData.id, advertiserId: $state.params.id});
                }
            }

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
                        }]
                }),
                change: (e: any) => {
                    $scope.onSearchChange();
                },
                optionLabel: "--Select a Status--",
                dataTextField: "displayName",
                dataValueField: "id",
                valuePrimitive: true,
            }
        }

        $scope.getStatusText = (kendoData: any) => {
            switch (kendoData.campaignStatus) {
                case CampaignStatus.Approved:
                    return "Approved";
                case CampaignStatus.Denied:
                    return "Denied";
                case CampaignStatus.Pending:
                    return "Pending";
                default:
                    return "";
            }
        }


        $scope.onSearchChange = () => {
            var filters = [];
            if ($scope.searchThings.searchText.length == 0) {
                //clear filters

            } else {
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
                    field: "campaignStatus",
                    operator: "eq",
                    value: $scope.searchThings.statusValue,
                });
            }

            $scope.campaignOptions.dataSource.filter(filters);
        }

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

        function resetLoading() {
            $scope.isSaving = false;
            $scope.saveText = "Save";
        }

        $scope.openLogoUpload = () => {
            $('#logoFile').trigger('click');
        }

        $scope.saveAdvertiser = () => {
            $scope.isSaving = true;
            if ($('#logoFile').data('kendoUpload').getFiles().length == 0) {
                $scope.saveText = "Saving...";
                saveAdvertiser();
            } else {
                $scope.saveText = "Uploading Logo...";
                $('#logoFile').data('kendoUpload').upload();
            }
        }


        function saveAdvertiser() {
            $scope.modelState = {};
            $http.put('/api/Advertisers', $scope.advertiser).then((promsie: any) => {
                $state.go('root');
                swal("Saved.", $scope.advertiser.companyName + " was saved successfully", "success");
            }, (error: any) => {
                if (error && error.data && error.data.modelState) {
                    $scope.modelState = error.data.modelState;
                }
            });
        }



        loadControllerData();
        setupControls();
    }

})();