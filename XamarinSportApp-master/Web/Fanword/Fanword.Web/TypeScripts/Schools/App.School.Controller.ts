(() => {
    'use strict';

    angular.module('FanwordApp').controller('SchoolController', schoolController);

    schoolController.$inject = ['$scope', '$http', '$state', '$timeout', '$uibModal'];

    function schoolController($scope: any, $http: ng.IHttpService, $state: any, $timeout: any, $uibModal: ng.ui.bootstrap.IModalService) {
        $scope.showInactive = false;
        function loadControllerData() {
            if ($state.current.name == "addNewSchool") {
                //default sport create
                $scope.school = new School();
            }
            if ($state.current.name == "viewSchool") {
                //get from database
                $http.get('/api/Schools/' + $state.params.id).then((promise: any) => {
                    $scope.school = promise.data as School;
                    $scope.profileImageSource = $scope.school.profilePublicUrl;
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
                            pageSize:25,
                            filter: {
                                field: "schoolId",
                                operator: "contains",
                                value: $scope.school.id
                            }
                        }),
                        toolbar:[
                        {
                           template:"<button type='button' class='btn btn-primary' ng-click='addTeams()'>Add Teams</button>" 
                        }],
                        columns: [
                            {
                                field: "sportName",
                                title: "Sport"
                            }, {
                                field: "name",
                                title: "Name",
                            }, {
                                title: " ",
                                width:"100px",
                                template:"<button type='button' class='btn btn-danger' ng-click='deleteTeam(this.dataItem)'>Delete</button>"
                            }],
                        scrollable: false,
                        selectable: true,
                        pageable: {
                            pageSizes: ['25', '50', '75', 'all'],
                            refresh:true,
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
            $scope.delete = () => {
                swal({
                    title: 'Are you sure?',
                    text: "Are you sure you want to delete '" + $scope.school.name + "'? This will also delete any Teams associated with this school.",
                    showCancelButton: true,
                    confirmButtonText: 'Delete',
                    confirmButtonColor: "#d9534f",
                    showLoaderOnConfirm: true,
                    type: "question",
                    preConfirm: function () {
                        return $http.delete('/api/Schools/' + $scope.school.id);
                    },
                    allowOutsideClick: false
                }).then(function () {
                    $state.go('root');
                    swal({
                        type: 'success',
                        text: 'School Removed Successfully',
                        title: "Success"
                    });
                }, (cancel: any) => {
                });
            };

            $scope.deleteTeam = (kendoData: any) => {
                swal({
                    title: 'Are you sure?',
                    text: "Are you sure you want to delete '" + kendoData.name + "'?",
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


            $scope.profileUploadOptions = {
                async: {
                    saveUrl: "/Uploads/SchoolProfilePhoto",
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
                        $scope.school.profilePublicUrl = file.Url;
                        $scope.school.profileBlob = file.Blob;
                        $scope.school.profileContainer = file.Container;
                    });

                },
                complete: (e: any) => {
                    $timeout(() => {
                        $('#files').data('kendoUpload').clearAllFiles();
                        saveSchoolObject();
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
                    saveSchoolObject();
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

        function saveSchoolObject() {
            if ($scope.school.id === "NEW") {
                $http.post('/api/Schools', $scope.school).then((promise: any) => {
                    //open modal to ask for teams
                    var instance = $uibModal.open({
                        templateUrl: "/Templates/Schools/SaveAndAddTeamsModal.html",
                        controller: "SaveAndAddTeamsModalController",
                        backdrop: 'static',
                        windowTopClass: "mt-30",
                        resolve: {
                            schoolId: () => {
                                return promise.data.id;
                            },
                            wasCreating:() => {
                                return true;
                            }
                        }
                    });

                    instance.result.then((success: any) => {
                        $state.go('root');
                        swal("Saved.", "School and Teams Added Successfully", "success");
                    }, (dismissal: any) => {
                        $state.go('root');
                        swal("Saved.", "School Added Successfully", "success");
                    });

                }, (error: any) => {
                    if (error && error.data && error.data.modelState) {
                        $scope.modelState = error.data.modelState;
                        resetLoading();
                    }
                });
            } else {
                $http.put('/api/Schools', $scope.school).then((promise: any) => {
                    $state.go('root');
                    swal("Saved.", "School Updated Successfully", "success");
                }, (error: any) => {
                    if (error && error.data && error.data.modelState) {
                        $scope.modelState = error.data.modelState;
                        resetLoading();
                    }
                });
            }
        }

        $scope.addTeams = () => {
            var instance = $uibModal.open({
                templateUrl: "/Templates/Schools/SaveAndAddTeamsModal.html",
                controller: "SaveAndAddTeamsModalController",
                backdrop: 'static',
                windowTopClass: "mt-30",
                resolve: {
                    schoolId: () => {
                        return $scope.school.id;
                    },
                    wasCreating:() => {
                        return false;
                    }
                }
            });

            instance.result.then((success: any) => {
                $scope.teamOptions.dataSource.read();
                swal("Saved.", "Teams Added Successfully", "success");
            }, (dismissal: any) => {
            });
        }


        loadControllerData();
        setupScopeFunctions();
    }
})();
