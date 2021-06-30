(function () {
    'use strict';
    angular.module("FanwordApp").controller("PostController", postController);
    postController.$inject = ['$scope', '$http', '$state', 'moment'];
    function postController($scope, $http, $state, moment) {
        function setupControls() {
        }
        function loadControllerData() {
            if ($state.current.name == "viewPost") {
                $http.get("/api/Posts/" + encodeURI($state.params.id)).then(function (promise) {
                    $scope.post = promise.data;
                });
            }
        }
        $scope.getMomentDate = function () {
            if ($scope.post == undefined)
                return "";
            return moment($scope.post.dateCreatedUtc).format("MM/DD/YYYY h:mm a");
        };
        $scope.save = function () {
            return $http.put('/api/Posts', $scope.post).then(function (promise) {
                swal("Saved!", "Post Succesfully saved", "success");
                $state.go('root');
            });
        };
        $scope.showRemovalLink = function () {
            if ($scope.post == undefined)
                return false;
            return $scope.post.postSource != 'Unknown';
        };
        $scope.delete = function () {
            swal({
                title: 'Are you sure?',
                text: "Are you sure you want to delete this post?",
                showCancelButton: true,
                confirmButtonText: 'Delete',
                confirmButtonColor: "#d9534f",
                showLoaderOnConfirm: true,
                type: "question",
                preConfirm: function () {
                    return $http.delete('/api/Posts/' + $scope.post.id);
                },
                allowOutsideClick: false
            }).then(function () {
                $state.go('root');
                swal({
                    type: 'success',
                    text: 'Post Removed Successfully',
                    title: "Success"
                });
            }, function (cancel) {
            });
        };
        $scope.cancel = function () {
            $state.go('root');
        };
        loadControllerData();
        setupControls();
    }
})();
//# sourceMappingURL=App.Post.Controller.js.map