using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalisburyChessEngine.Board.Positions
{
    public class BoardPosition : Dictionary<string, char>
    {
        public BoardPosition()
        {
            //
            Add("a8", 'r');
            Add("b8", 'n');
            Add("c8", 'b');
            Add("d8", 'q');
            Add("e8", 'k');
            Add("f8", 'b');
            Add("g8", 'n');
            Add("h8", 'r');

            Add("a7", 'p');
            Add("b7", 'p');
            Add("c7", 'p');
            Add("d7", 'p');
            Add("e7", 'p');
            Add("f7", 'p');
            Add("g7", 'p');
            Add("h7", 'p');

            Add("a1", 'R');
            Add("b1", 'N');
            Add("c1", 'B');
            Add("d1", 'Q');
            Add("e1", 'K');
            Add("f1", 'B');
            Add("g1", 'N');
            Add("h1", 'R');

            Add("a2", 'P');
            Add("b2", 'P');
            Add("c2", 'P');
            Add("d2", 'P');
            Add("e2", 'P');
            Add("f2", 'P');
            Add("g2", 'P');
            Add("h2", 'P');
        }
    }
}
