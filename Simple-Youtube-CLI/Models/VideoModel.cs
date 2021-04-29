using System;

using System.ComponentModel.DataAnnotations;

namespace Simple_Youtube_CLI
{
    public abstract class VideoModel
    {
        [Key]
        public int videoId { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public int views { get; set; } = 0;

        public Category category { get; set; }

        public int owner { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime createdAt { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime updatedAt { get; set; } = DateTime.Now;
    }
}
