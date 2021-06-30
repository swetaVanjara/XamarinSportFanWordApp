(function () {
    'use strict';
    angular.module("FanwordApp").controller("CommentsController", commentsController);
    commentsController.$inject = ['$scope', '$http'];
    function commentsController($scope, $http) {
        function setupControls() {
            $scope.commentOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/Comments/Grid",
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
                        field: "content",
                        title: "Comment"
                    }, {
                        field: "postId",
                        title: "Commented Post",
                        template: "<a href='/ContentManagement\\#/View/#=postId#' target='_blank'>Edit Post</a>",
                        width: "100px"
                    }, {
                        field: "createdByName",
                        title: "Creator",
                        template: "<a href='/UserManagement\\#!/View/#=createdById#' target='_blank'>#=createdByName#</a>",
                        width: "200px"
                    }, {
                        field: 'dateCreatedUtc',
                        title: "Created On",
                        width: "180px",
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
                text: "Are you sure you want to delete this comment? This cannot be undone.",
                showCancelButton: true,
                confirmButtonText: 'Delete',
                confirmButtonColor: "#d9534f",
                showLoaderOnConfirm: true,
                type: "question",
                preConfirm: function () {
                    return $http.delete('/api/Comments/' + kendoDataItem.id);
                },
                allowOutsideClick: false
            }).then(function () {
                $scope.commentOptions.dataSource.read();
                swal({
                    type: 'success',
                    text: 'Comment Removed Successfully',
                    title: "Success"
                });
            }, function (cancel) {
            });
        };
        setupControls();
    }
})();
//# sourceMappingURL=App.Comments.Controller.js.map