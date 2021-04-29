using System;

using System.ComponentModel.DataAnnotations;

namespace Simple_Youtube_CLI
{
    public abstract class LikeModel
    {
        [Key]
        public int likeId { get; set; }

        public int videoId { get; set; }

        public int likedBy { get; set; }
    }
}
