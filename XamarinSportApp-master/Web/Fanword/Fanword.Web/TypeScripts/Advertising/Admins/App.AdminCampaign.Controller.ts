(() => {
    'use strict';

    angular.module('FanwordApp').controller('AdminCampaignController', adminCampaignController);

    adminCampaignController.$inject = ['$scope', '$http', '$state', '$timeout'];

    function adminCampaignController($scope: any, $http: ng.IHttpService, $state: any, $timeout: any) {
        $scope.admin = true;
        resetLoading();
        function setupControls() {

            var weights = [
                { text: "Normal", value: 1 },
                { text: "High", value: 2 },
                { text: "Highest", value: 4 }
            ];
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
                placeholder: "Schools...",
                filter: "contains",
                change: () => {
                    setSchoolBox();
                },
                dataBound: () => {
                    setSchoolBox();
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
                placeholder: "Teams...",
                filter: "contains",
                change: () => {
                    setTeamBox();
                },
                dataBound: () => {
                    setTeamBox();
                }
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
                placeholder: "Sports...",
                filter: "contains",
                change: () => {
                    setSportBox();
                },
                dataBound: () => {
                    setSportBox();
                }
            };

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
                dataTextField: "displayName",
                dataValueField: "id",
                valuePrimitive: true,
            }
            $scope.weightOptions = {
                dataTextField: "text",
                dataValueField: "value",
                dataSource: weights,
                placeholder: "--Choose Frequency--",
                valuePrimitive: true
            }

            $scope.imageOptions = {
                async: {
                    saveUrl: "/Uploads/CampaignImage",
                    autoUpload: false
                },
                multiple: false,
                select: (event: any) => {
                    var fileReader = new FileReader();
                    fileReader.onload = (ev: any) => {
                        $timeout(() => {
                            var mapImage = ev.target.result;
                            $scope.imageSourceUrl = mapImage;
                        });
                    }
                    fileReader.readAsDataURL(event.files[0].rawFile);
                },
                success: (e: any) => {
                    var file = e.response;
                    $scope.$apply(() => {
                        $scope.campaign.imageUrl = file.Url;
                        $scope.campaign.imageBlob = file.Blob;
                        $scope.campaign.imageContainer = file.Container;
                    });

                },
                complete: (e: any) => {
                    $timeout(() => {
                        $('#fileUploader').data('kendoUpload').clearAllFiles();
                        saveCampaign();
                    });
                }
            }
        };

        $scope.openFileUpload = () => {
            $("#fileUploader").trigger('click');
        }

        function saveCampaign() {
            $scope.isSaving = true;
            $scope.saveText = "Saving...";
            $http.put('/api/Campaigns/', $scope.campaign).then((promise: any) => {
                swal("Success!", "Campaign Updated", "success");
                $state.go('viewAdvertiser', { id: $state.params.advertiserId });
            }, (error: any) => {
                resetLoading();
                if (error && error.data && error.data.modelState) {
                    $scope.modelState = error.data.modelState;
                }
            });

        }

        $scope.save =() => {
            $scope.modelState = {};
            $scope.isSaving = true;
            resetLoading();
            if ($('#fileUploader').data('kendoUpload').getFiles().length > 0) {
                $scope.saveText = "Uploading Photo....";
                $('#fileUploader').data('kendoUpload').upload();
            } else {
                $scope.saveText = "Saving...";
                saveCampaign();
            }
        }


        function setCheckboxes() {
            setSchoolBox();
            setTeamBox();
            setSportBox();
        }

        function setSchoolBox() {
            if ($scope.campaign == undefined || $scope.schoolOptions == undefined) {
                $scope.schoolSelectAll = false;
                return;
            };
            var schoolData = Enumerable.From($scope.schoolOptions.dataSource.data());
            $scope.schoolSelectAll = schoolData.Count() == $scope.campaign.schoolIds.length;
        }
        function setTeamBox() {
            if ($scope.campaign == undefined || $scope.teamOptions == undefined) {
                $scope.teamSelectAll = false;
                return;
            };
            var schoolData = Enumerable.From($scope.teamOptions.dataSource.data());
            $scope.teamSelectAll = schoolData.Count() == $scope.campaign.teamIds.length;
        }
        function setSportBox() {
            if ($scope.campaign == undefined || $scope.sportOptions == undefined) {
                $scope.sportSelectAll = false;
                return;
            };
            var schoolData = Enumerable.From($scope.sportOptions.dataSource.data());
            $scope.sportSelectAll = schoolData.Count() == $scope.campaign.sportIds.length;
        }



        $scope.selectAllSchools = () => {
            if ($scope.schoolSelectAll) {
                $scope.campaign.schoolIds = Enumerable.From($scope.schoolOptions.dataSource.data())
                    .Select((x: any) => { return x.id })
                    .ToArray();
            } else {
                $scope.campaign.schoolIds = [];
            }
        }

        $scope.selectAllTeams = () => {
            if ($scope.teamSelectAll) {
                $scope.campaign.teamIds = Enumerable.From($scope.teamOptions.dataSource.data())
                    .Select((x: any) => { return x.id })
                    .ToArray();
            } else {
                $scope.campaign.teamIds = [];
            }

        }

        $scope.selectAllSports = () => {
            if ($scope.sportSelectAll) {
                $scope.campaign.sportIds = Enumerable.From($scope.sportOptions.dataSource.data())
                    .Select((x: any) => { return x.id })
                    .ToArray();
            } else {
                $scope.campaign.sportIds = [];
            }
        }


        function loadControllerData() {
            $scope.imageSourceUrl = "https://fanword.blob.core.windows.net/appimages/placeholder.png";
            $scope.campaign = new Campaign();
            resetLoading();
            if ($state.current.name == 'addNewCampaign') {
                //do nothign
                setCheckboxes();
            } else {
                $http.get('/api/Campaigns/' + $state.params.id).then((promise: any) => {
                    $scope.campaign = promise.data as Campaign;
                    $scope.imageSourceUrl = $scope.campaign.imageUrl;
                    setCheckboxes();
                });
            }
        }

        $scope.getCampaignStatus = () => {
            if ($scope.campaign == undefined) return "";
            switch ($scope.campaign.campaignStatus) {
                case CampaignStatus.Approved:
                    return "Approved";
                case CampaignStatus.Denied:
                    return "Denied";
                case CampaignStatus.Pending:
                    return "Pending";
            }
        }

        //$scope.approve = () => {
        //    return $http.get('/api/Campaigns/' + $scope.campaign.id + "/Approve").then((promise: any) => {
        //        swal("Success", "Campaign Approved Successfully", 'success');
        //        $state.go('viewAdvertiser', { id: $state.params.advertiserId });
        //    });
        //}

        //$scope.deny=() => {
        //    return $http.get('/api/Campaigns/' + $scope.campaign.id + "/Deny").then((promise: any) => {
        //        swal("Success", "Campaign Denied Successfully", 'success');
        //        $state.go('viewAdvertiser', { id: $state.params.advertiserId });
        //    });
        //}


        function resetLoading() {
            $scope.isSaving = false;
            $scope.saveText = "Save";
        }

        $scope.delete = () => {
            swal({
                title: 'Are you sure?',
                text: "Are you sure you want to delete '" + $scope.campaign.name + "'?",
                showCancelButton: true,
                confirmButtonText: 'Delete',
                confirmButtonColor: "#d9534f",
                showLoaderOnConfirm: true,
                type: "question",
                preConfirm: function () {
                    return $http.delete('/api/Campaigns/' + $scope.campaign.id);
                },
                allowOutsideClick: false
            }).then(function () {
                $scope.teamOptions.dataSource.read();
                swal({
                    type: 'success',
                    text: 'Campaign Removed Successfully',
                    title: "Success"
                });
            }, (cancel: any) => {
            });
        }
        $scope.frequencyHelp = () => {
            swal("Frequency Help", "The frequency determines how often your selected audience is exposed to your ad. <br/><br/> <strong> Normal: </strong> Your ad is displayed as often as every other ad with 'normal' frequency. <br/> " +
                "<strong>High: </strong> Your ad is displayed twice as often as every other ad with 'normal' frequency.<br/>" +
                "<strong>Highest: </strong> Your ad is displayed three times as often as every other ad with 'normal' frequency.");
        }
        loadControllerData();
        setupControls();
    }
})();