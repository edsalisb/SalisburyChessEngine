using System;
using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    internal class Bishop : PieceBase
    {
        private Func<string, Cell> getCell;
        public pieceType piece;

        public Bishop(bool isWhite, Func<string, Cell> getCell): base(isWhite)
        {
            this.piece = pieceType.Bishop;
            this.getCell = getCell;
        }
        

        public override void determineValidMoves(string coords)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "B";
        }
    }
}