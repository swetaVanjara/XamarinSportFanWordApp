(function () {
    'use strict';
    angular.module('FanwordApp').controller('ContentSourceController', contentSourceController);
    contentSourceController.$inject = ['$scope', '$http', '$state', '$uibModal', '$timeout'];
    function contentSourceController($scope, $http, $state, $uibModal, $timeout) {
        $scope.showInactive = false;
        function loadControllerData() {
            //Load data
            $http.get('/api/ContentSources/').then(function (promise) {
                $scope.contentSource = promise.data;
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
                select: function (event) {
                    var fileReader = new FileReader();
                    fileReader.onload = function (ev) {
                        $timeout(function () {
                            var mapImage = ev.target.result;
                            $scope.profileImageSource = mapImage;
                        });
                    };
                    fileReader.readAsDataURL(event.files[0].rawFile);
                },
                success: function (e) {
                    var file = e.response;
                    $scope.$apply(function () {
                        $scope.contentSource.logoUrl = file.Url;
                        $scope.contentSource.logoBlob = file.Blob;
                        $scope.contentSource.logoContainer = file.Container;
                    });
                },
                complete: function (e) {
                    $timeout(function () {
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
            $scope.save = function () {
                $scope.isSaving = true;
                if ($('#files').data('kendoUpload').getFiles().length == 0) {
                    $scope.saveText = "Saving...";
                    saveContentSource();
                }
                else {
                    $scope.saveText = "Uploading Photo...";
                    $('#files').data('kendoUpload').upload();
                }
            };
            $scope.openUpload = function () {
                $('#files').trigger('click');
            };
        }
        function resetLoading() {
            $scope.saveText = "Save";
            $scope.isSaving = false;
        }
        function saveContentSource() {
            $http.put('/api/ContentSources', $scope.contentSource).then(function (promise) {
                $state.go('root');
                swal("Saved.", "Content Source Updated Successfully", "success");
            }, function (error) {
                if (error && error.data && error.data.modelState) {
                    $scope.modelState = error.data.modelState;
                    resetLoading();
                }
            });
        }
        loadControllerData();
        setupScopeFunctions();
    }
})();
//# sourceMappingURL=App.ContentSource.Controller.js.map