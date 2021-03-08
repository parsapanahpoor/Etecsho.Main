using System;
using System.Collections.Generic;
using System.Text;

namespace Etecsho.DataAccess.Services.Comment
{
    public interface ICommentService
    {
        void AddComment(Etecsho.Models.Entites.Comment.Comment comment);

        Tuple<List<Models.Entites.Comment.Comment>, int> GetBlogComment(int BlogId, int pageId = 1);
        Tuple<List<Models.Entites.Comment.Comment>, int> GetVideoComment(int videoid, int pageId = 1);



        #region PanelAdmin

        List<Models.Entites.Comment.Comment> GetAllBlogsComments();
        List<Models.Entites.Comment.Comment> GetAllVideosComments();

        Models.Entites.Comment.Comment GetCommentById(int id);

        void UpdateComment(Models.Entites.Comment.Comment comment);

        void DeleteComment(int id);

        List<Models.Entites.Comment.Comment> DeletedComments();
        List<Models.Entites.Comment.Comment> DeletedVideoComments();

        List<Models.Entites.Comment.Comment> GetCommentByBlogId(int id);
        List<Models.Entites.Comment.Comment> GetCommentByVideoId(int id);

        #endregion
    }
}
