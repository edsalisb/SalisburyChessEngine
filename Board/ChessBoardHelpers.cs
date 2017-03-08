using System;
using System.Linq;
using System.Collections.Generic;
using SalisburyChessEngine.Pieces;

namespace SalisburyChessEngine.Board
{
    public partial class ChessBoard: List<List<Cell>>
    {
        //wanted a wrapper around the double loop. Used to get rid of repetitive code
        public void ExecuteCellLevelFunction(Action<Cell> func)
        {
            foreach (var row in this)
            {
                foreach (var cell in row)
                {
                    func(cell);
                }
            }
        }
        public List<T> ExecuteCellLevelFunction<T>(Func<Cell, T> func)
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
            return ExecuteCellLevelFunction(FindPawns).Where(PiecesAreWhiteAndNotNull).ToList();
        }

        public List<Pawn> FindBlackPawns()
        {
            return ExecuteCellLevelFunction(FindPawns).Where(PiecesAreBlackAndNotNull).ToList();
        }

        public List<Knight> FindWhiteKnights()
        {
            return ExecuteCellLevelFunction(FindKnights).Where(PiecesAreWhiteAndNotNull).ToList();
        }
        public List<Knight> FindBlackKnights()
        {
            return ExecuteCellLevelFunction(FindKnights).Where(PiecesAreBlackAndNotNull).ToList();
        }
        public List<Bishop> FindWhiteBishops()
        {
            return ExecuteCellLevelFunction(FindBishops).Where(PiecesAreWhiteAndNotNull).ToList();
        }
        public List<Bishop> FindBlackBishops()
        {
            return ExecuteCellLevelFunction(FindBishops).Where(PiecesAreBlackAndNotNull).ToList();
        }
        public List<Rook> FindWhiteRooks()
        {
            return ExecuteCellLevelFunction(FindRooks).Where(PiecesAreWhiteAndNotNull).ToList();
        }
        public List<Rook> FindBlackRooks()
        {
            return ExecuteCellLevelFunction(FindRooks).Where(PiecesAreBlackAndNotNull).ToList();
        }
        public List<Queen> FindWhiteQueens()
        {
            return ExecuteCellLevelFunction(FindQueens).Where(PiecesAreWhiteAndNotNull).ToList();
        }
        public List<Queen> FindBlackQueens()
        {
            return ExecuteCellLevelFunction(FindQueens).Where(PiecesAreBlackAndNotNull).ToList();
        }
        public List<King> FindWhiteKings()
        {
            return ExecuteCellLevelFunction(FindKings).Where(PiecesAreWhiteAndNotNull).ToList();
        }
        public List<King> FindBlackKings()
        {
            return ExecuteCellLevelFunction(FindKings).Where(PiecesAreBlackAndNotNull).ToList();
        }

        private bool PiecesAreWhiteAndNotNull<T>(T arg) where T : PieceBase
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

        private bool PiecesAreBlackAndNotNull<T>(T arg) where T : PieceBase
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
            if (Cell.HasPiece(cell))
            {
                var piece = cell.CurrentPiece;
                if (piece.TypeOfPiece == PieceBase.PieceType.Pawn)
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
            if (Cell.HasPiece(cell))
            {
                var piece = cell.CurrentPiece;
                if (piece.TypeOfPiece == PieceBase.PieceType.Knight)
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
            if (Cell.HasPiece(cell))
            {
                var piece = cell.CurrentPiece;
                if (piece.TypeOfPiece == PieceBase.PieceType.Bishop)
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
            if (Cell.HasPiece(cell))
            {
                var piece = cell.CurrentPiece;
                if (piece.TypeOfPiece == PieceBase.PieceType.Rook)
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
            if (Cell.HasPiece(cell))
            {
                var piece = cell.CurrentPiece;
                if (piece.TypeOfPiece == PieceBase.PieceType.Queen)
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
            if (Cell.HasPiece(cell))
            {
                var piece = cell.CurrentPiece;
                if (piece.TypeOfPiece == PieceBase.PieceType.King)
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

        public King GetKingForTeam(bool isWhite)
        {
            if (isWhite)
            {
                return this.WhiteKing;
            }
            else
            {
                return this.BlackKing;
            }
        }
        public List<ValidBoardMove> GetAllMovesForTeam(bool isWhite)
        {
            IEnumerable<PieceBase> pieces;
            List<ValidBoardMove> moves = new List<ValidBoardMove>();
            if (isWhite)
            {
                pieces = ExecuteCellLevelFunction<PieceBase>(Cell.GetPiece).Where(PiecesAreWhiteAndNotNull);
            }
            else
            {
                pieces = ExecuteCellLevelFunction<PieceBase>(Cell.GetPiece).Where(PiecesAreBlackAndNotNull);
            }
            foreach(var piece in pieces)
            {
                moves.AddRange(piece.ValidMoves);
            }

            return moves;
        }

        public bool IsRookOnCell(Cell cell, out Rook rook)
        {
            if (Cell.HasPiece(cell))
            {
                var potentialKingPiece = Cell.GetPiece(cell);
                if (potentialKingPiece.TypeOfPiece != PieceBase.PieceType.Rook)
                {
                    rook = null;
                    return false;
                }
                else
                {
                    rook = (Rook)potentialKingPiece;
                    return true;
                }

            }
            rook = null;
            return false;
        }

        public bool IsKingOnCell(Cell cell, out King king)
        {
            if (Cell.HasPiece(cell))
            {
                var potentialKingPiece = Cell.GetPiece(cell);
                if (potentialKingPiece.TypeOfPiece != PieceBase.PieceType.King)
                {
                    king = null;
                    return false;
                }
                else
                {
                    king = (King)potentialKingPiece;
                    return true;
                }
                
            }
            king = null;
            return false;
        }
    }
}