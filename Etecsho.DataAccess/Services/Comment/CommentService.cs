using Etecsho.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Etecsho.DataAccess.Services.Comment
{
    public class CommentService : ICommentService
    {
        private EtecshoContext _context;
        public CommentService(EtecshoContext context)
        {
            _context = context;
        }
        public void AddComment(Models.Entites.Comment.Comment comment)
        {
            _context.Comment.Add(comment);
            _context.SaveChanges();

        }

        public List<Models.Entites.Comment.Comment> GetAllBlogsComments()
        {
            return _context.Comment.Where(p => p.ProductTypeId == 2)
                                .Include(p=>p.Users)
                                .Include(p=>p.Blog)
                                    .OrderByDescending(p=>p.CreateDate).ToList();
        }

        public Models.Entites.Comment.Comment GetCommentById(int id)
        {
            return _context.Comment.Where(p => p.CommentId == id)
                                .Include(p=>p.Blog)
                                .Include(p=>p.Video)
                                .Include(p=>p.Users)
                                .First();
                                
        }

        public Tuple<List<Models.Entites.Comment.Comment>, int> GetBlogComment(int BlogId, int pageId = 1)
        {
            int take = 10;
            int skip = (pageId - 1) * take;
            int pageCount = _context.Comment.Where(c => !c.IsDelete  && c.BlogId == BlogId).Count() / take;

            if ((pageCount % 2) != 0)
            {
                pageCount += 1;
            }

            return Tuple.Create(
                _context.Comment.Include(c => c.Users).Where(c => !c.IsDelete  && c.BlogId == BlogId).Skip(skip).Take(take)
                    .OrderByDescending(c => c.CreateDate).ToList(), pageCount);
        }

        public Tuple<List<Models.Entites.Comment.Comment>, int> GetVideoComment(int videoid, int pageId = 1)
        {
            int take = 10;
            int skip = (pageId - 1) * take;
            int pageCount = _context.Comment.Where(c => !c.IsDelete  && c.VideoId == videoid).Count() / take;

            if ((pageCount % 2) != 0)
            {
                pageCount += 1;
            }

            return Tuple.Create(
                _context.Comment.Include(c => c.Users).Where(c => !c.IsDelete && c.VideoId == videoid).Skip(skip).Take(take)
                    .OrderByDescending(c => c.CreateDate).ToList(), pageCount);
        }

        public void UpdateComment(Models.Entites.Comment.Comment comment)
        {
            _context.Comment.Update(comment);
            _context.SaveChanges();
        }

        public void DeleteComment(int id)
        {
            var comment = GetCommentById(id);
            comment.IsDelete = true;
            _context.Comment.Update(comment);
            _context.SaveChanges();
        }

        public List<Models.Entites.Comment.Comment> DeletedComments()
        {
            IQueryable< Models.Entites.Comment.Comment > result = _context.Comment.IgnoreQueryFilters()
                                                                    .Where(u => u.IsDelete && u.ProductTypeId == 2)
                                                                    .Include(p=>p.Users)
                                                                    .Include(p=>p.Blog);

            return result.ToList();
        }

        public List<Models.Entites.Comment.Comment> GetCommentByBlogId(int id)
        {
            var comments = _context.Comment.Where(p => p.BlogId == id)
                            .Include(p => p.Users)
                            .Include(p => p.Blog)
                            .OrderByDescending(p => p.CreateDate)
                            .ToList();
            return comments;
        }

        public List<Models.Entites.Comment.Comment> GetAllVideosComments()
        {
            return _context.Comment.Where(p => p.ProductTypeId == 3)
                            .Include(p => p.Users)
                            .Include(p => p.Video)
                                .OrderByDescending(p => p.CreateDate).ToList();
        }

        public List<Models.Entites.Comment.Comment> DeletedVideoComments()
        {
            IQueryable<Models.Entites.Comment.Comment> result = _context.Comment.IgnoreQueryFilters()
                                                                    .Where(u => u.IsDelete && u.ProductTypeId == 3)
                                                                    .Include(p => p.Users)
                                                                    .Include(p => p.Video);

            return result.ToList();
        }

        public List<Models.Entites.Comment.Comment> GetCommentByVideoId(int id)
        {
            var comments = _context.Comment.Where(p => p.VideoId == id)
                                      .Include(p => p.Users)
                                      .Include(p => p.Video)
                                      .OrderByDescending(p => p.CreateDate)
                                      .ToList();
            return comments;
        }
    }
}
