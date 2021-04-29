using System;
using System.Collections.Generic;
using System.Threading;

namespace Simple_Youtube_CLI.Logged
{
    public static class Logged
    {
        private static Account account;

        public static Mutex listeningMutex = new(false, "listeningMutex");
        public static bool listening = false;

        public static void SetAccount(Account _account)
        {
            account = _account;
        }

        public static void Menu()
        {
            int option;

            do
            {
                Console.Clear();

                Console.WriteLine($"Bem vindo de volta, senhor(a) \"{account.username}\"!\n");

                Console.WriteLine("+-----------------------------+");
                Console.WriteLine("|                             |");
                Console.WriteLine("|    1 - Listar video(s)      |");
                Console.WriteLine("|    2 - Listar meus videos   |");
                Console.WriteLine("|                             |");
                Console.WriteLine("|    3 - Visualizar video     |");
                Console.WriteLine("|                             |");
                Console.WriteLine("|    4 - Adicionar video      |");
                Console.WriteLine("|    5 - Editar video         |");
                Console.WriteLine("|    6 - Excluir video        |");
                Console.WriteLine("|                             |");
                Console.WriteLine("|    8 - Editar conta         |");
                Console.WriteLine("|    9 - Excluir conta        |");
                Console.WriteLine("|    0 - Deslogar             |");
                Console.WriteLine("|                             |");
                Console.WriteLine("+-----------------------------+\t");

                Console.Write("\nOpção: ");
            } while (!int.TryParse(Console.ReadLine(), out option));

            switch (option)
            {
                case 1: ListAll(); break;
                case 2: ListAll(true); break;

                case 3: ViewVideo(); break;

                case 4: AddVideo(); break;
                case 5: EditVideo(); break;
                case 6: RemoveVideo(); break;

                case 8: EditAccount(); break;
                case 9: RemoveAccount(); break;
                case 0: account = null; Program.Main(); break;

                default: Logged.Menu(); break;
            }

            Logged.Menu();
        }

        #region VideoMethods
        private static void ListAll(bool myVideos = false)
        {
            Console.Clear();

            List<Video> videos = account.ListAll(myVideos);

            if (videos.Count == 0)
            {
                if (myVideos)
                {
                    Console.WriteLine("Você não possui nenhum video!");
                }
                else
                {
                    Console.WriteLine("O youtube ainda não possui nenhum video!");
                }

                Console.Write("\nPressione ENTER para continuar...");
                Console.ReadKey();
                return;
            }

            OrderingVideos.OrderVideos(videos);
        }

