var EmpApp = angular.module('BlgApp', ['ngRoute', 'BlgControllers']);  
EmpApp.config(['$routeProvider', function($routeProvider)  
{  
    $routeProvider.when('/List',  
    {  
        templateUrl: '/Views/BlogPosts/List.html',  
        controller: 'BlogPostsController'  
    }).  
    when('/create',  
    {  
        templateUrl: '/Views/BlogPosts/Create.html',  
        controller: 'BlogPostsController'  
    }).  
    otherwise(  
    {  
        redirectTo: '/list'  
    });  
}]);  