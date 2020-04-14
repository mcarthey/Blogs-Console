using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {
                var db = new BloggingContext();

                // Ask user for new Blog name
                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();

                // save blog
                var newBlog = new Blog { Name = name };
                db.Blogs.Add(newBlog);
                db.SaveChanges();
                logger.Info("new blog saved");

                // Show all blogs
                var blogs = db.Blogs;
                Console.WriteLine("Here are all the blogs:");
                foreach (var item in blogs)
                {
                    Console.WriteLine(item.Name);
                }

                // Ask user to select blog
                Console.Write("Enter a Blog name: ");
                var userChoice = Console.ReadLine();
                var blog = blogs.FirstOrDefault(b => b.Name == userChoice);

                // Ask user to enter post details
                Console.WriteLine("Enter a Post");

                Console.WriteLine("Enter a title");
                var title = Console.ReadLine();

                Console.WriteLine("Enter some content");
                var content = Console.ReadLine();

                // save the post
                var newPost = new Post
                {
                    Title = title,
                    Content = content,
                    BlogId = blog.BlogId
                };

                db.Posts.Add(newPost);
                db.SaveChanges();
                logger.Info("new post saved");

                // display the posts
                Console.WriteLine("Here are the posts:");
                foreach (var item in db.Posts)
                {
                    Console.WriteLine($"title: {item.Title}; content: {item.Content}");
                }

                // ask the user to select a post
                Console.Write("Enter a Post name: ");
                var postChoice = Console.ReadLine();
                var post = db.Posts.FirstOrDefault(p => p.Title == postChoice);

                // edit the existing post
                Console.Write("Enter new post content: ");
                var newContent = Console.ReadLine();
                post.Title = newContent;

                // save the post
                db.SaveChanges();
                logger.Info("posted edited");

                // display the posts again
                Console.WriteLine("Here are the posts again:");
                foreach (var item in db.Posts)
                {
                    Console.WriteLine($"title: {item.Title}; content: {item.Content}");
                }

                // delete a post
                // ask the user to select a post
                Console.Write("Enter a Post name: ");
                var postToDelete = Console.ReadLine();
                var deletePost = db.Posts.FirstOrDefault(p => p.Title == postToDelete);

                db.Posts.Remove(deletePost);
                db.SaveChanges();
                logger.Info("posted deleted");

                // display the posts one last time
                Console.WriteLine("Here are the posts again:");
                foreach (var item in db.Posts)
                {
                    Console.WriteLine($"title: {item.Title}; content: {item.Content}");
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}
