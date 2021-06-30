(function () {
    'use strict';
    angular.module('FanwordApp').controller('TeamController', teamController);
    teamController.$inject = ['$scope', '$http', '$state', '$timeout'];
    function teamController($scope, $http, $state, $timeout) {
        function loadControllerData() {
            if ($state.current.name == "addNewTeam") {
                //default team create
                $scope.team = new Team();
            }
            if ($state.current.name == "viewTeam") {
                //get from database
                $http.get('/api/Teams/' + $state.params.id).then(function (promise) {
                    $scope.team = promise.data;
                    $scope.profileImageSource = $scope.team.profilePublicUrl;
                });
            }
            setupControls();
        }
        $scope.cancel = function () {
            $state.go('root');
        };
        function setupControls() {
            $scope.delete = function () {
                swal({
                    title: 'Are you sure?',
                    text: "Are you sure you want to delete '" + $scope.team.nickname + "'?",
                    showCancelButton: true,
                    confirmButtonText: 'Delete',
                    confirmButtonColor: "#d9534f",
                    showLoaderOnConfirm: true,
                    type: "question",
                    preConfirm: function () {
                        return $http.delete('/api/Teams/' + $scope.team.id);
                    },
                    allowOutsideClick: false
                }).then(function () {
                    $state.go('root');
                    swal({
                        type: 'success',
                        text: 'Team Removed Successfully',
                        title: "Success"
                    });
                }, function (cancel) {
                });
            };
            $scope.profileUploadOptions = {
                async: {
                    saveUrl: "/Uploads/TeamProfilePhoto",
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
                        $scope.team.profilePublicUrl = file.Url;
                        $scope.team.profileBlob = file.Blob;
                        $scope.team.profileContainer = file.Container;
                    });
                },
                complete: function (e) {
                    $timeout(function () {
                        $('#files').data('kendoUpload').clearAllFiles();
                        saveTeamObject();
                    });
                }
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
        }
        function setupScopeFunctions() {
            $scope.profileImageSource = "https://fanword.blob.core.windows.net/appimages/placeholder.png";
            $scope.modelState = {};
            resetLoading();
            $scope.save = function () {
                $scope.isSaving = true;
                if ($('#files').data('kendoUpload').getFiles().length == 0) {
                    $scope.saveText = "Saving...";
                    saveTeamObject();
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
        function saveTeamObject() {
            if ($scope.team.id === "NEW") {
                $http.post('/api/Teams', $scope.team).then(function (promise) {
                    $state.go('root');
                    swal("Saved.", "Team Added Successfully", "success");
                }, function (error) {
                    if (error && error.data && error.data.modelState) {
                        $scope.modelState = error.data.modelState;
                        resetLoading();
                    }
                });
            }
            else {
                $http.put('/api/Teams', $scope.team).then(function (promise) {
                    $state.go('root');
                    swal("Saved.", "Team Updated Successfully", "success");
                }, function (error) {
                    if (error && error.data && error.data.modelState) {
                        $scope.modelState = error.data.modelState;
                        resetLoading();
                    }
                });
            }
        }
        loadControllerData();
        setupScopeFunctions();
    }
})();
//# sourceMappingURL=App.Team.Controller.js.map