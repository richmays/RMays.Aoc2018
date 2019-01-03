using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018
{
    public class Pots
    {
        private HashSet<int> PotsSet;
        private int LowPlant;
        private int HighPlant;
        public Pots()
        {
            PotsSet = new HashSet<int>();
            LowPlant = 0;
            HighPlant = 0;
        }

        /// <summary>
        /// Returns whether or not there's a plant in the pot.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool GetPot(int index)
        {
            return PotsSet.Contains(index);
        }

        public void SetPot(int index, bool hasPlant)
        {
            PotsSet.Remove(index);
            if (hasPlant)
            {
                PotsSet.Add(index);
            }
        }

        private void CalcHighLow()
        {
            LowPlant = PotsSet.Min();
            HighPlant = PotsSet.Max();
        }

        public void Go(HashSet<string> rules, long generations)
        {
            int prevSum = 0;

            for (var gen = 1; gen <= generations; gen++)
            {
                CalcHighLow();
                var NewPotsSet = new HashSet<int>();
                string orientation = ".....";
                for (int index = LowPlant - 2; index <= HighPlant + 1; index++)
                {
                    orientation = orientation.Substring(1) + (this.GetPot(index+2) ? '#' : '.');

                    if (rules.Contains(orientation))
                    {
                        NewPotsSet.Add(index);
                    }
                }

                PotsSet.Clear();
                foreach (var pot in NewPotsSet)
                {
                    PotsSet.Add(pot);
                }


                if (gen % 1000 == 0)
                {
                    Console.WriteLine($"gen: {gen}, sum: {this.PlantSum()}: {this}");
                }

                /*
                var plantSum = this.PlantSum();
                Console.WriteLine($"{gen}: {plantSum} ({plantSum - prevSum})");
                prevSum = plantSum;
                */
            }
        }

        public int PlantSum()
        {
            return PotsSet.Sum(x => x);
        }

        public override string ToString()
        {
            CalcHighLow();
            var toReturn = $"{LowPlant}: ";
            for(int i = this.LowPlant; i <= this.HighPlant; i++)
            {
                toReturn += (GetPot(i) ? '#' : '.');
            }
            return toReturn;
            //return "?";
        }
    }

    public class Day12
    {
        public long SolveA(string input, long generations)
        {
            // initial state: #..#.#..##......###...###
            var intialPots = Parser.TokenizeLines(input)[0].Split(' ')[2];

            var myPots = new Pots();
            int potId = 0;
            foreach(var pot in intialPots.ToCharArray())
            {
                if (pot == '#')
                {
                    myPots.SetPot(potId, true);
                }
                potId++;
            }

            // Now read the rules.

            // ...## => #
            var rulesText = Parser.TokenizeLines(input);
            var rules = new HashSet<string>();
            foreach(var line in rulesText)
            {
                var splitLine = line.Split(' ').ToList();
                if (splitLine[1] != "=>") continue;
                if (splitLine[2] == "#")
                {
                    rules.Add(splitLine[0]);
                }
            }

            myPots.Go(rules, generations);
            return myPots.PlantSum();
        }
    }
}
