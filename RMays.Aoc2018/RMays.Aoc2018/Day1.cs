using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018
{
    public class Day1
    {
        public long SolveA(string input)
        {
            var myList = input.Split(new char[] { ',', '\r', '\n' }).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();

            long runningCount = 0;
            foreach(var token in myList)
            {
                var sign = token[0];
                var mag = long.Parse(token.Substring(1));
                switch (sign)
                {
                    case '+':
                        runningCount += mag;
                        break;
                    case '-':
                        runningCount -= mag;
                        break;
                }
            }

            return runningCount;
        }

        public long SolveB(string input)
        {
            var myList = input.Split(new char[] { ',', '\r', '\n' }).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();

            long runningCount = 0;
            var safety = 0;
            var foundNums = new List<long>() { 0 };
            while(safety < 9999)
            {
                foreach (var token in myList)
                {
                    var sign = token[0];
                    var mag = long.Parse(token.Substring(1));
                    switch (sign)
                    {
                        case '+':
                            runningCount += mag;
                            break;
                        case '-':
                            runningCount -= mag;
                            break;
                    }

                    if (foundNums.Contains(runningCount))
                    {
                        return runningCount;
                    }

                    foundNums.Add(runningCount);
                    //safety++;
                }
            }

            return -1;
        }
    }
}
