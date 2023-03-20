using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy H-mm}")]
        public DateTime Date { get; set; }
        public CommentDTO ReplyToComment { get; set; }
        public int? ReplyToCommentId { get; set; }
        public ICollection<CommentDTO> Replies { get; set; }
        public string NicknameToReply { get; set; }
        public GameDTO Game { get; set; }
        public int GameId { get; set; }
        public ApplicationUserDTO ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
