(() => {
    'use strict';

    angular.module('FanwordApp').controller("ManageEventsController", eventsController);

    eventsController.$inject = ['$scope', '$http', '$state', 'moment', '$timeout','$q'];

    function eventsController($scope: any, $http: ng.IHttpService, $state: any, moment: any, $timeout: any, $q:any) {
        $scope.isValidPin = false;
        $scope.error = false;
        $scope.pinModel = {};
        $scope.previousTeams = [];
        $scope.currentTimeZoneId = "";

        var start = new Date();
        start.setDate(start.getDate() - 60);      
        $scope.startDate = start;

        $http.get('/api/Timezones/CurrentTimeZoneId').then((promise: any) => {
            $scope.currentTimeZoneId = promise.data;
        });


        $scope.getSportDisplay = (sportId: any) => {
            if ($scope.allSports == undefined) return;
            return Enumerable.From($scope.allSports).Where((x: any) => { return x.id == sportId }).Select((x: any) => { return x.displayName }).First();
        }

        $scope.getTimezoneDisplay = (timezoneId: any) => {
            if ($scope.allTimezones == undefined) return;
            return Enumerable.From($scope.allTimezones).Where((x: any) => { return x.id == timezoneId }).Select((x: any) => { return x.displayName }).First();
        }

        function loadSelectLists() {
           
            var sportListPromise = $http.get('/api/ManageEvents/SelectSportControlList');
            var timeZoneListPromise = $http.get('/api/Timezones/SelectControlList');
            var teamListPromise = $http.get('/api/ManageEvents/SelectTeamControlList');            
            var promises = [sportListPromise, teamListPromise, timeZoneListPromise];
            $q.all(promises).then((values: any) => {
                $scope.allTimezones = values[2].data;
                $scope.allSports = values[0].data;
                $scope.allTeams = values[1].data;

                $scope.timezoneOptions = {
                    dataSource: new kendo.data.DataSource({
                        transport: {
                            read: (e: any) => {
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
                            read: (e: any) => {
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
                    change: () => {
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

                $scope.getTeamOptions = (event: any) => {
                    return {
                        dataSource: new kendo.data.DataSource({
                            transport: {
                                read: (e: any) => {
                                    var teams = Enumerable.From($scope.allTeams).Where((x: any) => { return x.sportId == event.sportId }).ToArray();
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
                    }
                }
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
            $http.post("/api/ManageEvents/GetManagementGridEvents", $scope.filters).then((promise: any) => {
                $scope.events = promise.data.events;
               
            }, (error: any) => {
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
                change: (e: any) => {
                    //$scope.onSearchChange();
                }
            }

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

            $scope.onSearchChange = (currentPage: any) => {
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
                $timeout(() => {
                    swal.close();
                }, 300);
                
            };

            $scope.addNewEvent = (event: any) => {
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


            $scope.save = () => {
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
                }).then(() => {
                    showSavingModal();
                    $http.post("/api/ManageEvents/SaveEvents", $scope.events).then((promise: any) => {
                        $timeout(() => {
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
                            $timeout(() => {
                                loadGrid();
                            });
                        });
                    }, (error: any) => {
                        $timeout(() => {
                            swal.close();
                        });
                        $scope.modelState = error.data.modelState;
                        $scope.error = true;
                    });
                }, (dismiss: any) => { });

            }

            $scope.submitPin = () => {
                $http.post('/api/ManageEvents/ValidatePin', $scope.pinModel).then((promise: any) => {
                    $scope.isValidPin = true;

                }, (error: any) => {
                    $scope.pinModelState = error.data.modelState;
                });
            }

            
            

            $scope.filterTeamOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: (e: any) => {
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
            }

            

            $scope.cloneEvent = (event: Fanword.Event) => {

                swal({
                    title: "Are you sure?",
                    text: "Are you sure you want to clone this Event?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Clone",
                    cancelButtonText: "Cancel",
                    confirmButtonColor: "#d9534f",
                }).then(() => {
                    $timeout(() => {
                        //create new instance of the event
                        var obj = JSON.stringify(event);
                        var newEvent = JSON.parse(obj);
                        newEvent.id = new Guid().newGuid().toString();
                        newEvent.dateOfEventInTimezone = event.dateOfEventInTimezone;
                        newEvent.eventTeams.forEach(x => x.id = "NEW" + new Guid().newGuid().toString());
                        $scope.events.unshift(newEvent);
                        $scope.$apply();
                    });
                }, (dismiss: any) => { });

            };

            $scope.editEvent = (event: Fanword.Event) => {
                var eventIndex = $scope.events.findIndex(x => x.id == event.id);

                if (eventIndex >= 0) {
                    $scope.events[eventIndex].editEvent = true;
                }

            };

            $scope.removeEvent = (event: Fanword.Event) => {
                swal({
                    title: "Are you sure?",
                    text: "Are you sure you want to remove this Event?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Remove",
                    cancelButtonText: "Cancel",
                    confirmButtonColor: "#d9534f",
                }).then(() => {
                    $timeout(() => {
                        var eventIndex = $scope.events.findIndex(x => x.id == event.id);

                        if (eventIndex >= 0) {
                            $scope.events[eventIndex].isDeleted = true;
                        }
                    });
                }, (dismiss: any) => { });

            };

            $scope.checkValue = (team: EventTeam) => {
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

            }

            $scope.showEventTeams = (event: Fanword.Event) => {
                var eventIndex = $scope.events.findIndex(x => x.id == event.id);

                if (eventIndex >= 0) {
                    $scope.events[eventIndex].showEventTeams = $scope.events[eventIndex].showEventTeams == false ? true : false;
                }
            }

            $scope.addTeamRow = (event: Fanword.Event) => {
                if ($scope.newEventTeam.teamId == undefined || $scope.newEventTeam.teamId == '') {
                    swal("Error", "Team is required to add.", "error");
                    return;
                }

                if (Enumerable.From(event.eventTeams)
                    .Any((x: EventTeam) => { return x.teamId == $scope.newEventTeam.teamId && !x.isDeleted })) {
                    swal("Error", "Team is already in the list.", "error");
                    return;
                }
                $scope.newEventTeam.dateCreatedUtc = new Date();

                var events = $scope.events;
                var eventIndex = $scope.events.findIndex(x => x.id == event.id);

                //var length = $scope.events[eventIndex].eventTeams.length;
                //if (length > 0 && $scope.events[eventIndex].eventTeams[length - 1].displayOrder != undefined) {
                //    $scope.newEventTeam.displayOrder = $scope.events[eventIndex].eventTeams[length-1].displayOrder + 1;
                //}

                $scope.events[eventIndex].eventTeams.push($scope.newEventTeam);

                $scope.newEventTeam = new EventTeam();
            };

            $scope.removeTeam = (event: Fanword.Event, eventTeam: EventTeam) => {
                swal({
                    title: "Are you sure?",
                    text: "Are you sure you want to remove this team?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Remove",
                    cancelButtonText: "Cancel",
                    confirmButtonColor: "#d9534f",
                }).then(() => {
                    $timeout(() => {
                        var eventIndex = $scope.events.findIndex(x => x.id == event.id);
                        var teamIndex = event.eventTeams.indexOf(eventTeam)                        

                        if (eventIndex >= 0) {
                            $scope.events[eventIndex].eventTeams[teamIndex].isDeleted = true;;
                        }
                    });
                    }, (dismiss: any) => { });
            };

            $scope.clear = () => {
                swal({
                    title: "Are you sure?",
                    text: "Are you sure you want to remove all events?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Remove",
                    cancelButtonText: "Cancel",
                    confirmButtonColor: "#d9534f",
                }).then(() => {
                    $timeout(() => {
                        initialize();
                    });
                }, (dismiss: any) => { });
            };

           
            $scope.setTicketLink = (event: Fanword.Event) => {
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
                            $http.post('/api/Events/ValidateTicketsLink', model).then((promise: any) => {
                                resolve()
                            }, (error: any) => {
                                reject('Invalid URL!')
                            });
                        })
                    }
                }).then(function (url) {
                    var eventIndex = $scope.events.findIndex(x => x.id == event.id);

                    if (eventIndex >= 0) {
                        $scope.events[eventIndex].purchaseTicketsUrl = url;
                    }
                }, function (dismiss) {
                });
            };

        }
    }
})();