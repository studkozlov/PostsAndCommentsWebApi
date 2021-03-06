﻿using System;
using System.Data.Entity;

namespace TestApp.DAL.Models
{
    public class TestAppEntityFrameworkContext : DbContext
    {
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        
        static TestAppEntityFrameworkContext()
        {
            Database.SetInitializer(new TestInitializer());
        }
        public TestAppEntityFrameworkContext()
        { }
        public TestAppEntityFrameworkContext(string connection) : base(connection) { }

        public virtual void SetModified(object element)
        {
            Entry(element).State = EntityState.Modified;
        }
        
        public class TestInitializer : DropCreateDatabaseAlways<TestAppEntityFrameworkContext>
        {
            protected override void Seed(TestAppEntityFrameworkContext context)
            {
                var post1 = new Post
                {
                    Title = "First",
                    Author = "Me",
                    Content = "Per aspera ad astra",
                    CreationDate = DateTime.Now
                };
                var post2 = new Post
                {
                    Title = "Second",
                    Author = "Me",
                    Content = "Some fascinating article",
                    CreationDate = DateTime.Now
                };
                context.Posts.Add(post1);
                context.Posts.Add(post2);
                var comment1 = new Comment
                {
                    User = "Friend",
                    Text = "That's awesome",
                    CreationDate = DateTime.Now,
                    Post = post1
                };
                var comment2 = new Comment
                {
                    User = "Friend",
                    Text = "I can't believe it",
                    CreationDate = DateTime.Now,
                    Post = post1
                };
                context.Comments.Add(comment1);
                context.Comments.Add(comment2);
            }
        }
    }
}
