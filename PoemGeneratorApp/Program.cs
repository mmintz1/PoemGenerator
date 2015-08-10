using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemGeneratorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            PoemGenerator generator = new PoemGenerator();
            generator.SetPoemFile("../../PoemRules.txt");
            string poem = generator.GeneratePoem();

            Console.WriteLine(poem);
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }
    }
}
