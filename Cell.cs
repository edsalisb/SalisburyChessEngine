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
        public char columnLetter { get; set; }
        public Cell(int row, int column)
        {
            this.ColumnNumber = column;
            this.Row = row;
            this.CurrentPiece = null;
            char columnLetter;

            CellProperties.ColumnNumbersMappedToLetters.TryGetValue(column, out columnLetter);
            this.columnLetter = columnLetter;
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