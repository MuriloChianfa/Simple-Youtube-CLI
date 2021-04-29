using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Youtube_CLI
{
    public static class Vote
    {
        public static bool VerifyIfAlreadyVote(int _accountId, int _videoId)
        {
            using (var db = new Database())
            {
                try
                {
                    if (db.Likes.Where(like => like.likedBy == _accountId && like.videoId == _videoId).First() != null)
                    {
                        return true;
                    }

                    if (db.Dislikes.Where(dislike => dislike.dislikedBy == _accountId && dislike.videoId == _videoId).First() != null)
                    {
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

        public static int WitchVote(int _accountId, int _videoId)
        {
            if (!VerifyIfAlreadyVote(_accountId, _videoId))
            {
                throw new Exception("Você precisa votar para trocar de voto!");
            }

            using (var db = new Database())
            {
                try
                {
                    if (db.Likes.Where(like => like.likedBy == _accountId && like.videoId == _videoId).First() != null)
                    {
                        return 1;
                    }

                    if (db.Dislikes.Where(dislike => dislike.dislikedBy == _accountId && dislike.videoId == _videoId).First() != null)
                    {
                        return 2;
                    }
                }
                catch (InvalidOperationException)
                {
                    // pass if not found
                }
            }

            return 3;
        }

        public static bool SwitchVote(int _accountId, int _videoId)
        {
            if (!VerifyIfAlreadyVote(_accountId, _videoId))
            {
                throw new Exception($"\"{_accountId}\" need one vote to switch there!");
            }

            using (var db = new Database())
            {
                try
                {
                    Like like = db.Likes.Where(like => like.likedBy == _accountId && like.videoId == _videoId).First();

                    if (like != null)
                    {
                        db.Likes.Remove(like);

                        db.Dislikes.Add(new Dislike()
                        {
                            videoId = _videoId,
                            dislikedBy = _accountId
                        });
                        db.SaveChanges();

                        return true;
                    }
                }
                catch (InvalidOperationException)
                {
                    Dislike dislike = db.Dislikes.Where(dislike => dislike.dislikedBy == _accountId && dislike.videoId == _videoId).First();

                    if (dislike != null)
                    {
                        db.Dislikes.Remove(dislike);

                        db.Likes.Add(new Like()
                        {
                            videoId = _videoId,
                            likedBy = _accountId
                        });
                        db.SaveChanges();

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
