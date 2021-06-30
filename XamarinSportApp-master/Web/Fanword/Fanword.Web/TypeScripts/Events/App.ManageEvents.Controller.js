(function () {
    'use strict';
    angular.module('FanwordApp').controller("ManageEventsController", eventsController);
    eventsController.$inject = ['$scope', '$http', '$state', 'moment', '$timeout', '$q'];
    function eventsController($scope, $http, $state, moment, $timeout, $q) {
        $scope.isValidPin = false;
        $scope.error = false;
        $scope.pinModel = {};
        $scope.previousTeams = [];
        $scope.currentTimeZoneId = "";
        var start = new Date();
        start.setDate(start.getDate() - 60);
        $scope.startDate = start;
        $http.get('/api/Timezones/CurrentTimeZoneId').then(function (promise) {
            $scope.currentTimeZoneId = promise.data;
        });
        $scope.getSportDisplay = function (sportId) {
            if ($scope.allSports == undefined)
                return;
            return Enumerable.From($scope.allSports).Where(function (x) { return x.id == sportId; }).Select(function (x) { return x.displayName; }).First();
        };
        $scope.getTimezoneDisplay = function (timezoneId) {
            if ($scope.allTimezones == undefined)
                return;
            return Enumerable.From($scope.allTimezones).Where(function (x) { return x.id == timezoneId; }).Select(function (x) { return x.displayName; }).First();
        };
        function loadSelectLists() {
            var sportListPromise = $http.get('/api/ManageEvents/SelectSportControlList');
            var timeZoneListPromise = $http.get('/api/Timezones/SelectControlList');
            var teamListPromise = $http.get('/api/ManageEvents/SelectTeamControlList');
            var promises = [sportListPromise, teamListPromise, timeZoneListPromise];
            $q.all(promises).then(function (values) {
                $scope.allTimezones = values[2].data;
                $scope.allSports = values[0].data;
                $scope.allTeams = values[1].data;
                $scope.timezoneOptions = {
                    dataSource: new kendo.data.DataSource({
                        transport: {
                            read: function (e) {
                                e.success($scope.allTimezones);
                            }
                        }
                    }),
                    dataTextField: "displayName",
                    dataValueField: "id",
                    valuePrimitive: true,
                    filter: 'contains',
                    optionLabel: "Select a Timezone",
                };
                $scope.sportOptions = {
                    dataSource: new kendo.data.DataSource({
                        transport: {
                            read: function (e) {
                                e.success($scope.allSports);
                            }
                        },
                        sort: {
                            field: "displayName",
                            dir: "asc"
                        }
                    }),
                    dataTextField: "displayName",
                    dataValueField: "id",
                    valuePrimitive: true,
                    filter: 'contains',
                    optionLabel: "Select a Sport",
                    change: function () {
                        $("#hostTeamDropdown").data("kendoDropDownList").dataSource.filter({
                            field: "sportId",
                            operator: "eq",
                            value: $scope.filters.sportId
                        });
                        $scope.filters.teamId = "";
                        setNewEventOptions();
                        $scope.$apply();
                    }
                };
                $scope.getTeamOptions = function (event) {
                    return {
                        dataSource: new kendo.data.DataSource({
                            transport: {
                                read: function (e) {
                                    var teams = Enumerable.From($scope.allTeams).Where(function (x) { return x.sportId == event.sportId; }).ToArray();
                                    e.success(teams);
                                }
                            },
                            sort: {
                                field: "displayName",
                                dir: "asc"
                            }
                        }),
                        dataTextField: "displayName",
                        dataValueField: "id",
                        valuePrimitive: true,
                        filter: 'contains',
                        optionLabel: "Select a Team",
                    };
                };
            });
        }
        function setNewEventOptions() {
            $scope.newEvent = new Fanword.Event();
            $scope.newEvent.id = new Guid().newGuid().toString();
            $scope.newEvent.sportId = $scope.filters.sportId;
            $scope.newEvent.timezoneId = $scope.currentTimeZoneId;
            $scope.newEventTeam = new EventTeam();
            $scope.newEventHostTeam = new EventTeam();
        }
        function loadGrid() {
            $http.post("/api/ManageEvents/GetManagementGridEvents", $scope.filters).then(function (promise) {
                $scope.events = promise.data.events;
            }, function (error) {
                $scope.modelState = error.data.modelState;
            });
        }
        function initialize() {
            $scope.events = [];
            $scope.filters = new EventFilter();
            loadSelectLists();
            setupControls();
            setNewEventOptions();
        }
        initialize();
        function setupControls() {
            $scope.dateOptions = {
                change: function (e) {
                    //$scope.onSearchChange();
                }
            };
            function showLoadingModal() {
                swal({
                    title: 'Loading Events',
                    html: "<div class='modal-body'>" +
                        "<span><i class='fa fa-refresh fa-spin'></i>&nbsp;Loading...</span>" +
                        "</div>",
                    showConfirmButton: false,
                });
            }
            function showSavingModal() {
                swal({
                    title: 'Saving Events',
                    html: "<div class='modal-body'>" +
                        "<span><i class='fa fa-refresh fa-spin'></i>&nbsp;Loading...</span>" +
                        "</div>",
                    showConfirmButton: false,
                });
            }
            $scope.onSearchChange = function (currentPage) {
                showLoadingModal();
                if ($scope.startDate != undefined) {
                    //$scope.filters.startDate = new Date($scope.startDate).toUTCString();
                    //$scope.filters.startDate = new Date($scope.startDate);
                    $scope.filters.startDate = $scope.startDate.toString();
                }
                if ($scope.endDate != undefined) {
                    //$scope.filters.endDate = new Date($scope.endDate).toUTCString();
                    //$scope.filters.endDate = new Date($scope.endDate);
                    $scope.filters.endDate = $scope.endDate.toString();
                }
                loadGrid();
                setNewEventOptions();
                $timeout(function () {
                    swal.close();
                }, 300);
            };
            $scope.addNewEvent = function (event) {
                if ($scope.newEvent.sportId == undefined || $scope.newEvent.sportId == '') {
                    swal("Error", "Sport is required to add an event.", "error");
                    return;
                }
                if ($scope.filters.teamId) {
                    $scope.newEventHostTeam.dateCreatedUtc = new Date();
                    $scope.newEventHostTeam.teamId = $scope.filters.teamId;
                    $scope.newEvent.eventTeams.push($scope.newEventHostTeam);
                }
                $scope.newEvent.sportDisplay = $scope.getSportDisplay($scope.newEvent.sportId);
                $scope.events.unshift($scope.newEvent);
                setNewEventOptions();
            };
            $scope.save = function () {
                $scope.modelState = {};
                $scope.error = false;
                $scope.events.forEach(function (event) {
                    event.stringConversionDate = moment(event.dateOfEventInTimezone).format("MM/DD/YYYY H:mm");
                    console.log(event.stringConversionDate);
                });
                swal({
                    title: "Are you sure?",
                    text: "Are you sure you want to save these events?",
                    type: "warning",
                    confirmButtonText: "Ok",
                    confirmButtonColor: "#27ae60",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    showCancelButton: true,
                }).then(function () {
                    showSavingModal();
                    $http.post("/api/ManageEvents/SaveEvents", $scope.events).then(function (promise) {
                        $timeout(function () {
                            swal.close();
                            swal({
                                title: "Success",
                                text: "Events Saved",
                                confirmButtonText: "Ok",
                                confirmButtonColor: "#27ae60",
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                showCancelButton: false,
                                type: "success"
                            });
                            $timeout(function () {
                                loadGrid();
                            });
                        });
                    }, function (error) {
                        $timeout(function () {
                            swal.close();
                        });
                        $scope.modelState = error.data.modelState;
                        $scope.error = true;
                    });
                }, function (dismiss) { });
            };
            $scope.submitPin = function () {
                $http.post('/api/ManageEvents/ValidatePin', $scope.pinModel).then(function (promise) {
                    $scope.isValidPin = true;
                }, function (error) {
                    $scope.pinModelState = error.data.modelState;
                });
            };
            $scope.filterTeamOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: function (e) {
                            e.success(Enumerable.From($scope.allTeams).ToArray());
                        },
                    },
                    sort: {
                        field: "displayName",
                        dir: "asc"
                    }
                }),
                dataTextField: "displayName",
                dataValueField: "id",
                valuePrimitive: true,
                filter: 'contains',
                optionLabel: "Select a Team",
            };
            $scope.cloneEvent = function (event) {
                swal({
                    title: "Are you sure?",
                    text: "Are you sure you want to clone this Event?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Clone",
                    cancelButtonText: "Cancel",
                    confirmButtonColor: "#d9534f",
                }).then(function () {
                    $timeout(function () {
                        //create new instance of the event
                        var obj = JSON.stringify(event);
                        var newEvent = JSON.parse(obj);
                        newEvent.id = new Guid().newGuid().toString();
                        newEvent.dateOfEventInTimezone = event.dateOfEventInTimezone;
                        newEvent.eventTeams.forEach(function (x) { return x.id = "NEW" + new Guid().newGuid().toString(); });
                        $scope.events.unshift(newEvent);
                        $scope.$apply();
                    });
                }, function (dismiss) { });
            };
            $scope.editEvent = function (event) {
                var eventIndex = $scope.events.findIndex(function (x) { return x.id == event.id; });
                if (eventIndex >= 0) {
                    $scope.events[eventIndex].editEvent = true;
                }
            };
            $scope.removeEvent = function (event) {
                swal({
                    title: "Are you sure?",
                    text: "Are you sure you want to remove this Event?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Remove",
                    cancelButtonText: "Cancel",
                    confirmButtonColor: "#d9534f",
                }).then(function () {
                    $timeout(function () {
                        var eventIndex = $scope.events.findIndex(function (x) { return x.id == event.id; });
                        if (eventIndex >= 0) {
                            $scope.events[eventIndex].isDeleted = true;
                        }
                    });
                }, function (dismiss) { });
            };
            $scope.checkValue = function (team) {
                if (team.winLossTie == $scope.previousValue && $scope.previousTeams.indexOf(team.teamId) != -1) {
                    $scope.previousValue = team.winLossTie;
                    team.winLossTie = null;
                    var index = $scope.previousTeams.indexOf(team.teamId);
                    $scope.previousTeams[index] = null;
                    return;
                }
                if ($scope.previousTeams.indexOf(team.teamId) != -1) {
                    var index = $scope.previousTeams.indexOf(team.teamId);
                    $scope.previousTeams[index] = null;
                }
                $scope.previousValue = team.winLossTie;
                $scope.previousTeams.push(team.teamId);
            };
            $scope.showEventTeams = function (event) {
                var eventIndex = $scope.events.findIndex(function (x) { return x.id == event.id; });
                if (eventIndex >= 0) {
                    $scope.events[eventIndex].showEventTeams = $scope.events[eventIndex].showEventTeams == false ? true : false;
                }
            };
            $scope.addTeamRow = function (event) {
                if ($scope.newEventTeam.teamId == undefined || $scope.newEventTeam.teamId == '') {
                    swal("Error", "Team is required to add.", "error");
                    return;
                }
                if (Enumerable.From(event.eventTeams)
                    .Any(function (x) { return x.teamId == $scope.newEventTeam.teamId && !x.isDeleted; })) {
                    swal("Error", "Team is already in the list.", "error");
                    return;
                }
                $scope.newEventTeam.dateCreatedUtc = new Date();
                var events = $scope.events;
                var eventIndex = $scope.events.findIndex(function (x) { return x.id == event.id; });
                //var length = $scope.events[eventIndex].eventTeams.length;
                //if (length > 0 && $scope.events[eventIndex].eventTeams[length - 1].displayOrder != undefined) {
                //    $scope.newEventTeam.displayOrder = $scope.events[eventIndex].eventTeams[length-1].displayOrder + 1;
                //}
                $scope.events[eventIndex].eventTeams.push($scope.newEventTeam);
                $scope.newEventTeam = new EventTeam();
            };
            $scope.removeTeam = function (event, eventTeam) {
                swal({
                    title: "Are you sure?",
                    text: "Are you sure you want to remove this team?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Remove",
                    cancelButtonText: "Cancel",
                    confirmButtonColor: "#d9534f",
                }).then(function () {
                    $timeout(function () {
                        var eventIndex = $scope.events.findIndex(function (x) { return x.id == event.id; });
                        var teamIndex = event.eventTeams.indexOf(eventTeam);
                        if (eventIndex >= 0) {
                            $scope.events[eventIndex].eventTeams[teamIndex].isDeleted = true;
                            ;
                        }
                    });
                }, function (dismiss) { });
            };
            $scope.clear = function () {
                swal({
                    title: "Are you sure?",
                    text: "Are you sure you want to remove all events?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Remove",
                    cancelButtonText: "Cancel",
                    confirmButtonColor: "#d9534f",
                }).then(function () {
                    $timeout(function () {
                        initialize();
                    });
                }, function (dismiss) { });
            };
            $scope.setTicketLink = function (event) {
                swal({
                    title: "Event Tickets!",
                    text: "Enter a link to purchase tickets for this event:",
                    input: 'text',
                    showCancelButton: true,
                    inputPlaceholder: "Enter Url",
                    inputValue: event.purchaseTicketsUrl,
                    inputValidator: function (value) {
                        if (value == "") {
                            value = null;
                        }
                        return new Promise(function (resolve, reject) {
                            var model = { purchaseTicketsurl: value };
                            $http.post('/api/Events/ValidateTicketsLink', model).then(function (promise) {
                                resolve();
                            }, function (error) {
                                reject('Invalid URL!');
                            });
                        });
                    }
                }).then(function (url) {
                    var eventIndex = $scope.events.findIndex(function (x) { return x.id == event.id; });
                    if (eventIndex >= 0) {
                        $scope.events[eventIndex].purchaseTicketsUrl = url;
                    }
                }, function (dismiss) {
                });
            };
        }
    }
})();
//# sourceMappingURL=App.ManageEvents.Controller.js.map