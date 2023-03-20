using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public Comment ReplyToComment { get; set; }
        public int? ReplyToCommentId { get; set; }
        public ICollection<Comment> Replies { get; set; }
        public string NicknameToReply { get; set; }
        public Game Game { get; set; }
        public int GameId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
