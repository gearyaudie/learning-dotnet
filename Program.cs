using System;
using System.Data;
using System.Globalization;
using System.IO.Compression;
using System.Text.Json;
using AutoMapper;
using Dapper;
using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HelloWorld 
{
    internal class Program 
    {
        static void Main(string[] args)
        {

            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            DataContextDapper dapper = new DataContextDapper(config);



            // Computer myComputer = new Computer()
            // {
            //     Motherboard = "Z690",
            //     HasWifi = true,
            //     HasLTE = false,
            //     ReleaseDate = DateTime.Now,
            //     Price = 957.34m,
            //     VideoCard = "RTX 2060"
            // };

            // string sql = @"INSERT INTO TutorialAppSchema.Computer (
            //     Motherboard,
            //     HasWifi,
            //     HasLTE,
            //     ReleaseDate,
            //     Price,
            //     VideoCard
            // ) VALUES ('" + myComputer.Motherboard 
            //     + "','" + myComputer.HasWifi
            //     + "','" + myComputer.HasLTE
            //     + "','" + myComputer.ReleaseDate.ToString("yyyy-MM-ddTHH:mm:ss")
            //     + "','" + myComputer.Price.ToString(CultureInfo.InvariantCulture)
            //     + "','" + myComputer.VideoCard
            // + "')\n";

            // File.WriteAllText("log.txt", sql);
            // using StreamWriter openFile = new("log.txt", append: true);

            // openFile.WriteLine(sql);
            // openFile.Close();

            // Console.WriteLine(File.ReadAllText("log.txt"));

            string computersJson = File.ReadAllText("ComputersSnake.json");

            Mapper mapper = new Mapper(new MapperConfiguration((cfg) =>
            {
                cfg.CreateMap<ComputerSnake, Computer>()
                .ForMember(destination => destination.ComputerId, options => options.MapFrom(source => source.computer_id))
                .ForMember(destination => destination.CPUCores, options => options.MapFrom(source => source.cpu_cores))
                .ForMember(destination => destination.HasLTE, options => options.MapFrom(source => source.has_lte))
                .ForMember(destination => destination.HasWifi, options => options.MapFrom(source => source.has_wifi))
                .ForMember(destination => destination.Motherboard, options => options.MapFrom(source => source.motherboard))
                .ForMember(destination => destination.VideoCard, options => options.MapFrom(source => source.video_card))
                .ForMember(destination => destination.ReleaseDate, options => options.MapFrom(source => source.release_date))
                .ForMember(destination => destination.Price, options => options.MapFrom(source => source.price));
            }));

            // IEnumerable<ComputerSnake>? computerSystem = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ComputerSnake>>(computersJson);


            IEnumerable<Computer>? computerSystem = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson);

            if(computerSystem != null)
            {
                foreach(Computer computer in computerSystem)
                {
                    Console.WriteLine(computer.Motherboard);
                }
            }


            // if(computerSystem != null)
            // {
            //     IEnumerable<Computer> computerResult = mapper.Map<IEnumerable<Computer>>(computerSystem);

            //     foreach(Computer computer in computerResult)
            //     {
            //         Console.WriteLine(computer.Motherboard);
            //     }
            // }

            // Console.WriteLine(computersJson);

            // JsonSerializerOptions options = new JsonSerializerOptions()
            // {
            //     PropertyNamingPolicy = JsonNamingPolicy.CamelCase  
            // };

            IEnumerable<Computer>? computers = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computersJson);

            if(computers != null)
            {
                foreach(Computer computer in computers)
                {
                    string sql = @"INSERT INTO TutorialAppSchema.Computer (
                        Motherboard,
                        HasWifi,
                        HasLTE,
                        ReleaseDate,
                        Price,
                        VideoCard
                    ) VALUES ('" + EscapeSingleQuote(computer.Motherboard)
                        + "','" + computer.HasWifi
                        + "','" + computer.HasLTE
                        + "','" + computer.ReleaseDate?.ToString("yyyy-MM-ddTHH:mm:ss")
                        + "','" + computer.Price.ToString(CultureInfo.InvariantCulture)
                        + "','" + EscapeSingleQuote(computer.VideoCard)
                    + "')\n";
                    dapper.ExecuteSql(sql);
                }
            }

            string computersCopy = JsonConvert.SerializeObject(computers);

            File.WriteAllText("computersCopyNewtonsoft.txt", computersCopy);


            // File.WriteAllText("computersCopySystem.txt", computersCopySystem);
        
        }

        static string EscapeSingleQuote(string input)
        {
            string output = input.Replace("'", "''");
            return output;
        }
    }
}