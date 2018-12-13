app.controller("BlogPostsController", ['$scope', '$http', '$location', '$routeParams', function ($scope, $http, $location, $routeParams) {
    //Get all employee and bind with html table
    $http.get("api/blogposts/GetBlogPosts").success(function (data) {
        $scope.ListOfBlogPost = data;

    })
        .error(function (data) {
            $scope.Status = "data not found";
        });
    //Add new blog post
    $scope.Add = function () {
        var blogpostData = {
            Title: $scope.Title,
            Content: $scope.Content,
            ImageUrl: $scope.ImageUrl,
            PublishToDate: null,
            CreatedDate: now(),
            ModifiedDate: now(),
            CreatedBy: "smh",
            ModifiedBy: "smh",
            Status: 0
           };
        debugger;
        $http.post("api/blogposts/AddBlogPost", blogpostData).success(function (data) {
            $location.path('/List');
        }).error(function (data) {
            console.log(data);
            $scope.error = "Something wrong when adding new employee " + data.ExceptionMessage;
        });
    }
}]);