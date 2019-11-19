using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;
using System.Collections.Generic;

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
                String input;
                do
                {
                    Console.WriteLine("\n1) Display all blogs");
                    Console.WriteLine("2) Add blog");
                    Console.WriteLine("3) Create Post");
                    Console.WriteLine("4) Display Posts");
                    Console.WriteLine("enter anything else to exit");
                    input = Console.ReadLine();

                    var db = new BloggingContext();
                    var query = db.Blogs.OrderBy(b => b.Name);
                    switch (input)
                    {
                        case "1":
                            // Display all Blogs from the database
                            Console.WriteLine("\nAll blogs in the database:");
                            var counter = 0;
                            foreach (var item in query)
                            {
                                counter++;
                                Console.WriteLine(counter + ") " + item.Name);
                            }
                            Console.WriteLine(counter + " Blogs returned");
                            break;
                        case "2":
                            // Create and save a new Blog
                            Console.Write("\nEnter a name for a new Blog: ");
                            var name = Console.ReadLine();
                            // make sure string is not empty or null
                            if (!String.IsNullOrEmpty(name))
                            {
                                //check if blog already exists
                                if (checkBlog(name))
                                {
                                    var blog = new Blog { Name = name };
                                    db.AddBlog(blog);
                                    logger.Info("Blog added - {name}", name);
                                }
                                else
                                {
                                    Console.WriteLine("\nBlog already exists");
                                }
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Name is empty or null");
                                break;
                            }
                        case "3":
                            foreach (var item in query)
                            {
                                Console.WriteLine(item.Name);
                            }

                            Console.WriteLine("\nEnter the name of the blog you wish to post to");
                            String blogToPostTo = Console.ReadLine();

                            //check that blog exists
                            if (!checkBlog(blogToPostTo))
                            {
                                //get blog
                                var blogItem = db.Blogs.Where(b => b.Name.Equals(blogToPostTo));
                                Blog blog = blogItem.First();

                                //get post details
                                String postTitle = "";
                                while (String.IsNullOrEmpty(postTitle))
                                {
                                    Console.WriteLine("enter a post title");
                                    postTitle = Console.ReadLine();
                                }

                                String postContent = "";
                                while (String.IsNullOrEmpty(postContent))
                                {
                                    Console.WriteLine("Enter content for the post");
                                    postContent = Console.ReadLine();
                                }

                                //make post
                                var post = new Post { Title = postTitle, Content = postContent, BlogId = blog.BlogId };

                                //add post to dataBase
                                db.AddPost(post);
                            }
                            else
                            {
                                Console.WriteLine("Blog does not exist");
                            }
                            break;
                        case "4":
                            var choice = "";
                            Console.WriteLine("\n0) Display all blogs");
                            foreach (var item in query)
                            {
                                Console.WriteLine(item.Name);
                            }
                            Console.WriteLine("\nEnter the name of the blog you wish to see posts from or enter '0' to see all");
                            Console.WriteLine("enter anything else to exit");
                            choice = Console.ReadLine();
                            var postList = db.Posts.OrderBy(p => p.BlogId);
                            //check for input
                            if (!String.IsNullOrEmpty(choice))
                            {
                                //display all
                                if (choice == "0")
                                {
                                    //get number of posts returned
                                    var postCounter = 0;
                                    var blogCounter = 0;
                                    foreach (var blogID in postList.Distinct())
                                    {
                                        // increment blogCounter for each blog that has atleast 1 post
                                        blogCounter++;
                                    }
                                    foreach (var post in postList)
                                    {
                                        //increment postCounter for each post 
                                        postCounter++;
                                    }

                                    //print out the results
                                    Console.WriteLine(postCounter + " Post(s) returned from " + blogCounter + " Blog(s).");

                                    //now print out all posts and their details
                                    foreach (var post in postList)
                                    {
                                        Console.WriteLine("\nBlog: " + post.Blog.Name);
                                        Console.WriteLine("Title: " + post.Title);
                                        Console.WriteLine("Content: " + post.Content);
                                    }
                                }

                                //dispay selected blogs posts
                                else
                                {
                                    //get blog
                                    var blogItem = db.Blogs.Where(b => b.Name.Equals(choice));

                                    //post counter
                                    var postCounter = 0;
                                    foreach (var post in blogItem)
                                    {
                                        postCounter++;
                                    }

                                    foreach (var blog in blogItem)
                                    {
                                        Console.WriteLine(postCounter + " Posts returned from " + blog.Name);
                                        Console.WriteLine("\n" + blog.Name);
                                    }

                                    foreach (var post in postList.Where(p => p.Blog.Name.Equals(choice)))
                                    {
                                        Console.WriteLine(post.Title);
                                        Console.WriteLine(post.Content);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("You did not select a blog correctly");
                            }
                            break;
                    }

                } while (input == "1" || input == "2" || input == "3" || input == "4");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                //logger.Error(ex.InnerException.Message);
                Console.ReadLine();

            }

            logger.Info("Program ended");
        }
        public static Boolean checkBlog(String name)
        {
            var unique = true;
            var db = new BloggingContext();
            var query = db.Blogs.OrderBy(b => b.Name);

            foreach (var item in query)
            {
                if (name == item.Name)
                {
                    unique = false;
                }
            }
            return unique;
        }
    }
}