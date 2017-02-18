using System;
using System.Linq;
using System.Collections.Generic;
using SalisburyChessEngine.Pieces;

namespace SalisburyChessEngine.Board
{
    public partial class ChessBoard: List<List<Cell>>
    {
        //wanted a wrapper around the double loop. Used to get rid of repetitive code
        public void executeCellLevelFunction(Action<Cell> func)
        {
            foreach (var row in this)
            {
                foreach (var cell in row)
                {
                    func(cell);
                }
            }
        }

        public List<T> executeCellLevelFunction<T>(Func<Cell, T> func)
        {
            var listOfStuff = new List<T>();
            foreach (var row in this)
            {
                foreach (var cell in row)
                {
                    var genericEntry = func(cell);
                    listOfStuff.Add(genericEntry);
                }
            }
            return listOfStuff;
        }
        public List<Pawn> FindWhitePawns()
        {
            return executeCellLevelFunction(FindPawns).Where(piecesAreWhiteAndNotNull).ToList();
        }

        public List<Pawn> FindBlackPawns()
        {
            return executeCellLevelFunction(FindPawns).Where(piecesAreBlackAndNotNull).ToList();
        }

        public List<Knight> FindWhiteKnights()
        {
            return executeCellLevelFunction(FindKnights).Where(piecesAreWhiteAndNotNull).ToList();
        }
        public List<Knight> FindBlackKnights()
        {
            return executeCellLevelFunction(FindKnights).Where(piecesAreWhiteAndNotNull).ToList();
        }
        public List<Bishop> FindWhiteBishops()
        {
            return executeCellLevelFunction(FindBishops).Where(piecesAreWhiteAndNotNull).ToList();
        }
        public List<Bishop> FindBlackBishops()
        {
            return executeCellLevelFunction(FindBishops).Where(piecesAreWhiteAndNotNull).ToList();
        }
        public List<Rook> FindWhiteRooks()
        {
            return executeCellLevelFunction(FindRooks).Where(piecesAreWhiteAndNotNull).ToList();
        }
        public List<Rook> FindBlackRooks()
        {
            return executeCellLevelFunction(FindRooks).Where(piecesAreWhiteAndNotNull).ToList();
        }
        public List<Queen> FindWhiteQueens()
        {
            return executeCellLevelFunction(FindQueens).Where(piecesAreWhiteAndNotNull).ToList();
        }
        public List<Queen> FindBlackQueens()
        {
            return executeCellLevelFunction(FindQueens).Where(piecesAreWhiteAndNotNull).ToList();
        }
        public List<King> FindWhiteKings()
        {
            return executeCellLevelFunction(FindKings).Where(piecesAreWhiteAndNotNull).ToList();
        }
        public List<King> FindBlackKings()
        {
            return executeCellLevelFunction(FindKings).Where(piecesAreWhiteAndNotNull).ToList();
        }

        private bool piecesAreWhiteAndNotNull<T>(T arg) where T : PieceBase
        {
            var piece = (PieceBase)arg;
            if (piece != null)
            {
                if (piece.isWhite)
                {
                    return true;
                }
            }
            return false;
        }

        private bool piecesAreBlackAndNotNull<T>(T arg) where T : PieceBase
        {
            var piece = (PieceBase)arg;
            if (piece != null)
            {
                if (!piece.isWhite)
                {
                    return true;
                }
            }
            return false;
        }

        public Pawn FindPawns(Cell cell)
        {
            if (cell.CurrentPiece != null)
            {
                var piece = cell.CurrentPiece;
                if (piece.TypeOfPiece == PieceBase.pieceType.Pawn)
                {
                    return (Pawn)piece;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        public Knight FindKnights(Cell cell)
        {
            if (cell.CurrentPiece != null)
            {
                var piece = cell.CurrentPiece;
                if (piece.TypeOfPiece == PieceBase.pieceType.Knight)
                {
                    return (Knight)piece;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public Bishop FindBishops(Cell cell)
        {
            if (cell.CurrentPiece != null)
            {
                var piece = cell.CurrentPiece;
                if (piece.TypeOfPiece == PieceBase.pieceType.Bishop)
                {
                    return (Bishop)piece;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public Rook FindRooks(Cell cell)
        {
            if (cell.CurrentPiece != null)
            {
                var piece = cell.CurrentPiece;
                if (piece.TypeOfPiece == PieceBase.pieceType.Rook)
                {
                    return (Rook)piece;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public Queen FindQueens(Cell cell)
        {
            if (cell.CurrentPiece != null)
            {
                var piece = cell.CurrentPiece;
                if (piece.TypeOfPiece == PieceBase.pieceType.Queen)
                {
                    return (Queen)piece;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public King FindKings(Cell cell)
        {
            if (cell.CurrentPiece != null)
            {
                var piece = cell.CurrentPiece;
                if (piece.TypeOfPiece == PieceBase.pieceType.King)
                {
                    return (King)piece;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}