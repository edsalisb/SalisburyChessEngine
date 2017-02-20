using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalisburyChessEngine.Board;
namespace SalisburyChessEngine.Utilities
{
    class ListUtilities
    {
        public static string SelectCoordinates(ValidBoardMove arg)
        {
            return arg.CoordinatesTo;
        }
    }
}
