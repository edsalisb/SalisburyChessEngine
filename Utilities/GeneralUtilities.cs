using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalisburyChessEngine.Board;
namespace SalisburyChessEngine.Utilities
{
    class GeneralUtilities
    {
        public static string SelectCoordinates(ValidBoardMove arg)
        {
            return arg.CoordinatesTo;
        }
    }

    static class Extensions
    {
        public static char? getColumnLetter(this string coords)
        {
            if (coords.Length >= 1)
            {
                return coords[0];
            }
            return null;

        }

        public static char? getRowNumber(this string coords)
        {
            if (coords.Length >= 2)
            {
                return coords[1];
            }
            return null;
        }

        public static bool isBetween<T>(this T item, T constraint1, T constraint2)
        {
            T upper, lower;

            if (constraint1 < constraint2)
            {

            }
        }
    }
}
