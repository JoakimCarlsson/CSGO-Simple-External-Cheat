using System;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;

namespace DataBase
{
    public class Connection
    {
        private  MySqlConnection _connection;
        private  MySqlCommand _cmd;
        private  string _connectionString =
            "Server=MYSQL5011.myWindowsHosting.com;Database=db_9b8e03_smurf;Uid=9b8e03_smurf;Pwd=Phanta123!";
        public  bool loggedIn = false;
        public  string username;
        public  string password;

        public void Main()
        {

            Console.Write("Username: ");
            username = Console.ReadLine();
            Console.Write("Password: ");
            password = Console.ReadLine();
           password = CalculateMD5Hash(password);
            Login(username, password);
        }

        public void Login(string username, string password)
        {
            using (_connection = new MySqlConnection(_connectionString))
            {
                using (_cmd = _connection.CreateCommand())
                {
                    //Opens to the connection to the database.
                    _connection.Open();
                    try
                    {
                        //This is the command we'll run on the database.
                        _cmd.CommandText = "SELECT * FROM `users` WHERE username = @user AND password = @pass;";
                        //We will replace @hwid with our field _hwid. We do this to avoid SQL injection.
                        _cmd.Parameters.AddWithValue("@user", username);
                        //We will replace @user with our field username. We do this to avoid SQL injection.
                        _cmd.Parameters.AddWithValue("@pass", password);
                        //EXecutes the command.
                        var test = _cmd.ExecuteScalar();

                        if (test != null)
                            loggedIn = true;
                        else
                            loggedIn = false;
                    }
                    catch (Exception e)
                    {
                        //If we have any issues we write so hear.
                        Console.WriteLine(e);
                    }
                }
            }

        }

        private static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }
    }
}
