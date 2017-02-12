using System;
using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    internal class Queen : PieceBase
    {
        public pieceType piece;
        private Func<string, Cell> getCell;

        public Queen(bool isWhite, Func<string, Cell> getCell) : base(isWhite)
        {
            this.piece = pieceType.Queen;
            this.getCell = getCell;
        }

        
        public override string ToString()
        {
            return "Q";
        }

        public override void determineValidMoves(string coords)
        {
            throw new NotImplementedException();
        }
    }
}