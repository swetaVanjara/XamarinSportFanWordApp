(() => {
    'use strict';

    angular.module('FanwordApp').controller('ContentSourceController', contentSourceController);

    contentSourceController.$inject = ['$scope', '$http', '$state', '$uibModal', '$timeout'];

    function contentSourceController($scope: any, $http: ng.IHttpService, $state: any, $uibModal: ng.ui.bootstrap.IModalService, $timeout: any) {
        $scope.showInactive = false;
        function loadControllerData() {
            //Load data
            $http.get('/api/ContentSources/').then((promise: any) => {
                $scope.contentSource = promise.data as ContentSource;
                if ($scope.contentSource.logoUrl != null) {
                    $scope.profileImageSource = $scope.contentSource.logoUrl;
                }
            });
            setupControls();
        }
        function setupControls() {

            $scope.contentSourceUploadOptions = {
                async: {
                    saveUrl: "/Uploads/UploadContentSourceLogo",
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
                        $scope.contentSource.logoUrl = file.Url;
                        $scope.contentSource.logoBlob = file.Blob;
                        $scope.contentSource.logoContainer = file.Container;
                    });

                },
                complete: (e: any) => {
                    $timeout(() => {
                        $('#files').data('kendoUpload').clearAllFiles();
                        saveContentSource();
                    });
                }
            };

        }

        function setupScopeFunctions() {
            $scope.profileImageSource = "https://fanword.blob.core.windows.net/appimages/placeholder.png";
            $scope.modelState = {};
            resetLoading();
            $scope.save = () => {
                $scope.isSaving = true;
                if ($('#files').data('kendoUpload').getFiles().length == 0) {
                    $scope.saveText = "Saving...";
                    saveContentSource();
                } else {
                    $scope.saveText = "Uploading Photo...";
                    $('#files').data('kendoUpload').upload();
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

        function saveContentSource() {
            $http.put('/api/ContentSources', $scope.contentSource).then((promise: any) => {
                $state.go('root');
                swal("Saved.", "Content Source Updated Successfully", "success");
            }, (error: any) => {
                if (error && error.data && error.data.modelState) {
                    $scope.modelState = error.data.modelState;
                    resetLoading();
                }
            });
        }
    


        loadControllerData();
        setupScopeFunctions();
    }
})()