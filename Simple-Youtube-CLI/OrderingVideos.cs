using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple_Youtube_CLI
{
    public static class OrderingVideos
    {
        public static List<Video> videos;

        public static Dictionary<int, int> likeCount;

        public static Dictionary<int, int> dislikeCount;

        public static void OrderVideos(List<Video> _videos)
        {
            likeCount = new Dictionary<int, int>();
            dislikeCount = new Dictionary<int, int>();

            foreach (Video video in _videos)
            {
                likeCount.Add(video.videoId, Video.GetLikesNumber(video.videoId));
                dislikeCount.Add(video.videoId, Video.GetDislikesNumber(video.videoId));
            }

            videos = _videos;

            for (; ; )
            {
                Console.Clear();

                Console.WriteLine("< Lista de Videos >\n");
                
                ShowVideos();

                Console.WriteLine("Pressione \"L\" para ordenar por likes em order decrescente!");
                Console.WriteLine("Pressione \"O\" para ordenar por likes em order crescente!\n");

                Console.WriteLine("Pressione \"D\" para ordenar por Dislikes em order decrescente");
                Console.WriteLine("Pressione \"U\" para ordenar por Dislikes em order crescente\n");

                Console.WriteLine("Pressione \"V\" para ordenar por Visualizações em order decrescente");
                Console.WriteLine("Pressione \"C\" para ordenar por Visualizações em order crescente\n");

                Console.WriteLine("Pressione ENTER para voltar\n");

                Console.Write("Opção: ");

                var option = Console.ReadKey();

                switch(option.Key.ToString().ToUpper())
                {
                    case "L":
                        videos = videos.OrderByDescending(video => likeCount.GetValueOrDefault(video.videoId)).ToList();
                        break;
                    case "O":
                        videos = videos.OrderBy(video => likeCount.GetValueOrDefault(video.videoId)).ToList();
                        break;
                    case "D":
                        videos = videos.OrderByDescending(video => dislikeCount.GetValueOrDefault(video.videoId)).ToList();
                        break;
                    case "U":
                        videos = videos.OrderBy(video => dislikeCount.GetValueOrDefault(video.videoId)).ToList();
                        break;
                    case "V":
                        videos = videos.OrderByDescending(video => video.views).ToList();
                        break;
                    case "C":
                        videos = videos.OrderBy(video => video.views).ToList();
                        break;
                    default:
                        videos = null;
                        likeCount = null;
                        dislikeCount = null;
                        return;
                }
            }
        }
        
        public static void ShowVideos()
        {
            foreach (Video video in videos)
            {
                Console.WriteLine($"Video {video.videoId}:");
                Console.WriteLine($"\tTitulo: {video.title}");
                Console.WriteLine($"\tDescrição: {video.description}");
                Console.WriteLine($"\tCategoria: {video.category}");
                Console.WriteLine($"\tData de publicação: {video.createdAt}");
                Console.WriteLine($"\tVisualizacoes: {video.views}");
                Console.WriteLine($"\tLikes: {Video.GetLikesNumber(video.videoId)}");
                Console.WriteLine($"\tDeslikes: {Video.GetDislikesNumber(video.videoId)}");
                Console.WriteLine("\tCanal: {0}\n", (Account.GetUsernameById(video.owner)));
            }
        }
    }
}
