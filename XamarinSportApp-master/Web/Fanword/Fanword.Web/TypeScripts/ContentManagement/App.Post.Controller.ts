(() => {
    'use strict';

    angular.module("FanwordApp").controller("PostController", postController);

    postController.$inject = ['$scope', '$http', '$state','moment'];

    function postController($scope: any, $http: ng.IHttpService, $state: any,moment:any) {
        function setupControls() {

        }


        function loadControllerData() {
            if ($state.current.name == "viewPost") {
                $http.get("/api/Posts/" + encodeURI($state.params.id)).then((promise: any) => {
                    $scope.post = promise.data as Post;
                });
            }
        }

        $scope.getMomentDate = () => {
            if ($scope.post == undefined) return "";
            return moment($scope.post.dateCreatedUtc).format("MM/DD/YYYY h:mm a");
        }

        $scope.save = () => {
            return $http.put('/api/Posts',$scope.post).then((promise: any) => {
                swal("Saved!", "Post Succesfully saved", "success");
                $state.go('root');
            });
        }

        $scope.showRemovalLink =() => {
            if ($scope.post == undefined) return false;
            return $scope.post.postSource != 'Unknown';
        }

        $scope.delete =() => {
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
            }, (cancel: any) => {
            });
        }

        $scope.cancel = () => {
            $state.go('root');
        }

        loadControllerData();
        setupControls();
    }

})();