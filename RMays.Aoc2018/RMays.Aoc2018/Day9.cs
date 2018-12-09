using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018
{
    public class Day9
    {
        public long SolveA(string input)
        {
            // input example:
            //  10 players; last marble is worth 1618 points
            var tokens = Parser.Tokenize(input, ' ');
            var players = int.Parse(tokens[0]);
            var lastMarble = int.Parse(tokens[6]);

            var marbles = new List<int>() { 0 };
            var scores = new Dictionary<int, int>();
            for(int i = 1; i <= players; i++)
            {
                scores.Add(i, 0);
            }

            var currPos = 0;
            var currPlayer = 1;
            for(var currMarble = 1; currMarble <= lastMarble; currMarble++)
            {
                if (currMarble % 23 == 0)
                {
                    // Special rules
                    var scoreDelta = currMarble;
                    var removalIndex = (currPos - 7 + marbles.Count()) % marbles.Count();
                    var marbleToRemove = marbles[removalIndex];
                    scoreDelta += marbleToRemove;
                    marbles.Remove(marbleToRemove);
                    currPos = removalIndex;

                    scores[currPlayer] += scoreDelta;
                }
                else
                {
                    // Normal rules
                    var insertIndex = (currPos + 2) % marbles.Count();
                    marbles.Insert(insertIndex, currMarble);
                    currPos = insertIndex;
                }

                currPlayer++;
                if (currPlayer > players)
                {
                    currPlayer = 1;
                }
            }

            return scores.Values.Max();
        }

        public long SolveB(string input)
        {
            var myList = Parser.Tokenize(input);
      

            return 0;
        }
    }
}
