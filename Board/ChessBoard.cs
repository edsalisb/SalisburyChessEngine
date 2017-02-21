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

        public List<ValidBoardMove> blackPiecePressure { get; set; }
        public List<ValidBoardMove> whitePiecePressure { get; set; }
   
        public bool isWhitesTurn { get;  set; }

        private AlgebraicNotationParser parser;
        public ChessBoard()
        {
            this.isWhitesTurn = true;
            this.parser = new AlgebraicNotationParser(this);

            this.WhiteKing = new King(true, this.getCell, "e1");
            this.BlackKing = new King(false, this.getCell, "e8");

            this.WhiteKing.registerOnCheckCallback(UpdateBoard);
            this.BlackKing.registerOnCheckCallback(UpdateBoard);
            this.blackPiecePressure = new List<ValidBoardMove>();
            this.whitePiecePressure = new List<ValidBoardMove>();


            this.initializeBoard();
            this.UpdateBoardState();
        }
        public void UpdateBoardState()
        {
            this.blackPiecePressure = new List<ValidBoardMove>();
            this.whitePiecePressure = new List<ValidBoardMove>();
            this.UpdateBoard();
            this.determineTeamPressure();
            this.determineKingMoves();
            this.CheckIfKingChecked();
            this.displayBoard();
        }

        public void initializeBoard()
        {
            initializeCells();
            placePiecesOnBoard();
        }
        private void initializeCells()
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

        internal void replacePiece(Move move)
        {
            var currentPiece = move.CellFrom.CurrentPiece;
            move.CellTo.CurrentPiece = currentPiece;
            move.CellTo.CurrentPiece.CurrentCoordinates = move.CellTo.Coordinates;
            move.CellFrom.CurrentPiece = null;
        }

        internal void rollback(Move move)
        {
            var currentPiece = move.CellTo.CurrentPiece;
            move.CellFrom.CurrentPiece = currentPiece;
            move.CellFrom.CurrentPiece.CurrentCoordinates = move.CellFrom.Coordinates;
            move.CellTo.CurrentPiece = null;
        }

        internal void CheckIfKingChecked()
        {
            int whitePiecePressureIndex = this.whitePiecePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(this.BlackKing.CurrentCoordinates);
            int blackPiecePressureIndex = this.blackPiecePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(this.WhiteKing.CurrentCoordinates);
            if (whitePiecePressureIndex > -1)
            {
                //black king checked
                this.CheckingBoardMove = this.whitePiecePressure[whitePiecePressureIndex];
                this.BlackKing.IsChecked = true;
            }

            else if (blackPiecePressureIndex > -1)
            {
                //white king checked
                this.CheckingBoardMove = this.blackPiecePressure[blackPiecePressureIndex];
                this.WhiteKing.IsChecked = true;
            }
            else
            {
                this.BlackKing.IsChecked = false;
                this.WhiteKing.IsChecked = false;
            }
        }

        private void placePiecesOnBoard()
        {
            //white back rank
            this.getCell("a1").CurrentPiece = new Rook(true, this.getCell, "a1");
            this.getCell("b1").CurrentPiece = new Knight(true, this.getCell, "b1");
            this.getCell("c1").CurrentPiece = new Bishop(true, this.getCell, "c1");
            this.getCell("d1").CurrentPiece = new Queen(true, this.getCell, "d1");
            this.getCell("e1").CurrentPiece = this.WhiteKing;
            this.getCell("f1").CurrentPiece = new Bishop(true, this.getCell, "f1");
            this.getCell("g1").CurrentPiece = new Knight(true, this.getCell, "g1");
            this.getCell("h1").CurrentPiece = new Rook(true, this.getCell, "h1");
            //white pawn rank
            this.getCell("a2").CurrentPiece = new Pawn(true, this.getCell, "a2");
            this.getCell("b2").CurrentPiece = new Pawn(true, this.getCell, "b2");
            this.getCell("c2").CurrentPiece = new Pawn(true, this.getCell, "c2");
            this.getCell("d2").CurrentPiece = new Pawn(true, this.getCell, "d2");
            this.getCell("e2").CurrentPiece = new Pawn(true, this.getCell, "e2");
            this.getCell("f2").CurrentPiece = new Pawn(true, this.getCell, "f2");
            this.getCell("g2").CurrentPiece = new Pawn(true, this.getCell, "g2");
            this.getCell("h2").CurrentPiece = new Pawn(true, this.getCell, "h2");

            //black back rank
            this.getCell("a8").CurrentPiece = new Rook(false, this.getCell, "a8");
            this.getCell("b8").CurrentPiece = new Knight(false, this.getCell, "b8");
            this.getCell("c8").CurrentPiece = new Bishop(false, this.getCell, "c8");
            this.getCell("d8").CurrentPiece = new Queen(false, this.getCell, "d8");
            this.getCell("e8").CurrentPiece = this.BlackKing;
            this.getCell("f8").CurrentPiece = new Bishop(false, this.getCell, "f8");
            this.getCell("g8").CurrentPiece = new Knight(false, this.getCell, "g8");
            this.getCell("h8").CurrentPiece = new Rook(false, this.getCell, "h8");

            this.getCell("a7").CurrentPiece = new Pawn(false, this.getCell, "a7");
            this.getCell("b7").CurrentPiece = new Pawn(false, this.getCell, "b7");
            this.getCell("c7").CurrentPiece = new Pawn(false, this.getCell, "c7");
            this.getCell("d7").CurrentPiece = new Pawn(false, this.getCell, "d7");
            this.getCell("e7").CurrentPiece = new Pawn(false, this.getCell, "e7");
            this.getCell("f7").CurrentPiece = new Pawn(false, this.getCell, "f7");
            this.getCell("g7").CurrentPiece = new Pawn(false, this.getCell, "g7");
            this.getCell("h7").CurrentPiece = new Pawn(false, this.getCell, "h7");
        }

        public void UpdateBoard()
        {
            executeCellLevelFunction(DetermineValidMovesIfNotKing);
            if (this.CheckingBoardMove != null)
            {
                this.CheckingBoardMove = null;
            }
            
        }
        public void DetermineValidMovesIfNotKing(Cell cell)
        {
            if (cell.CurrentPiece != null)
            {
                if (cell.CurrentPiece.GetType() == typeof(King))
                {
                    return;
                }
                cell.CurrentPiece.CurrentCoordinates = cell.Coordinates;
                
                cell.CurrentPiece.determineValidMoves(cell.Coordinates, this.CheckingBoardMove);
            }
        }

      
        private void determineTeamPressure()
        {
            executeCellLevelFunction(determineCellPressure);
        }

        private void determineCellPressure(Cell cell)
        {
            if (cell.CurrentPiece != null)
            {
                List<ValidBoardMove> piecePressureList;
                if (cell.CurrentPiece.isWhite)
                {
                    piecePressureList = whitePiecePressure;
                }
                else
                {
                    piecePressureList = blackPiecePressure;
                }

                List<ValidBoardMove> validMovesList;
                if (cell.CurrentPiece.GetType() == typeof(Pawn))
                {
                    var pawn = (Pawn)cell.CurrentPiece;
                    validMovesList = pawn.piecePressureCoords;
                }
                else
                {
                    validMovesList = cell.CurrentPiece.ValidMoves;
                }

                foreach (var move in validMovesList)
                {
                    if (piecePressureList.IndexOf(move) == -1)
                    {
                        piecePressureList.Add(move);
                    }
                }

            }
        }
        private void determineKingMoves()
        {
            executeCellLevelFunction(DetermineValidMoveForKing);
        }

        public void DetermineValidMoveForKing(Cell cell)
        {
            if (cell.CurrentPiece != null)
            {
                if (cell.CurrentPiece.GetType() != typeof(King))
                {
                    return;
                }
                var king = (King)cell.CurrentPiece;
                if (king.isWhite)
                {
                    this.whitePiecePressure = king.determineValidMoves(cell.Coordinates, blackPiecePressure, whitePiecePressure, this.CheckingBoardMove);
                }
                else
                {
                    this.blackPiecePressure = king.determineValidMoves(cell.Coordinates, whitePiecePressure, blackPiecePressure, this.CheckingBoardMove);
                }
            }

            
        }

        //Utility method to extract a cell from the list based on coordinates
        public Cell getCell(string coords)
        {
            //expected: 2 letters, a1-h8
            if (!determineIfCoordinatesMeetLengthRequirement(coords))
            {
                return null;
            }
            char columnLetter = coords[0];
            char row = coords[1];
            int parsedColumn = determineIfColumnLetterValid(columnLetter);
            int parsedRow = determineIfRowNumberValid(row);
            if (parsedColumn == -1 || parsedRow == -1)
            {
                return null;
            }

            return this[BoardProperties.Rows - parsedRow][parsedColumn - 1];
        }

        private bool determineIfCoordinatesMeetLengthRequirement(string coords)
        {
            if (coords.Length != 2)
            {
                return false;
            }
            return true;
        }

        private int determineIfRowNumberValid(char row)
        {
            int parsedRow;
            if (!int.TryParse(row.ToString(), out parsedRow))
            {
                return -1;
            }
            if (parsedRow > BoardProperties.Rows || parsedRow <= 0)
            {
                return -1;
            }
            return parsedRow;
        }

        private int determineIfColumnLetterValid(char columnLetter)
        {
            if (columnLetter < 'a' || columnLetter > 'h')
            {
                return -1;
            }
            int parsedColumn;
            if (!CellProperties.ColumnLettersMappedToNumbers.TryGetValue(columnLetter, out parsedColumn))
            {
                return -1;
            }
            return parsedColumn;
        }
        public void displayBoard()
        {
            for (var i = BoardProperties.Rows; i > 0; i--)
            {
                displayRows(i);
            }
        }
        public void displayRows(int rowNumber)
        {
            Console.Write(rowNumber + " ");
            displayCellsInRow(rowNumber);
            Console.WriteLine(" ");
        }
        public void displayCellsInRow(int rowNumber)
        {
            for (var j = 1; j <= BoardProperties.Columns; j++)
            {
                Console.Write(this[BoardProperties.Rows - rowNumber][j - 1].ToString() + " ");
            }
        }
        public void displayFooter()
        {
            Console.WriteLine(" ");
            Console.Write("  A B C D E F G H");
        }
        internal bool TryMovePiece(string algebraicCoord, out Move move)
        {
            move = parser.Parse(algebraicCoord, this.isWhitesTurn);

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