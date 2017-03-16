using System;
using System.Collections.Generic;
using System.Linq;
using SalisburyChessEngine.Pieces;
using SalisburyChessEngine.Moves;
using SalisburyChessEngine.Utilities;

namespace SalisburyChessEngine.Board
{
    public partial class ChessBoard : List<List<Cell>>
    {
        public ValidBoardMove CheckingBoardMove { get; set; }

        public King WhiteKing { get; set; }
        public King BlackKing { get; set; }

        public List<ValidBoardMove> BlackPiecePressure { get; set; }
        public List<ValidBoardMove> WhitePiecePressure { get; set; }
        
        public bool IsWhitesTurn { get;  set; }

        private AlgebraicNotationParser parser;
        public ChessBoard()
        {
            this.IsWhitesTurn = true;
            this.parser = new AlgebraicNotationParser(this);

            this.WhiteKing = new King(true, this.GetCell, "e1");
            this.BlackKing = new King(false, this.GetCell, "e8");

            this.WhiteKing.OnCheckCallbacks += UpdateBoard;
            this.BlackKing.OnCheckCallbacks += UpdateBoard;
            this.BlackPiecePressure = new List<ValidBoardMove>();
            this.WhitePiecePressure = new List<ValidBoardMove>();


            this.InitializeBoard();
            this.UpdateBoardState();
        }
        public void UpdateBoardState()
        {
            this.BlackPiecePressure = new List<ValidBoardMove>();
            this.WhitePiecePressure = new List<ValidBoardMove>();
            this.ResetPieces();
            this.UpdateBoard();
            this.DetermineTeamPressure();
            this.DetermineKingMoves();
            this.CheckIfKingChecked();
            this.DisplayBoard();
        }

        public void InitializeBoard()
        {
            InitializeCells();
            PlacePiecesOnBoard();
        }
        private void InitializeCells()
        {
            for (var i = BoardProperties.Rows; i >= 1; i--)
            {
                var row = new List<Cell>();
                for (var j = 1; j <= BoardProperties.Columns; j++)
                {
                    row.Add(new Cell(i, j));
                }
                Add(row);
            }
        }

        internal void ReplacePiece(Move move)
        {
            if (move.IsKingSideCastle)
            {
                if (move.IsWhitesTurn)
                {
                    var kingCellFrom = this.GetCell("e1");
                    var kingCellTo = this.GetCell("g1");
                    var rookCellFrom = this.GetCell("h1");
                    var rookCellTo = this.GetCell("f1");

                    this.ReplacePieceBase(kingCellFrom, kingCellTo);
                    this.ReplacePieceBase(rookCellFrom, rookCellTo);
                }
                else
                {
                    var kingCellFrom = this.GetCell("e8");
                    var kingCellTo = this.GetCell("g8");
                    var rookCellFrom = this.GetCell("h8");
                    var rookCellTo = this.GetCell("f8");

                    this.ReplacePieceBase(kingCellFrom, kingCellTo);
                    this.ReplacePieceBase(rookCellFrom, rookCellTo);
                }
                return;
            }
            else if (move.IsQueenSideCastle)
            {
                if (move.IsWhitesTurn)
                {
                    var kingCellFrom = this.GetCell("e1");
                    var kingCellTo = this.GetCell("c1");
                    var rookCellFrom = this.GetCell("a1");
                    var rookCellTo = this.GetCell("d1");

                    this.ReplacePieceBase(kingCellFrom, kingCellTo);
                    this.ReplacePieceBase(rookCellFrom, rookCellTo);
                }
                else
                {
                    var kingCellFrom = this.GetCell("e8");
                    var kingCellTo = this.GetCell("c8");
                    var rookCellFrom = this.GetCell("a8");
                    var rookCellTo = this.GetCell("d8");

                    this.ReplacePieceBase(kingCellFrom, kingCellTo);
                    this.ReplacePieceBase(rookCellFrom, rookCellTo);
                }
                return;
            }
           else
            {
                this.ReplacePieceBase(move.CellFrom, move.CellTo);
            }
        }

        private void ReplacePieceBase(Cell cellFrom, Cell cellTo)
        {
            var currentPiece = cellFrom.CurrentPiece;
            if (currentPiece.TypeOfPiece == PieceBase.PieceType.Rook)
            {
                var rook = (Rook)currentPiece;
                rook.HasMoved = true;
            }
            else if (currentPiece.TypeOfPiece == PieceBase.PieceType.King)
            {
                var king = (King)currentPiece;
                king.HasMoved = true;
            }
            cellTo.CurrentPiece = currentPiece;
            cellTo.CurrentPiece.CurrentCoordinates = cellTo.Coordinates;
            cellFrom.CurrentPiece = null;
        }

