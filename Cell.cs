using System;
using System.Collections.Generic;
using SalisburyChessEngine.Pieces;

namespace SalisburyChessEngine
{
    public class Cell
    {
        public int ColumnNumber { get; set; }
        public int Row { get; set; }
        public string Coordinates { get; set; }
        public PieceBase CurrentPiece { get; set; }
        public static Dictionary<int, char> ColumnNumbersMappedToLetters { get; } =
        new Dictionary<int, char>()
        {
           { 1, 'a'},
           { 2, 'b'},
           { 3, 'c'},
           { 4, 'd'},
           { 5, 'e'},
           { 6, 'f'},
           { 7, 'g'},
           { 8, 'h'},
        };
        public static Dictionary<char, int> ColumnLettersMappedToNumbers { get; } =
        new Dictionary<char, int>()
        {
           { 'a', 1},
           { 'b', 2},
           { 'c', 3},
           { 'd', 4},
           { 'e', 5},
           { 'f', 6},
           { 'g', 7},
           { 'h', 8},
        };
        public Cell(int row, int column)
        {
            this.ColumnNumber = column;
            this.Row = row;
            this.CurrentPiece = null;
            char columnLetter;
            ColumnNumbersMappedToLetters.TryGetValue(column, out columnLetter);
            Coordinates = columnLetter + row.ToString();
        }

        public override string ToString()
        {
            if (this.CurrentPiece != null)
            { 
                return this.CurrentPiece.ToString();
            }
            return ".";
        }
    }
}