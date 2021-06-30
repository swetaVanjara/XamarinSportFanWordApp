(() => {
    'use strict';

    angular.module('FanwordApp').controller("EventsController", eventsController);

    eventsController.$inject = ['$scope', '$http', '$state', '$timeout'];

    function eventsController($scope: any, $http: ng.IHttpService, $state: any, $timeout: any) {

        $scope.manageLink = location.protocol + '/Events/Manage#!/Index';
        $scope.manageLinkText = location.protocol + '//' + location.host + '/Events/Manage#!/Index';

        function getEventManagementPin() {
            $http.get("/api/Events/GetEventManagementPin").then((promise: any) => {
                $scope.pinModel = promise.data;
            });
        }

        getEventManagementPin();

        $scope.savePin = () => {
            $scope.modelState = {};
            $http.post('/api/Events/SaveEventManagementPin', $scope.pinModel).then((promise: any) => {
                $scope.pinModel = promise.data;
                swal({
                    title: "Success",
                    text: "Pin Save Successful",
                    confirmButtonText: "Ok",
                    confirmButtonColor: "#27ae60",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    showCancelButton: false,
                    type: "success"
                }).then(() => {
                }, (dismiss: any) => { });
            }, (error: any) => {
                $scope.modelState = error.data.modelState;
            });
        }

        $scope.generatePin = () => {
            var pin = "";
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz012345678901234567890123456789".split('');
            for (var i = 0; i < 12; i++) {
                pin += (chars[Math.floor(Math.random() * 82)]);
            }
            $scope.pinModel.pinNumber = pin;
        }

        $scope.searchText = "";
        function setupControls() {
            $scope.eventOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Events/Grid"
                        }
                    },
                    schema: {
                        model: {
                            fields: {
                                dateOfEventInTimezone: { type: "date" }
                            }
                        }
                    },
                    pageSize: 25,
                    sort: {
                        field: "dateOfEventInTimezone",
                        dir: "desc"
                    }
                }),
                toolbar: [
                    {
                        template:
                        "<input type='text' class='k-textbox agilx-name-lg pull-left' ng-model='searchText' ng-change='onSearchChange()' style='width:300px'  placeholder='Search by name, sport or team' />" +
                        "<span style='margin-left:15px;'>Between:</span><input type='date' kendo-date-time-picker k-ng-model='startDate' style='margin-left:5px;' k-options='dateOptions'/> - <input type='date' kendo-date-time-picker k-ng-model='endDate' k-options='dateOptions' />" +
                        "<button type='button' class='btn btn-primary pull-right' ng-click='addNew()'>Add New</button>"
                    }
                ],
                columns: [
                    {
                        title: "Date",
                        //template: "#= kendo.toString(dateOfEventUtc, 'M/d/yy') #",
                        template: "#= kendo.toString(dateOfEventInTimezone, 'M/d/yy') #",
                        width: "100px",
                        field: "dateOfEventInTimezone"
                    }, {
                        title: "Time",
                        //template: "#= kendo.toString(dateOfEventInTimezone,'t') #",
                        width: "100px",
                        field: "timeOfEventInTimezone",
                        sortable: false,
                        template: "# if (timeOfEventInTimezone == '12:00 AM') { #" +
                        "<span data-content=''>TBD</span>" +
                        "# } else{ #" +
                        "<span data-content=''> #:timeOfEventInTimezone# </span>" +
                        "# } #"
                    }, {
                        title: "Timezone",
                        field: "timezoneId"
                    }, {
                        title: "Name",
                        field: "name",
                    }, {
                        title: "Sport",
                        field: "sport",
                    }, {
                        title: "Teams",
                        field: "teams",
                    }, {
                        template: "<button type='button' class='btn btn-danger' ng-click='deleteEvent(this.dataItem)' style='margin-right: 15px'>Delete </button>" ,
                        //"<button type='button' class='btn btn-warning' ng-click='setTicketLink(this.dataItem)'><span class='fa fa-ticket'></span></button>"
                    }],
                scrollable: false,
                pageable: {
                    pageSizes: ['25', '50', '75', 'all'],
                    refresh: true,
                },
                sortable: true,
                selectable: true,
                change: (e: any) => {
                    var rowData = e.sender.dataItem(e.sender.select());
                    var url = $state.href('viewEvent', { id: rowData.id });
                    window.open(url, '_blank');
                }
            }
        }

        $scope.addNew = () => {
            $state.go('addNewEvent');
        };

        $scope.dateOptions = {
            change: (e: any) => {
                $scope.onSearchChange();
            }
        }

        $scope.onSearchChange = () => {

            var filters = [];
            if ($scope.searchText.length == 0) {
                //clear filters
            } else {
                //set filters

                filters.push({
                    logic: "or",
                    filters: [{
                        field: "name",
                        operator: "contains",
                        value: $scope.searchText
                    },
                    {
                        field: "sport",
                        operator: "contains",
                        value: $scope.searchText
                    },
                    {
                        field: "teams",
                        operator: "contains",
                        value: $scope.searchText
                    }]
                });
            }

            var dateFilter = {
                logic: "and",
                filters: [],
            };

            if ($scope.startDate != undefined) {
                var filter1 =
                    {
                        field: "dateOfEventUtc",
                        operator: "gte",
                        value: new Date($scope.startDate)
                    };
                dateFilter.filters.push(filter1);
            }

            if ($scope.endDate != undefined) {
                var endOfDate = new Date($scope.endDate);
                var filter2 =
                    {
                        field: "dateOfEventUtc",
                        operator: "lte",
                        value: endOfDate,
                    };
                dateFilter.filters.push(filter2);
            }

            if (dateFilter.filters.length > 0) {
                filters.push(dateFilter);
            }

            //this will clear if none since filters will be empty array
            $scope.eventOptions.dataSource.filter(filters);
        };
        $scope.deleteEvent = (dataItem: any) => {
            swal({
                title: 'Are you sure?',
                text: "Are you sure you want to delete '" + dataItem.name + "'?",
                showCancelButton: true,
                confirmButtonText: 'Delete',
                confirmButtonColor: "#d9534f",
                showLoaderOnConfirm: true,
                type: "question",
                preConfirm: function () {
                    return $http.delete('/api/Events/' + dataItem.id);
                },
                allowOutsideClick: false
            }).then(function () {
                $scope.eventOptions.dataSource.read();
                swal({
                    type: 'success',
                    text: 'Event Removed Successfully',
                    title: "Success"
                });
            }, (cancel: any) => {
            });
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
                    return new Promise(function (resolve, reject) {
                        if (value) {
                            resolve()
                        } else {
                            reject('Invalid URL!')
                        }
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

        setupControls();
    }
})();