        internal void Rollback(Move move)
        {
            var currentPiece = move.CellTo.CurrentPiece;
            move.CellFrom.CurrentPiece = currentPiece;
            move.CellFrom.CurrentPiece.CurrentCoordinates = move.CellFrom.Coordinates;
            move.CellTo.CurrentPiece = null;
        }

        public void ResetPieces()
        {
            ExecuteCellLevelFunction(ResetPiece);
        }

        public void ResetPiece(Cell cell)
        {
            if (Cell.HasPiece(cell))
            {
                var piece = Cell.GetPiece(cell);
                piece.ValidMovesSet = false;
            }
        }
        internal void CheckIfKingChecked()
        {
            int whitePiecePressureIndex = this.WhitePiecePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(this.BlackKing.CurrentCoordinates);
            int blackPiecePressureIndex = this.BlackPiecePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(this.WhiteKing.CurrentCoordinates);
            if (whitePiecePressureIndex > -1)
            {
                //black king checked
                this.CheckingBoardMove = this.WhitePiecePressure[whitePiecePressureIndex];
                this.BlackKing.IsChecked = true;
            }

            else if (blackPiecePressureIndex > -1)
            {
                //white king checked
                this.CheckingBoardMove = this.BlackPiecePressure[blackPiecePressureIndex];
                this.WhiteKing.IsChecked = true;
            }
            else
            {
                this.BlackKing.IsChecked = false;
                this.WhiteKing.IsChecked = false;
            }
        }

        private void PlacePiecesOnBoard()
        {
            //white back rank
            this.GetCell("a1").CurrentPiece = new Rook(true, this.GetCell, "a1", this.BlackKing);
            this.GetCell("b1").CurrentPiece = new Knight(true, this.GetCell, "b1");
            this.GetCell("c1").CurrentPiece = new Bishop(true, this.GetCell, "c1", this.BlackKing);
            this.GetCell("d1").CurrentPiece = new Queen(true, this.GetCell, "d1", this.BlackKing);
            this.GetCell("e1").CurrentPiece = this.WhiteKing;
            this.GetCell("f1").CurrentPiece = new Bishop(true, this.GetCell, "f1", this.BlackKing);
            this.GetCell("g1").CurrentPiece = new Knight(true, this.GetCell, "g1");
            this.GetCell("h1").CurrentPiece = new Rook(true, this.GetCell, "h1", this.BlackKing);
            //white pawn rank
            this.GetCell("a2").CurrentPiece = new Pawn(true, this.GetCell, "a2");
            this.GetCell("b2").CurrentPiece = new Pawn(true, this.GetCell, "b2");
            this.GetCell("c2").CurrentPiece = new Pawn(true, this.GetCell, "c2");
            this.GetCell("d2").CurrentPiece = new Pawn(true, this.GetCell, "d2");
            this.GetCell("e2").CurrentPiece = new Pawn(true, this.GetCell, "e2");
            this.GetCell("f2").CurrentPiece = new Pawn(true, this.GetCell, "f2");
            this.GetCell("g2").CurrentPiece = new Pawn(true, this.GetCell, "g2");
            this.GetCell("h2").CurrentPiece = new Pawn(true, this.GetCell, "h2");

            //black back rank
            this.GetCell("a8").CurrentPiece = new Rook(false, this.GetCell, "a8", this.WhiteKing);
            this.GetCell("b8").CurrentPiece = new Knight(false, this.GetCell, "b8");
            this.GetCell("c8").CurrentPiece = new Bishop(false, this.GetCell, "c8", this.WhiteKing);
            this.GetCell("d8").CurrentPiece = new Queen(false, this.GetCell, "d8", this.WhiteKing);
            this.GetCell("e8").CurrentPiece = this.BlackKing;
            this.GetCell("f8").CurrentPiece = new Bishop(false, this.GetCell, "f8", this.WhiteKing);
            this.GetCell("g8").CurrentPiece = new Knight(false, this.GetCell, "g8");
            this.GetCell("h8").CurrentPiece = new Rook(false, this.GetCell, "h8", this.WhiteKing);

            this.GetCell("a7").CurrentPiece = new Pawn(false, this.GetCell, "a7");
            this.GetCell("b7").CurrentPiece = new Pawn(false, this.GetCell, "b7");
            this.GetCell("c7").CurrentPiece = new Pawn(false, this.GetCell, "c7");
            this.GetCell("d7").CurrentPiece = new Pawn(false, this.GetCell, "d7");
            this.GetCell("e7").CurrentPiece = new Pawn(false, this.GetCell, "e7");
            this.GetCell("f7").CurrentPiece = new Pawn(false, this.GetCell, "f7");
            this.GetCell("g7").CurrentPiece = new Pawn(false, this.GetCell, "g7");
            this.GetCell("h7").CurrentPiece = new Pawn(false, this.GetCell, "h7");
        }

