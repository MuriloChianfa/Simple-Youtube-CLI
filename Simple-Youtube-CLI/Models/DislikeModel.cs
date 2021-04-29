using System;

using System.ComponentModel.DataAnnotations;

namespace Simple_Youtube_CLI
{
    public abstract class DislikeModel
    {
        [Key]
        public int dislikeId { get; set; }

        public int videoId { get; set; }

        public int dislikedBy { get; set; }
    }
}
