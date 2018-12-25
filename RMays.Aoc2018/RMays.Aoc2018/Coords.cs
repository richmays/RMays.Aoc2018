using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018
{
    public class Coords
    {
        public Coords()
        {
        }

        public Coords(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Row { get; set; }
        public int Col { get; set; }

        public Coords Up()
        {
            return new Coords(Row - 1, Col);
        }
        public Coords Down()
        {
            return new Coords(Row + 1, Col);
        }
        public Coords Left()
        {
            return new Coords(Row, Col - 1);
        }
        public Coords Right()
        {
            return new Coords(Row, Col + 1);
        }
    }
}
