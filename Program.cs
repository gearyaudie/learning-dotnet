using System;
using System.Data;
using System.Globalization;
using Dapper;
using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.Data.SqlClient;

namespace HelloWorld 
{
    internal class Program 
    {
        static void Main(string[] args)
        {
            DataContextDapper dapper = new DataContextDapper();



            string sqlCommand = "SELECT GETDATE()";

            DateTime rightNow = dapper.LoadDataSingle<DateTime>(sqlCommand);

            Computer myComputer = new Computer()
            {
                Motherboard = "Z690",
                HasWifi = true,
                HasLTE = false,
                ReleaseDate = DateTime.Now,
                Price = 957.34m,
                VideoCard = "RTX 2060"
            };

            string sql = @"INSERT INTO TutorialAppSchema.Computer (
                Motherboard,
                HasWifi,
                HasLTE,
                ReleaseDate,
                Price,
                VideoCard
            ) VALUES ('" + myComputer.Motherboard 
                + "','" + myComputer.HasWifi
                + "','" + myComputer.HasLTE
                + "','" + myComputer.ReleaseDate.ToString("yyyy-MM-ddTHH:mm:ss")
                + "','" + myComputer.Price.ToString(CultureInfo.InvariantCulture)
                + "','" + myComputer.VideoCard
            + "')";

            bool result = dapper.ExecuteSql(sql);

            string sqlSelect = @"SELECT 
                Computer.Motherboard,
                Computer.HasWifi,
                Computer.HasLTE,
                Computer.ReleaseDate,
                Computer.Price,
                Computer.VideoCard FROM TutorialAppSchema.Computer"; 

            IEnumerable<Computer> computers = dapper.LoadData<Computer>(sqlSelect);

            foreach(Computer singleComputer in computers)
            {
                Console.WriteLine(
                    "'" + myComputer.Motherboard 
                + "','" + myComputer.HasWifi
                + "','" + myComputer.HasLTE
                + "','" + myComputer.ReleaseDate.ToString("yyyy-MM-ddTHH:mm:ss")
                + "','" + myComputer.Price.ToString(CultureInfo.InvariantCulture)
                + "','" + myComputer.VideoCard
                    + "'" );
            }
        }
    }
}