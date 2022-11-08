using System;
using System.Collections.Generic;

namespace BLL.DTO
{
    public class ComparedUserModel
    {
        public string ComparedUserId { get; set; }
        public double SimilarityScore { get; set; }        
    }
}
