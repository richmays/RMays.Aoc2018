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

        public long Go(HashSet<string> rules, long generations)
        {
            long prevSum = 0;
            var CycleToFind = new List<long>();
            var Sums = new List<long>();
            for(int i = 0; i < 5; i++)
            {
                CycleToFind.Add(0);
            }

            var sumDiffs = new List<long>();

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


                /*
                if (gen % 1000 == 0)
                {
                    Console.WriteLine($"gen: {gen}, sum: {this.PlantSum()}: {this}");
                }
                */

                var plantSum = this.PlantSum();
                long sumDiff = plantSum - prevSum;
                //Console.WriteLine($"{gen}: {plantSum} ({sumDiff})");
                prevSum = plantSum;

                var firstCycleOffset = 0;
                var secondCycleOffset = 0;

                var foundMatch = false;
                int i;
                for (i = 0; i < sumDiffs.Count - 5; i++)
                {
                    if (sumDiffs[i] != CycleToFind[0]) continue;
                    foundMatch = true;
                    for (int j = 1; j < 5; j++)
                    {
                        if (sumDiffs[i + j] != CycleToFind[j])
                        {
                            foundMatch = false;
                            break;
                        }

                        if (foundMatch)
                        {
                            if (firstCycleOffset == 0)
                            {
                                //Console.WriteLine($"gen: {gen}; First loop found; offset is {i}!");
                                firstCycleOffset = i;
                            }
                            else if (secondCycleOffset == 0 && i > firstCycleOffset)
                            {
                                //Console.WriteLine($"gen: {gen}; Second loop found; offset is {i}!");
                                secondCycleOffset = i;

                                // NOTE: This won't work if the differences don't converge to a single number!
                                // We'll need to calculate the sum of the differences for the entire cycle,
                                //  then divide by the cycle length.
                                // Then we use that number as the multiplier in teh 'return' statement.


                                // Now let's do some clever math to find the answer.

                                var lengthOfCycle = (secondCycleOffset - firstCycleOffset);
                                // Cycle starts at 'firstCycleOffset'.

                                // How many more steps do we need to take to get to the end?
                                var stepsNeeded = generations - i - 1;
                                while(stepsNeeded % lengthOfCycle != 0)
                                {
                                    // Take a step back.
                                    i--;
                                    stepsNeeded = generations - i;
                                }

                                return Sums[i] + (stepsNeeded * sumDiffs[i]);
                            }
                        }
                    }
                }

                Sums.Add(plantSum);
                sumDiffs.Add(sumDiff);
                CycleToFind.RemoveAt(0);
                CycleToFind.Add(sumDiff);
            }

            return PlantSum();
        }

        public long PlantSum()
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

            return myPots.Go(rules, generations);
        }
    }
}