        public void UpdateBoard()
        {
            ExecuteCellLevelFunction(DetermineValidMovesIfNotKing);
            if (this.CheckingBoardMove != null)
            {
                this.CheckingBoardMove = null;
            }
            
        }

        public void DetermineValidMovesIfNotKing(Cell cell)
        {
            if (Cell.HasPiece(cell))
            {
                if (cell.CurrentPiece.GetType() == typeof(King))
                {
                    return;
                }
                if (!cell.CurrentPiece.ValidMovesSet || (this.WhiteKing.IsChecked || this.BlackKing.IsChecked))
                {
                    cell.CurrentPiece.CurrentCoordinates = cell.Coordinates;
                    cell.CurrentPiece.DetermineValidMoves(cell.Coordinates, this.CheckingBoardMove, null);
                }
            }
        }

        


        private void DetermineTeamPressure()
        {
            ExecuteCellLevelFunction(DetermineCellPressure);
        }

        private void DetermineCellPressure(Cell cell)
        {
            if (Cell.HasPiece(cell))
            {
                List<ValidBoardMove> piecePressureList;
                if (cell.CurrentPiece.isWhite)
                {
                    piecePressureList = WhitePiecePressure;
                }
                else
                {
                    piecePressureList = BlackPiecePressure;
                }

                List<ValidBoardMove> validMovesList;
                validMovesList = cell.CurrentPiece.PiecePressure;

                foreach (var move in validMovesList)
                {
                    if (piecePressureList.IndexOf(move) == -1)
                    {
                        piecePressureList.Add(move);
                    }
                }

            }
        }
        private void DetermineKingMoves()
        {
            ExecuteCellLevelFunction(DetermineValidMoveForKing);
        }

        public void DetermineValidMoveForKing(Cell cell)
        {
            if (Cell.HasPiece(cell))
            {
                if (cell.CurrentPiece.GetType() != typeof(King))
                {
                    return;
                }
                var king = (King)cell.CurrentPiece;
                if (king.isWhite)
                {
                    this.WhitePiecePressure = king.DetermineValidMoves(cell.Coordinates, BlackPiecePressure, WhitePiecePressure, this.CheckingBoardMove);
                }
                else
                {
                    this.BlackPiecePressure = king.DetermineValidMoves(cell.Coordinates, WhitePiecePressure, BlackPiecePressure, this.CheckingBoardMove);
                }
            }

            
        }

        //Utility method to extract a cell from the list based on coordinates
        public Cell GetCell(string coords)
        {
            //expected: 2 letters, a1-h8
            if (!DetermineIfCoordinatesMeetLengthRequirement(coords))
            {
                return null;
            }
            char columnLetter = coords[0];
            char row = coords[1];
            int parsedColumn = DetermineIfColumnLetterValid(columnLetter);
            int parsedRow = DetermineIfRowNumberValid(row);
            if (parsedColumn == -1 || parsedRow == -1)
            {
                return null;
            }

            return this[BoardProperties.Rows - parsedRow][parsedColumn - 1];
        }

        private bool DetermineIfCoordinatesMeetLengthRequirement(string coords)
        {
            if (coords.Length != 2)
            {
                return false;
            }
            return true;
        }

        private int DetermineIfRowNumberValid(char row)
        {
            if (!int.TryParse(row.ToString(), out int parsedRow))
            {
                return -1;
            }
            if (parsedRow > BoardProperties.Rows || parsedRow <= 0)
            {
                return -1;
            }
            return parsedRow;
        }

        private int DetermineIfColumnLetterValid(char columnLetter)
        {
            if (columnLetter < 'a' || columnLetter > 'h')
            {
                return -1;
            }
            if (!CellProperties.ColumnLettersMappedToNumbers.TryGetValue(columnLetter, out int parsedColumn))
            {
                return -1;
            }
            return parsedColumn;
        }
        public void DisplayBoard()
        {
            for (var i = BoardProperties.Rows; i > 0; i--)
            {
                DisplayRows(i);
            }
        }
        public void DisplayRows(int rowNumber)
        {
            Console.Write(rowNumber + " ");
            DisplayCellsInRow(rowNumber);
            Console.WriteLine(" ");
        }
        public void DisplayCellsInRow(int rowNumber)
        {
            for (var j = 1; j <= BoardProperties.Columns; j++)
            {
                var cell = this[BoardProperties.Rows - rowNumber][j - 1];
                if (Cell.HasPiece(cell))
                {
                    var piece = Cell.GetPiece(cell);
                    if (piece.isWhite)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                }
                Console.Write(this[BoardProperties.Rows - rowNumber][j - 1].ToString() + " ");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public void DisplayFooter()
        {
            Console.WriteLine(" ");
            Console.Write("  A B C D E F G H");
        }
        internal bool TryMovePiece(string algebraicCoord, out Move move)
        {
            move = parser.Parse(algebraicCoord, this.IsWhitesTurn);

            if (move.IsValid)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }


    
   
}