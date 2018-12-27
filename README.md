# BlogApplication
BlogApplication

Login (User, Editor and Admin)
POST: http://localhost:50602/api/v1/auth/user/login

Add Blog Post (Editor and Admin)
POST: http://localhost:50602/api/v1/blogposts/addBlogPost

Image Upload (Editor and Admin)
POST: http://localhost:50602/api/v1/imageupload/PostUserImage

Publish to Ready(Admin)
POST: http://localhost:50602/api/v1/blogposts/publictoready/1

Get All Blog Posts:
GET: http://localhost:50602/api/v1/blogposts/getBlogPosts

Get Blog Post By ID:
GET: http://localhost:50602/api/v1/blogposts/getBlogPostsById/1

Get All Users (Admin)
GET: http://localhost:50602/api/v1/user/getAllUsers

Get User BY Id
GET: http://localhost:50602/api/v1/user/getUser/u004

Create User (Admin)
POST: http://localhost:50602/api/v1/user/getUser/u001

Update User (Admin)
POST: http://localhost:50602/api/v1/user/updateUser
