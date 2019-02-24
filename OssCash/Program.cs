using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OssCash
{
    class Program
    {
        static void Main(string[] args)
        {
           var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            Console.Write("OSS file reading started");

            Task<IEnumerable<string>> lineList =  FileEx.ReadAllLinesAsync(config["inputFile"], config["outputFile"], config["errorFile"]);

            Console.Write("OSS file reading completed");
        }


       

    }
}
