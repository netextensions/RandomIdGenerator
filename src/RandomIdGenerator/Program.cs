using Microsoft.Data.Sqlite;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetExtensions
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var loader = new RandomIdLoader(new RandomIdGenerator());
            await loader.Start($"{Guid.NewGuid()}.db", 1000);

            Console.WriteLine("Hello World!");
        }


    }
}
