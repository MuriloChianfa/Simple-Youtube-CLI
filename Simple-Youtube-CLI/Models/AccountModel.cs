using System;
using System.Collections.Generic;
using System.Linq;

using System.ComponentModel.DataAnnotations;

namespace Simple_Youtube_CLI
{
    public abstract class AccountModel
    {
        [Key]
        public int accountId { get; set; }

        public string username { get; set; }

        public string password { get; set; }

        public List<VideoModel> videos { get; set; } = new List<VideoModel>();

        [DataType(DataType.DateTime)]
        public DateTime createdAt { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime updatedAt { get; set; } = DateTime.Now;
    }
}
