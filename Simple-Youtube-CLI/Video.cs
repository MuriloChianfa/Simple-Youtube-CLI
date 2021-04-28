using System;
using System.Linq;

namespace Simple_Youtube_CLI
{
    public sealed class Video: VideoModel
    {
        #region StaticMethods
        public static bool VerifyVideoById(int _videoId)
        {
            using (var db = new Database())
            {
                try
                {
                    if (db.Videos.Where(video => video.videoId == _videoId).First() != null)
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

        public static bool VerifyIfVideoIsThisAccount(int _videoId, int _accountId)
        {
            using (var db = new Database())
            {
                try
                {
                    Video video = db.Videos.Where(video => video.videoId == _videoId).First();
                    if (video != null)
                    {
                        if (video.owner != _accountId)
                        {
                            return false;
                        }

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
        #endregion
    }
}
