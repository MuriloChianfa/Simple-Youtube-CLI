using System;
using System.Linq;

namespace Simple_Youtube_CLI
{
    public sealed class Like: LikeModel, IVote
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
                    db.Likes.Add(new Like()
                    {
                        videoId = _videoId,
                        likedBy = _accountId
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
                    Like like = db.Likes.Where(like => like.likedBy == _accountId && like.videoId == _videoId).First();

                    if (like != null)
                    {
                        db.Likes.Remove(like);
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
