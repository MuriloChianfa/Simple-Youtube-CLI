using System.ComponentModel.DataAnnotations;

namespace Simple_Youtube_CLI
{
    public abstract class VideoModel
    {
        [Key]
        public int videoId { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public Category category { get; set; }

        public int owner { get; set; }
    }
}