        private static void ViewVideo()
        {
            Console.Clear();

            Console.WriteLine("\t< Visualização de Video >\n");

            int videoId;
            Console.WriteLine("Digite o número do video há ser visto!\n");

            do
            {
                do
                {
                    Console.Write("ID: ");
                } while (!int.TryParse(Console.ReadLine(), out videoId));
            } while (!Video.VerifyVideoById(videoId));

            if (Video.View(videoId))
            {
                Console.WriteLine("\nVideo visto com sucesso!\n");

                Console.WriteLine("Qual sua satisfação com este video ?\n");

                Console.WriteLine("1 - Gostei");
                Console.WriteLine("2 - Não gostei");
                Console.WriteLine("3 - Sem opnião\n");

                int option;
                do
                {
                    Console.Write("Opção: ");
                } while (!int.TryParse(Console.ReadLine(), out option) && !option.In(1, 2, 3));

                try
                {
                    if (Vote.VerifyIfAlreadyVote(account.accountId, videoId))
                    {
                        int accountVote = Vote.WitchVote(account.accountId, videoId);

                        if (accountVote != option)
                        {
                            if (option != 3)
                            {
                                Vote.SwitchVote(account.accountId, videoId);
                            }
                        }

                        return;
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);

                    Console.Write("\nPressione ENTER para continuar...");
                    Console.ReadKey();
                    return;
                }

                switch(option)
                {
                    case 1:
                        try
                        {
                            if (!Like.Vote(account.accountId, videoId))
                            {
                                throw new Exception("\nOcorreu algum problema ao votar!");
                            }
                            else
                            {
                                Console.WriteLine("\nVoto computado com sucesso!");
                            }
                        }
                        catch (Exception err)
                        {
                            Console.WriteLine(err.Message);
                        }
                        break;
                    case 2:
                        try
                        {
                            if (!Dislike.Vote(account.accountId, videoId))
                            {
                                throw new Exception("\nOcorreu algum problema ao votar!");
                            }
                            else
                            {
                                Console.WriteLine("\nVoto computado com sucesso!");
                            }
                        }
                        catch (Exception err)
                        {
                            Console.WriteLine(err.Message);
                            return;
                        }
                        break;
                    case 3:
                        // 
                        break;
                }

                Console.Write("\nPressione ENTER para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nOcorreu algum problema ao ver o video!");

            Console.Write("\nPressione ENTER para continuar...");
            Console.ReadKey();
        }

        private static void AddVideo()
        {
            Console.Clear();

            Console.WriteLine("\t< Upload de Video >\n");

            string title;
            Console.Write("Digite o titulo do Video: ");
            title = Console.ReadLine();

            string description;
            Console.Write("Digite a descrição do Video: ");
            description = Console.ReadLine();

            Category category;
            Console.WriteLine("\nEscolha uma Categoria: ");
            foreach (Category categoryListed in Enum.GetValues(typeof(Category)))
            {
                Console.WriteLine($"\t{(UInt32)categoryListed} = {categoryListed.ToString()}");
            }

            Console.Write("\n");

            do
            {
                Console.Write("Opção: ");
            } while (!Enum.TryParse(Console.ReadLine(), out category));

            if (account.AddVideo(title, description, category))
            {
                Console.WriteLine("\nVideo Adicionado com sucesso!");

                Console.Write("\nPressione ENTER para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\n{account.ErrorMessage}");

            Console.Write("\nPressione ENTER para continuar...");
            Console.ReadKey();
            return;
        }

        private static void EditVideo()
        {
            Console.Clear();

            Console.WriteLine("\t< Edição de Video >\n");

            int videoId;
            Console.WriteLine("Digite o número do video há ser editado!");

            do
            {
                do
                {
                    Console.Write("\nID: ");
                } while (!int.TryParse(Console.ReadLine(), out videoId));
            } while (!Video.VerifyVideoById(videoId));

            if (!Video.VerifyIfVideoIsThisAccount(videoId, account.accountId))
            {
                Console.WriteLine("\nVocê não é o Owner deste video!");

                Console.Write("\nPressione ENTER para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nEditar qual opção ?");

            Console.WriteLine("1 - Titulo");
            Console.WriteLine("2 - Descrição");
            Console.WriteLine("3 - Categoria\n");

            int option;
            do
            {
                Console.Write("Opção: ");
            } while (!int.TryParse(Console.ReadLine(), out option));

            string title = null;
            string description = null;
            Category category = Category.Action;

            switch (option)
            {
                case 1:
                    Console.Write("Digite o titulo do Video: ");
                    title = Console.ReadLine();
                    break;
                case 2:
                    Console.Write("Digite a descrição do Video: ");
                    description = Console.ReadLine();
                    break;
                case 3:
                    Console.WriteLine("\nEscolha uma Categoria: ");
                    foreach (Category categoryListed in Enum.GetValues(typeof(Category)))
                    {
                        Console.WriteLine($"\t{(UInt32)categoryListed} = {categoryListed.ToString()}");
                    }

                    do
                    {
                        Console.Write("Opção: ");
                    } while (!Enum.TryParse(Console.ReadLine(), out category));
                    break;

                default: Console.WriteLine("Opção inválida!"); Program.Main(); break;
            }

            if (account.EditVideo(videoId, title, description, category))
            {
                Console.WriteLine("\nVideo Editado com sucesso!");

                Console.Write("\nPressione ENTER para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\n{account.ErrorMessage}");

            Console.Write("\nPressione ENTER para continuar...");
            Console.ReadKey();
            return;
        }

        private static void RemoveVideo()
        {
            Console.Clear();

            Console.WriteLine("\t< Exclusão de Video >\n");

            int videoId;
            Console.WriteLine("Digite o número do video há ser excluido!");

            do
            {
                do
                {
                    Console.Write("\nID: ");
                } while (!int.TryParse(Console.ReadLine(), out videoId));
            } while (!Video.VerifyVideoById(videoId));

            if (!Video.VerifyIfVideoIsThisAccount(videoId, account.accountId))
            {
                Console.WriteLine("\nVocê não é o Owner deste video!");

                Console.Write("\nPressione ENTER para continuar...");
                Console.ReadKey();
                return;
            }

            if (account.RemoveVideo(videoId))
            {
                Console.WriteLine("\nVideo Excluido com sucesso!");

                Console.Write("\nPressione ENTER para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\n{account.ErrorMessage}");

            Console.Write("\nPressione ENTER para continuar...");
            Console.ReadKey();
            return;
        }
        #endregion

        #region AccoundMethods
        private static void EditAccount()
        {
            Console.Clear();

            Console.WriteLine("\t< Editar Conta >");

            Console.WriteLine("\nEditar qual opção ?");

            Console.WriteLine("1 - Username");
            Console.WriteLine("2 - Password\n");

            int option;
            do
            {
                Console.Write("Opção: ");
            } while (!int.TryParse(Console.ReadLine(), out option));

            string username = null;
            string password = null;

            switch (option)
            {
                case 1:
                    Console.Write("Digite o novo username: ");
                    username = Console.ReadLine();
                    break;
                case 2:
                    Console.Write("Digite a nova senha: ");
                    password = Console.ReadLine();
                    break;

                default:
                    Console.WriteLine("Opção inválida!");

                    Console.Write("\nPressione ENTER para continuar...");
                    Console.ReadKey();

                    Logged.Menu();
                    break;
            }

            try
            {
                if (Account.VerifyUsername(username))
                {
                    if (account.EditAccount(account, username, password))
                    {
                        Console.WriteLine("\nConta Editada com sucesso!");

                        Console.Write("\nPressione ENTER para continuar...");
                        Console.ReadKey();
                        return;
                    }
                }
            }
            catch (ArgumentException err)
            {
                Console.Clear();

                Console.WriteLine(err.Message);
                Console.Write("\nPressione ENTER para continuar...");

                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\n{account.ErrorMessage}");

            Console.Write("\nPressione ENTER para continuar...");
            Console.ReadKey();
        }

        private static void RemoveAccount()
        {
            Console.Clear();

            Console.WriteLine("Deseja mesmo excluir sua conta ?\n");
            Console.WriteLine("1 - Sim!");
            Console.WriteLine("2 - Não!\n");

            int option;
            do
            {
                Console.Write("Opção: ");
            } while (!int.TryParse(Console.ReadLine(), out option));

            switch (option)
            {
                case 1:
                    if (account.RemoveAccount(account))
                    {
                        account = null;

                        Program.Main();
                    }
                    break;
                case 2:
                    break;

                default: 
                    Console.WriteLine("Opção inválida!");

                    Console.Write("\nPressione ENTER para continuar...");
                    Console.ReadKey();

                    Logged.Menu(); 
                    break;
            }
        }
        #endregion
    }
}
