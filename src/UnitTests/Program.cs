using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq;

namespace UnitTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //MLPTest test = new MLPTest();
            //SVMTest test = new SVMTest();

            //test.Process();

            double[] t = { 1,2,3};

            t.ForEach(g => Console.WriteLine(g));

            //Console.WriteLine(t);
        }
    }
}