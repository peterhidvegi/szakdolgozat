using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace Validator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string mainPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
        public static string nameFTP = "smartparking";
        public static string passFTP = "HIP29cod";
        public static bool fromFTP;
        public static string addressOfFTP = "ftp://ftp.atw.hu/";
        public static string snapDirName = "\\snap\\";
        public static string motionDirName = "\\motion\\";
        public static string masksDirName = "\\masks\\";
        public static string maskDirName = "\\mask\\";
        public static string dbName = "log";
        public static string dbTicket = "ticket";
        public static string oldImgLocal = "cam\\archive\\";
        public static string newImgLocal = "cam2\\archive\\";
        internal static int motionMinDiffAvg;
        internal static int motionMaxDiffAvg;
        internal static int motionPixelCorrectLimit;
        internal static float motionMainCorrectPixelPercentage;
        internal static float motionNodeCorrectPixelPercentage;

        public App()
        {
            motionMinDiffAvg = 2;
            motionMaxDiffAvg = 3;
            motionPixelCorrectLimit = 25;
            motionMainCorrectPixelPercentage = 98;
            motionNodeCorrectPixelPercentage = 75;
        }

        public static MySqlConnection getEPSMySqlConnection()
        {
            MySqlConnectionStringBuilder myBuilder = new MySqlConnectionStringBuilder();
            myBuilder.Database = "smartparking";
            myBuilder.Server = "eu-cdbr-azure-west-d.cloudapp.net";
            myBuilder.Port = 3306;
            myBuilder.UserID = "bd0ea03098e4e8";
            myBuilder.Password = "e28ca23e";
            myBuilder.ConvertZeroDateTime = true;

            MySqlConnection myconn = new MySqlConnection(myBuilder.ConnectionString);
            if (myconn.State == ConnectionState.Closed)
            {
                myconn.Open();
            }

            return myconn;
        }

        public static MySqlConnection GetMySqlConnection(string dataBase, string server, uint port, string userID, string pass)
        {
            MySqlConnectionStringBuilder myBuilder = new MySqlConnectionStringBuilder();
            myBuilder.Database = dataBase;
            myBuilder.Server = server;
            myBuilder.Port = port;
            myBuilder.UserID = userID;
            myBuilder.Password = pass;
            myBuilder.ConvertZeroDateTime = true;

            MySqlConnection myconn = new MySqlConnection(myBuilder.ConnectionString);
            if (myconn.State == ConnectionState.Closed)
            {
                myconn.Open();
            }

            return myconn;
        }
    }
}
