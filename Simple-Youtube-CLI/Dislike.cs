using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Youtube_CLI
{
    public sealed class Dislike : DislikeModel, IVote
    {
        public static bool Vote(int _accountId, int _videoId)
        {
            if (Simple_Youtube_CLI.Vote.VerifyIfAlreadyVote(_accountId, _videoId))
            {
                throw new Exception("You already vote!");
            }

            using (var db = new Database())
            {
                try
                {
                    db.Dislikes.Add(new Dislike()
                    {
                        videoId = _videoId,
                        dislikedBy = _accountId
                    });
                    db.SaveChanges();

                    return true;
                }
                catch (InvalidOperationException)
                {
                    // pass if not found
                }
            }

            return false;
        }

        public static bool UnVote(int _accountId, int _videoId)
        {
            if (!Simple_Youtube_CLI.Vote.VerifyIfAlreadyVote(_accountId, _videoId))
            {
                throw new Exception("You need one vote to UnVote!");
            }

            using (var db = new Database())
            {
                try
                {
                    Dislike dislike = db.Dislikes.Where(dislike => dislike.dislikedBy == _accountId && dislike.videoId == _videoId).First();

                    if (dislike != null)
                    {
                        db.Dislikes.Remove(dislike);
                        db.SaveChanges();

                        return true;
                    }
                }
                catch (InvalidOperationException)
                {
                    // pass if not found
                }
            }

            return false;
        }
    }
}
