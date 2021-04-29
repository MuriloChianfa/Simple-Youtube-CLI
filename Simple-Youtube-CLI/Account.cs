using System;
using System.Collections.Generic;
using System.Linq;

using System.ComponentModel.DataAnnotations.Schema;

namespace Simple_Youtube_CLI
{
    public sealed class Account: AccountModel
    {
        [NotMapped]
        public string ErrorMessage;

        #region StaticMethods
        public static bool VerifyUsername(string _username)
        {
            using (var db = new Database())
            {
                try
                {
                    if (db.Accounts.Where(account => account.username == _username).First() != null)
                    {
                        throw new ArgumentException($"Já existe um usuário com o username de \"{_username}\"");
                    }
                }
                catch (InvalidOperationException)
                {
                    // pass if not found
                }
            }

            return true;
        }

        public static Account GetAccount(string _username, string _password)
        {
            Account account;

            using (var db = new Database())
            {
                try
                {
                    account = db.Accounts.Where(account => account.username == _username).First();
                }
                catch (Exception)
                {
                    throw new ArgumentException("Username ou Senha inválidos!");
                }
            }

            if (account.password != _password)
            {
                throw new ArgumentException("Username ou Senha inválidos!");
            }

            return account;
        }

        public static bool AddAccount(Account _account)
        {
            using (var db = new Database())
            {
                try
                {
                    db.Add(_account);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public static string GetUsernameById(int _id)
        {
            using (var db = new Database())
            {
                return db.Accounts.Where(account => account.accountId == _id).First().username;
            }
        }
        #endregion

        #region VideoMethods
        public List<Video> ListAll(bool _myVideos = false)
        {
            using (var db = new Database())
            {
                try
                {
                    if (_myVideos)
                    {
                        return db.Videos.Where(video => video.owner == this.accountId).ToList();
                    }

                    return db.Videos.ToList();
                }
                catch (Exception err)
                {
                    this.ErrorMessage = err.Message;
                    return new List<Video>();
                }
            }
        }

        public bool AddVideo(string _title, string _description, Category _category)
        {
            using (var db = new Database())
            {
                try
                {
                    db.Videos.Add(new Video
                    {
                        title = _title,
                        description = _description,
                        category = _category,
                        owner = this.accountId
                    });
                    db.SaveChanges();
                }
                catch (Exception err)
                {
                    this.ErrorMessage = err.Message;
                    return false;
                }
            }

            return true;
        }

        public bool EditVideo(int _videoId, string _title = null, string _description = null, Category _category = Category.Action)
        {
            using (var db = new Database())
            {
                try
                {
                    Video video = db.Videos.Where(video => video.videoId == _videoId).First();

                    if (_title != null)
                    {
                        video.title = _title;
                    }

                    if (_description != null)
                    {
                        video.description = _description;
                    }

                    if (video.category != _category)
                    {
                        video.category = _category;
                    }

                    db.Update(video);
                    db.SaveChanges();
                }
                catch (Exception err)
                {
                    this.ErrorMessage = err.Message;
                    return false;
                }
            }

            return true;
        }

        public bool RemoveVideo(int _videoId)
        {
            using (var db = new Database())
            {
                try
                {
                    Video video = db.Videos.Where(video => video.videoId == _videoId).First();

                    db.Remove(video);
                    db.SaveChanges();
                }
                catch (Exception err)
                {
                    this.ErrorMessage = err.Message;
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region AccountMethods
        public bool EditAccount(Account _account, string _username = null, string _password = null)
        {
            using (var db = new Database())
            {
                try
                {
                    if (_username != null)
                    {
                        _account.username = _username;
                    }

                    if (_password != null)
                    {
                        _account.password = _password;
                    }

                    db.Update(_account);
                    db.SaveChanges();
                }
                catch (Exception err)
                {
                    this.ErrorMessage = err.Message;
                    return false;
                }
            }

            return true;
        }

        public bool RemoveAccount(Account _account)
        {
            using (var db = new Database())
            {
                try
                {
                    //  Verificar os videos da conta para excluir tambem
                    List<Video> accountVideos = db.Videos.Where(video => video.owner == _account.accountId).ToList();

                    if (accountVideos.Count > 0)
                    {
                        foreach (Video video in accountVideos)
                        {
                            db.Remove(video);
                        }
                    }

                    db.Remove(_account);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
