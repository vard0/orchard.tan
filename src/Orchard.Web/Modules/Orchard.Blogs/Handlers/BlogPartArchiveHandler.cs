using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Orchard.Blogs.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Common.Models;
using Orchard.Data;

namespace Orchard.Blogs.Handlers {
    [UsedImplicitly]
    public class BlogPartArchiveHandler : ContentHandler {
        public BlogPartArchiveHandler(IRepository<BlogPartArchiveRecord> blogArchiveRepository, IRepository<CommonPartRecord> commonRepository) {
            OnPublished<BlogPostPart>((context, bp) => RecalculateBlogArchive(blogArchiveRepository, commonRepository, bp));
            OnRemoved<BlogPostPart>((context, bp) => RecalculateBlogArchive(blogArchiveRepository, commonRepository, bp));
        }

        private static void RecalculateBlogArchive(IRepository<BlogPartArchiveRecord> blogArchiveRepository, IRepository<CommonPartRecord> commonRepository, BlogPostPart blogPostPart) {
            blogArchiveRepository.Flush();

            // remove all current blog archive records
            var blogArchiveRecords =
                from bar in blogArchiveRepository.Table
                where bar.BlogPart == blogPostPart.BlogPart.Record
                select bar;
            blogArchiveRecords.ToList().ForEach(blogArchiveRepository.Delete);

            // get all blog posts for the current blog
            var postsQuery =
                from bpr in commonRepository.Table
                where bpr.ContentItemRecord.ContentType.Name == "BlogPost" && bpr.Container.Id == blogPostPart.BlogPart.Record.Id
                orderby bpr.PublishedUtc
                select bpr;

            // create a dictionary of all the year/month combinations and their count of posts that are published in this blog
            var inMemoryBlogArchives = new Dictionary<DateTime, int>(postsQuery.Count());
            foreach (var post in postsQuery) {
                if (!post.PublishedUtc.HasValue)
                    continue;

                var key = new DateTime(post.PublishedUtc.Value.Year, post.PublishedUtc.Value.Month, 1);

                if (inMemoryBlogArchives.ContainsKey(key))
                    inMemoryBlogArchives[key]++;
                else
                    inMemoryBlogArchives[key] = 1;
            }

            // create the new blog archive records based on the in memory values
            foreach (KeyValuePair<DateTime, int> item in inMemoryBlogArchives) {
                blogArchiveRepository.Create(new BlogPartArchiveRecord {BlogPart = blogPostPart.BlogPart.Record, Year = item.Key.Year, Month = item.Key.Month, PostCount = item.Value});
            }
        }
    }
}