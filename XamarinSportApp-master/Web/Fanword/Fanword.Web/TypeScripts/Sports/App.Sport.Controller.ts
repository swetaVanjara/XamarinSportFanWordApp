declare var swal: any;
(() => {
    'use strict';

    angular.module('FanwordApp').controller('SportController', sportController);

    sportController.$inject = ['$scope', '$http', '$state', '$uibModal', '$timeout'];

    function sportController($scope: any,
        $http: ng.IHttpService,
        $state: any,
        $uibModal: ng.ui.bootstrap.IModalService,
        $timeout: any) {
        $scope.profileImageSource = "https://fanword.blob.core.windows.net/appimages/placeholder.png";
        $scope.showInactive = false;
        function loadControllerData() {
            if ($state.current.name == "addNewSport") {
                //default sport create
                $scope.sport = new Sport();
            }
            if ($state.current.name == "viewSport") {
                //get from database
                $http.get('/api/Sports/' + $state.params.id).then((promise: any) => {
                    $scope.sport = promise.data as Sport;
                    $scope.profileImageSource = $scope.sport.iconPublicUrl;

                    $scope.teamOptions = {
                        dataSource: new kendo.data.DataSource({
                            transport: {
                                read: {
                                    url: "/api/Teams/Grid",
                                    data: () => {
                                        return {
                                            showInactive: $scope.showInactive
                                        }
                                    }
                                }
                            },
                            pageSize: 25,
                            filter: {
                                field: "sportId",
                                operator: "contains",
                                value: $scope.sport.id
                            }
                        }),
                        columns: [
                        {
                            field: "sportName",
                            title: "Sport"
                        }, {
                            field: "name",
                            title: "Name",
                        }, {
                            title: " ",
                            width: "100px",
                            template: "<button type='button' class='btn btn-danger' ng-click='deleteTeam(this.dataItem)'>Delete</button>"
                        }],
                        scrollable: false,
                        selectable: true,
                        pageable: {
                            pageSizes: ['25', '50', '75', 'all'],
                            refresh: true,
                        },
                        change: (e: any) => {
                            var rowData = e.sender.dataItem(e.sender.select());
                            window.location.href = '/Teams#!/View/' + rowData.id;
                        }

                    }


                });
            }
            setupControls();
        }

        $scope.cancel = () => {
            $state.go('root');
        }

        function setupControls() {
           
            $scope.iconFileUploadOptions = {
                async: {
                    saveUrl: "/Uploads/UploadSportIcon",
                    autoUpload: false
                },
                multiple: false,
                select: (event: any) => {
                    var fileReader = new FileReader();
                    fileReader.onload = (ev: any) => {
                        $timeout(() => {
                            var mapImage = ev.target.result;
                            $scope.profileImageSource = mapImage;
                        });
                    }
                    fileReader.readAsDataURL(event.files[0].rawFile);
                },
                success: (e: any) => {
                    var file = e.response;
                    $scope.$apply(() => {
                        $scope.sport.iconPublicUrl = file.Url;
                        $scope.sport.iconBlobName = file.Blob;
                        $scope.sport.iconContainer = file.Container;
                    });

                },
                complete: (e: any) => {
                    $timeout(() => {
                        $('#files').data('kendoUpload').clearAllFiles();
                        saveSportObject();
                    });
                }
            };

            $scope.openUpload = () => {
                $('#files').trigger('click');
            };

            
        }

        function resetLoading() {
            $scope.saveText = "Save";
            $scope.isSaving = false;
        }

        function saveSportObject() {
            if ($scope.sport.id === "NEW") {
                $http.post('/api/Sports', $scope.sport).then((promise: any) => {
                    $state.go('root');
                    swal("Saved.", "Sport Added Successfully", "success");
                }, (error: any) => {
                    if (error && error.data && error.data.modelState) {
                        $scope.modelState = error.data.modelState;
                        resetLoading();
                    }
                });
            } else {
                $http.put('/api/Sports', $scope.sport).then((promise: any) => {
                    $state.go('root');
                    swal("Saved.", "Sport Updated Successfully", "success");
                }, (error: any) => {
                    if (error && error.data && error.data.modelState) {
                        $scope.modelState = error.data.modelState;
                        resetLoading();
                    }
                });
            }
        }

        function setupScopeFunctions() {
            $scope.modelState = {};
            resetLoading();
            $scope.save = () => {
                $scope.isSaving = true;
                if ($('#files').data('kendoUpload').getFiles().length == 0) {
                    $scope.saveText = "Saving...";
                    saveSportObject();
                } else {
                    $scope.saveText = "Uploading Photo...";
                    $('#files').data('kendoUpload').upload();
                }
            };



            $scope.deleteTeam = (kendoData: any) => {
                swal({
                    title: 'Are you sure?',
                    text: "Are you sure you want to delete '" + kendoData.name + "'? This will also delete any Teams associated with this sport.",
                    showCancelButton: true,
                    confirmButtonText: 'Delete',
                    confirmButtonColor: "#d9534f",
                    showLoaderOnConfirm: true,
                    type: "question",
                    preConfirm: function () {
                        return $http.delete('/api/Teams/' + kendoData.id);
                    },
                    allowOutsideClick: false
                }).then(function () {
                    $scope.teamOptions.dataSource.read();
                    swal({
                        type: 'success',
                        text: 'Team Removed Successfully',
                        title: "Success"
                    });
                }, (cancel: any) => {
                });
            };



            $scope.delete = () => {
                swal({
                    title: 'Are you sure?',
                    text: "Are you sure you want to delete '" + $scope.sport.name + "'?",
                    showCancelButton: true,
                    confirmButtonText: 'Delete',
                    confirmButtonColor: "#d9534f",
                    showLoaderOnConfirm: true,
                    type: "question",
                    preConfirm: function () {
                        return $http.delete('/api/Sports/' + $scope.sport.id);
                    },
                    allowOutsideClick: false
                }).then(function () {
                    $state.go('root');
                    swal({
                        type: 'success',
                        text: 'Sport Removed Successfully',
                        title: "Success"
                    });
                }, (cancel: any) => {
                });
            };
        }

        loadControllerData();
        setupScopeFunctions();
    }
})();