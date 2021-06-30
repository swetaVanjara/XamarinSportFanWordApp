(() => {
    'use strict';

    angular.module("FanwordApp").controller('NewsNotificationModalController', newsNotificationModalController);

    newsNotificationModalController.$inject = ['$scope', '$http', '$uibModalInstance', 'id'];

    function newsNotificationModalController($scope: any,
        $http: ng.IHttpService,
        $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
        id: any) {

        function loadModalData() {
            
            if (id === null || id === "") {
                //do new stuff
                $scope.modalTitle = "New News Notification";
                $scope.notification = new NewsNotification();
                var today = new Date();
                today.setHours(0, 0, 0, 0);
                $scope.pushDateOptions = {
                    min: today,
                };
            } else {
                //load it
                $http.get('/api/NewsNotifications/' + id).then((promise: any) => {
                    $scope.notification = promise.data as NewsNotification;
                    var today = new Date($scope.notification.pushDateUtc);
                   // today.setHours(0, 0, 0, 0);
                    $scope.pushDateOptions = {
                        min: today,
                    };
                    if ($scope.notification.teamId != undefined) {
                        $scope.forType = "team";
                    }
                    if ($scope.notification.schoolId != undefined) {
                        $scope.forType = "school";
                    }
                    if ($scope.notification.sportId != undefined) {
                        $scope.forType = "sport";
                    }
                });
                $scope.modalTitle = "Edit News Notification";
            }
            $http.get('/api/Users/GetCurrent').then((promise: any) => {
                $scope.user = promise.data as User;
                $http.get('/api/UserAdmins/GetCurrentTeam/?id=' + $scope.user.id).then((promiseTeam: any) => {
                    $scope.teamAdmin = promiseTeam.data as TeamAdmin;
                    if ($scope.teamAdmin != undefined) {
                        $scope.notification.teamId = $scope.teamAdmin.teamId;
                    }
                });
                $http.get('/api/UserAdmins/GetCurrentSchool/?id=' + $scope.user.id).then((promiseSchool: any) => {
                    $scope.schoolAdmin = promiseSchool.data as SchoolAdmin;
                    if ($scope.schoolAdmin != undefined) {
                        $scope.notification.schoolId = $scope.schoolAdmin.schoolId;
                    }
                });
            });
            //console.log($scope.user);
            //console.log($scope.notification);
        }

        function setupControls() {
            $scope.forTypeOptions = {
                dataSource: new kendo.data.DataSource({
                    data: [{
                        displayName: "School",
                        id: 'school'
                    }, {
                        displayName: 'Sport',
                        id: 'sport'
                    }, {
                        displayName: 'Team',
                        id: 'team'
                    }
                    ]
                }),
                dataTextField: "displayName",
                dataValueField: "id",
                optionLabel:"--Select a Type--",
                valuePrimitive: true
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

            $scope.statusOptions = {
                dataSource: new kendo.data.DataSource({
                    data: [
                        {
                            displayName: "Pending",
                            id: NewsNotificationStatus.Pending
                        }, {
                            displayName: "Approved",
                            id: NewsNotificationStatus.Approved,
                        }, {
                            displayName: "Denied",
                            id: NewsNotificationStatus.Denied
                        }]
                }),
                dataTextField: "displayName",
                dataValueField: "id",
                valuePrimitive: true,
            }
            
           
            
        }

        $scope.dateHelp = () => {
            swal("Push Date", "Please be aware that all notifications need to be approved first before they are sent out.</br></br> Therefore, when creating time-sensitive notifications, please keep in mind that they will not be sent out until they are approved. (If we do not get to approve a notification before its scheduled date & time, it will still be delivered once it is approved).</br></br>We will do our best to check notifications in a timely manner (mostly between 8 am and 11 pm CDT), however, please be considerate of our approval process when planning to send notifications. Thank you!");
        }

        $scope.saveAdmin = () => {
            $scope.forError = "";
            $scope.modelState = [];
            switch ($scope.forType) {
                case "school":
                    $scope.notification.teamId = null;
                    $scope.notification.sportId = null;
                    break;
                case "team":
                    $scope.notification.schoolId = null;
                    $scope.notification.sportId = null;
                    break;
                case "sport":
                    $scope.notification.teamId = null;
                    $scope.notification.schoolId = null;
                    break;
                default:
                    $scope.forError = "Please select a value";
                    return;
            }
            return save();
        }
        $scope.saveAdminApprove = () => {
            return save();
        }

        $scope.save = () => {
            return save();
        }

        function save() {
            $scope.forError = "";
            $scope.modelState = [];
            if ($scope.notification.id != null) {
                return $http.put('/api/NewsNotifications', $scope.notification).then((promise: any) => {
                    $uibModalInstance.close();
                   }, (error: any) => {
                    if (error && error.data && error.data.modelState) {
                        $scope.modelState = error.data.modelState;
                    }
                });
            } else {
                return $http.post('/api/NewsNotifications', $scope.notification).then((promise: any) => {
                    $uibModalInstance.close();
                }, (error: any) => {
                    if (error && error.data && error.data.modelState) {
                        $scope.modelState = error.data.modelState;
                    }
                });
            }
        }
        $scope.cancel = () => {
            $uibModalInstance.dismiss();
        }

        loadModalData();
        setupControls();

}
    
})();