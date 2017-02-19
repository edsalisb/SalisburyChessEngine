using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalisburyChessEngine.Moves;

namespace SalisburyChessEngine.Utilities
{
    class ListUtilities
    {
        public static string SelectCoordinates(PotentialMoves arg)
        {
            return arg.Coordinates;
        }
    }
}
