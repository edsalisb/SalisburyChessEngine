using System;

namespace SalisburyChessEngine
{
    public class Move
    {
        private Func<string, Cell> getCell;
        
        public bool IsValid { get; set; }
        public Cell CellFrom { get; set; }
        public Cell CellTo { get; set; }

        public Move(Func<string, Cell> getCell)
        {
            this.getCell = getCell;
        }
        internal void Parse(string algebraicCoord)
        {
            if (algebraicCoord == null)
            {
                IsValid = false;
                return;
            }
            if (algebraicCoord.Length < 2)
            {
                IsValid = false;
                return;
            }

            var lastTwoLetters = algebraicCoord.Substring(algebraicCoord.Length - 2);
            var cell = this.getCell(lastTwoLetters);
            if (cell == null)
            {
                IsValid = false;
            }

            if (cell.CurrentPiece != null)
            {
                IsValid = false;
                return;
            }

            if (algebraicCoord[1] == '4')
            {
                var whiteInitialCell = getCell(algebraicCoord[0] + '2'.ToString());
                if (whiteInitialCell == null || whiteInitialCell.CurrentPiece == null)
                {
                    IsValid = false;
                    return;
                }
            }


            if (algebraicCoord.Length > 2)
            {
                char pieceLetter = algebraicCoord[0];
                if (pieceLetter != 'B' && pieceLetter != 'K' &&
                    pieceLetter != 'N' && pieceLetter != 'R' && pieceLetter != 'Q')
                {
                    return;
                }



            }
            return;
        }
    }
}