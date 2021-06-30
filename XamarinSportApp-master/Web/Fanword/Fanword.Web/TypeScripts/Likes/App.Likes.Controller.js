(function () {
    'use strict';
    angular.module("FanwordApp").controller("LikesController", likesController);
    likesController.$inject = ['$scope', '$http'];
    function likesController($scope, $http) {
        function setupControls() {
            $scope.likeOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Likes/Grid",
                        }
                    },
                    pageSize: 25,
                    schema: {
                        model: {
                            fields: {
                                dateCreatedUtc: { type: "date" }
                            }
                        }
                    }
                }),
                scrollable: false,
                pageable: {
                    refresh: true,
                    pageSizes: ['25', '50', '75', '100', 'all'],
                },
                columns: [
                    {
                        field: "likedItem",
                        title: "Liked Item"
                    }, {
                        field: "createdByName",
                        title: "Creator",
                        template: "<a href='/UserManagement\\#!/View/#=createdById#' target='_blank'>#=createdByName#</a>",
                    }, {
                        field: 'dateCreatedUtc',
                        title: "Created On",
                        //width: "180px",
                        template: "#=kendo.toString(dateCreatedUtc,'MM/dd/yyyy h:mm tt')#"
                    }, {
                        title: " ",
                        template: "<button type='button' class='btn btn-danger' ng-click='delete(this.dataItem)'><i class='fa fa-trash'></i></button>",
                        width: "100px",
                        attributes: {
                            style: "text-align:center;"
                        }
                    }
                ]
            };
        }
        $scope.delete = function (kendoDataItem) {
            swal({
                title: 'Are you sure?',
                text: "Are you sure you want to delete this Like? This cannot be undone.",
                showCancelButton: true,
                confirmButtonText: 'Delete',
                confirmButtonColor: "#d9534f",
                showLoaderOnConfirm: true,
                type: "question",
                preConfirm: function () {
                    return $http.delete('/api/Likes/' + kendoDataItem.id);
                },
                allowOutsideClick: false
            }).then(function () {
                $scope.likeOptions.dataSource.read();
                swal({
                    type: 'success',
                    text: 'Like Removed Successfully',
                    title: "Success"
                });
            }, function (cancel) {
            });
        };
        setupControls();
    }
})();
//# sourceMappingURL=App.Likes.Controller.js.map