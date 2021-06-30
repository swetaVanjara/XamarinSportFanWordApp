using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Poco.Models;

namespace Fanword.Shared.Helpers
{
    public class CommentsHelper
    {
        public static List<Comment> GetCommentsWithReply(List<Comment> allComments, string expandedCommentId)
        {
            return allComments.Where(m => string.IsNullOrEmpty(m.ParentCommentId) || m.ParentCommentId == expandedCommentId).OrderBy(m =>
            {
                if (string.IsNullOrEmpty(m.ParentCommentId))
                {
                    return m.DateCreatedUtc;
                }
                else
                {
                    return allComments.FirstOrDefault(j => j.Id == m.ParentCommentId).DateCreatedUtc;
                }
            }).ThenBy(m => m.DateCreatedUtc).ToList();
        }
    }
}
