using System.Data.Entity;

namespace BlogsConsole.Models
{
    public class BloggingContext : DbContext
    {
        public BloggingContext() : base("name=BlogContext") { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public void AddBlog(Blog blog)
        {
            this.Blogs.Add(blog);
            this.SaveChanges();
        }

        public void AddPost(Post post)
        {
            this.Posts.Add(post);
            this.SaveChanges();
        }

        public void DeletePost(Post post)
        {
            this.Posts.Remove(post);
            this.SaveChanges();
        }

        public void EditPost(Post UpdatedPost)
        {
            Post post = this.Posts.Find(UpdatedPost.PostId);
            post.Title = UpdatedPost.Title;
            post.Content = UpdatedPost.Content;
            this.SaveChanges();
        }
    }
}
