using System;
using System.Collections.Generic;
using SalisburyChessEngine.Pieces;

namespace SalisburyChessEngine.Board
{
    public class Cell
    {
        public int ColumnNumber { get; set; }
        public int Row { get; set; }
        public string Coordinates { get; set; }
        public PieceBase CurrentPiece { get; set; }
        public char ColumnLetter { get; set; }
        public Cell(int row, int column)
        {
            this.ColumnNumber = column;
            this.Row = row;
            this.CurrentPiece = null;

            BoardProperties.ColumnNumbersMappedToLetters.TryGetValue(column, out char columnLetter);
            this.ColumnLetter = columnLetter;
            Coordinates = columnLetter + row.ToString();
            
        }
        public static PieceBase GetPiece(Cell cell)
        {
            if (HasPiece(cell))
            {
                return cell.CurrentPiece;
            }
            return null;
        }
        public static bool IsNotNull(Cell cell)
        {
            if (cell != null)
            {
                return true;
            }
            return false;
        }

        public static bool HasPiece(Cell cell)
        {
            if (cell != null)
            {
                if (cell.CurrentPiece != null)
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            if (HasPiece(this))
            { 
                return this.CurrentPiece.ToString();
            }
            return ".";
        }
    }
}