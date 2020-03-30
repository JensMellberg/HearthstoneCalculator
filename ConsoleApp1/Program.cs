using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp1
{

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {



             TestRunner.runTests();
            BoardStateReader.Run();
           
           // TurnByTurnChecker.runTests();
          
            //StatisticsChecker.runTests();
        }
    }
}