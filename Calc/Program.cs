using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    class Program
    {
        
        static void Main(string[] args)
        {
            
            string[] calc = Environment.GetCommandLineArgs(); 
            const  int ValidCommandLineArgsLength = 2;   
                
            if (calc.Length != ValidCommandLineArgsLength)
            {
                Console.WriteLine("Invalid Input");                
            }
            else
            {
                string text = calc[ValidCommandLineArgsLength-1];
                Operations calculator = new Operations();
                calculator.CalculateTheInput(text);                 
            }

            
            Console.WriteLine("Syed Shean Assignment Demonstration");
            Console.ReadKey();
        }
    }
}
