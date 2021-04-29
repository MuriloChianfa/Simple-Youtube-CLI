using System;
using System.Threading;

using Microsoft.Data.Sqlite;

namespace Simple_Youtube_CLI
{
    class Program
    {
        public static void Main()
        {
            Console.ForegroundColor = Constants.Color;

            int option;

            do
            {
                Console.Clear();

                Console.WriteLine("Bem vindo ao Youtube!\n");

                Console.WriteLine("+---------------------------+");
                Console.WriteLine("|                           |");
                Console.WriteLine("|    1 - Entrar             |");
                Console.WriteLine("|    2 - Criar conta        |");
                Console.WriteLine("|                           |");
                Console.WriteLine("|    0 - Sair do sistema    |");
                Console.WriteLine("|                           |");
                Console.WriteLine("+---------------------------+\t");

                Console.Write("\nOpção: ");
            } while (!int.TryParse(Console.ReadLine(), out option));
            
            try
            {
                switch (option)
                {
                    case 1: Login(); break;
                    case 2: Create(); break;

                    case 0: Exit(); break;

                    default: Program.Main(); break;
                }
            }
            catch (SqliteException err)
            {
                Console.WriteLine(err.Message);

                Console.ResetColor();
                System.Environment.Exit(1);
            }
            
            Program.Main();
        }

        #region Functions
        public static void Login()
        {
            do
            {
                Console.Clear();

                Console.WriteLine("\t\t< Login >\n");
                Console.WriteLine("Para voltar, entre com um nome vazio!\n\n");

                string username;
                Console.Write("Digite seu username: ");
                username = Console.ReadLine();

                if (username == "")
                {
                    Program.Main();
                }

                string password;
                Console.Write("Digite sua senha: ");
                password = Console.ReadLine();

                try
                {
                    Account account = Account.GetAccount(username, password);

                    Simple_Youtube_CLI.Logged.Logged.SetAccount(account);

                    Simple_Youtube_CLI.Logged.Logged.Menu();
                }
                catch (InvalidOperationException)
                {
                    Console.Clear();

                    Console.WriteLine("Username ou Senha inválidos!");
                    Console.Write("\nPressione ENTER para continuar...");

                    Console.ReadKey();
                }
                catch (ArgumentException err)
                {
                    Console.Clear();

                    Console.WriteLine(err.Message);

                    Console.Write("\nPressione ENTER para continuar...");

                    Console.ReadKey();
                }
            } while (true);
        }

        public static void Create()
        {
            do
            {
                Console.Clear();

                Console.WriteLine("\t\t< Cadastro >\n");
                Console.WriteLine("Para voltar, entre com um nome vazio!\n\n");

                string username;
                Console.Write("Digite um username: ");
                username = Console.ReadLine();

                if (username == "")
                {
                    Program.Main();
                }

                string password;
                Console.Write("Digite uma senha: ");
                password = Console.ReadLine();

                try
                {
                    Account account = new();

                    if (Account.VerifyUsername(username))
                    {
                        account.username = username;
                        account.password = password;

                        if (Account.AddAccount(account))
                        {
                            Simple_Youtube_CLI.Logged.Logged.SetAccount(Account.GetAccount(username, password));

                            Simple_Youtube_CLI.Logged.Logged.Menu();
                        }
                    }
                }
                catch (ArgumentException err)
                {
                    Console.Clear();

                    Console.WriteLine(err.Message);

                    Console.Write("\nPressione ENTER para continuar...");

                    Console.ReadKey();
                }
            } while (true);
        }

        public static void Exit()
        {
            for (int i = 0; i < 4; i++)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Saindo do sistema{0}", (new String('.', i)));
                Console.ResetColor();

                Thread.Sleep(350);
            }

            System.Environment.Exit(0);
        }
        #endregion
    }
}
