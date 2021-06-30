(function () {
    'use strict';
    angular.module('FanwordApp').controller("UserController", userController);
    userController.$inject = ['$scope', '$http', '$state', '$timeout', 'moment'];
    function userController($scope, $http, $state, $timeout, moment) {
        $scope.saveText = "Save";
        $scope.profileImageSource = "https://fanword.blob.core.windows.net/appimages/placeholder.png";
        function loadControllerData() {
            if ($state.current.name == "addNewUser") {
                //default user create
                $scope.user = new User();
            }
            if ($state.current.name == "viewUser") {
                //get from database
                $http.get('/api/Users/' + $state.params.id).then(function (promise) {
                    $scope.user = promise.data;
                    $scope.profileImageSource = $scope.user.profileUrl;
                });
            }
            setupControls();
        }
        $scope.cancel = function () {
            $state.go('root');
        };
        function setupControls() {
            $scope.profileUploadOptions = {
                async: {
                    saveUrl: "/Uploads/UserProfilePhoto",
                    autoUpload: false,
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
                        $scope.user.profileUrl = file.Url;
                        $scope.user.profileBlob = file.Blob;
                        $scope.user.profileContainer = file.Container;
                    });
                },
                complete: function (e) {
                    $timeout(function () {
                        $('#files').data('kendoUpload').clearAllFiles();
                        saveUserObject();
                    });
                }
            };
            $scope.contentSourceList = {
                dataTextField: "contentSourceName",
                dataValueField: "id",
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/contentSources/selectControlList"
                        }
                    }
                }), valuePrimitive: true
            };
            $scope.athleteYearOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Users/" + $state.params.id + "/AthleteYears"
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
                    pageSize: 25,
                }),
                columns: [
                    {
                        field: "startUtc",
                        title: "Start",
                        template: "#=kendo.toString(endUtc,'MM/dd/yyyy')#"
                    }, {
                        field: "endUtc",
                        title: "End",
                        template: "# if(endUtc != undefined) { #" +
                            "#=kendo.toString(endUtc,'MM/dd/yyyy')#" +
                            "# } else { #" +
                            "Present" +
                            "# } #"
                    }, {
                        field: "team",
                        title: "Team",
                    }, {
                        field: "verified",
                        title: "Verified",
                        template: "# if(verified) { #" +
                            "Yes" +
                            "# } else { #" +
                            "No" +
                            "# } #"
                    }, {
                        title: " ",
                        template: "# if(verified) { #" +
                            "<button type='button' ng-click='flipVerification(this.dataItem)' class='btn btn-warning'>Revoke</button>" +
                            "# } else { #" +
                            "<button type='button' ng-click='flipVerification(this.dataItem)' class='btn btn-success'>Verified</button>" +
                            "# } #",
                        width: "150px"
                    }
                ],
                scrollable: false,
            };
        }
        $scope.flipVerification = function (kendoData) {
            $http.get('/api/Users/FlipVerification/' + kendoData.id).then(function (promise) {
                $scope.athleteYearOptions.dataSource.read();
            });
        };
        function saveUserObject() {
            // $scope.user.ContentSourceId = $scope.user.contentSource.Id
            if ($scope.user.id === "NEW") {
                $http.post('/api/Users', $scope.user).then(function (promise) {
                    $state.go('root');
                    swal("Saved.", "User Added Successfully", "success");
                }, function (error) {
                    if (error && error.data && error.data.modelState) {
                        $scope.modelState = error.data.modelState;
                        resetLoading();
                    }
                });
            }
            else {
                $http.put('/api/Users', $scope.user).then(function (promise) {
                    $state.go('root');
                    swal("Saved.", "User Updated Successfully", "success");
                }, function (error) {
                    if (error && error.data && error.data.modelState) {
                        $scope.modelState = error.data.modelState;
                        resetLoading();
                    }
                });
            }
        }
        $scope.openUpload = function () {
            $('#files').trigger('click');
        };
        function resetLoading() {
            $scope.saveText = "Save";
            $scope.isSaving = false;
        }
        $scope.getDateAdded = function () {
            if ($scope.user == undefined)
                return "";
            return moment($scope.user.dateCreatedUtc).format('M/DD/YYYY h:mm a');
        };
        $scope.modelState = {};
        resetLoading();
        $scope.save = function () {
            $scope.isSaving = true;
            if ($('#files').data('kendoUpload').getFiles().length == 0) {
                $scope.saveText = "Saving...";
                saveUserObject();
            }
            else {
                $scope.saveText = "Uploading Photo...";
                $('#files').data('kendoUpload').upload();
            }
        };
        $scope.reinstate = function () {
            swal({
                title: 'Are you sure?',
                text: "Are you sure you want to reinstate '" + $scope.user.firstName + " " + $scope.user.lastName + "'?",
                showCancelButton: true,
                confirmButtonText: 'Reinstate',
                confirmButtonColor: "#ec971f",
                showLoaderOnConfirm: true,
                type: "question",
                preConfirm: function () {
                    return $http.get('/api/Users/Reinstate?id=' + $scope.user.id);
                },
                allowOutsideClick: false
            }).then(function () {
                swal({
                    type: 'success',
                    text: 'User Reinstated Successfully',
                    title: "Success"
                });
                $state.go("root");
            }, function (cancel) {
            });
        };
        $scope.delete = function () {
            swal({
                title: 'Are you sure?',
                text: "Are you sure you want to delete '" + $scope.user.email + "'?",
                showCancelButton: true,
                confirmButtonText: 'Delete',
                confirmButtonColor: "#d9534f",
                showLoaderOnConfirm: true,
                type: "question",
                preConfirm: function () {
                    return $http.delete('/api/Users/' + $scope.user.id);
                },
                allowOutsideClick: false
            }).then(function () {
                $state.go('root');
                swal({
                    type: 'success',
                    text: 'User Removed Successfully',
                    title: "Success"
                });
            }, function (cancel) {
            });
        };
        loadControllerData();
    }
})();
//# sourceMappingURL=App.User.Controller.js.map