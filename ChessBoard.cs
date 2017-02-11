using System;
using System.Collections.Generic;
using System.Linq;
using SalisburyChessEngine.Pieces;

namespace SalisburyChessEngine
{
    internal class ChessBoard
    {
        public int Rows { get; } = 8;
        public int Columns { get; } = 8;
        public List<List<Cell>> Board { get; set; }
        public bool gameEnded { get; set; }
        public AlgebraicNotationParser parser {get; set;}
        public ChessBoard()
        {
            Board = new List<List<Cell>>();
            this.initializeBoard();
            this.displayBoard();
        }

        public void initializeBoard()
        {
            initializeCells();
            placesPiecesOnBoard();
        }
        private void initializeCells()
        {
            for (var i = Rows; i >= 1; i--)
            {
                var row = new List<Cell>();
                for (var j = 1; j <= Columns; j++)
                {
                    row.Add(new Cell(i, j));
                }
                Board.Add(row);
            }
        }

        private void placesPiecesOnBoard()
        {
            //white back rank
            this.getCell("a1").CurrentPiece = new Rook(true);
            this.getCell("b1").CurrentPiece = new Knight(true);
            this.getCell("c1").CurrentPiece = new Bishop(true);
            this.getCell("d1").CurrentPiece = new Queen(true);
            this.getCell("e1").CurrentPiece = new King(true);
            this.getCell("f1").CurrentPiece = new Bishop(true);
            this.getCell("g1").CurrentPiece = new Knight(true);
            this.getCell("h1").CurrentPiece = new Rook(true);
            //white pawn rank
            this.getCell("a2").CurrentPiece = new Pawn(true);
            this.getCell("b2").CurrentPiece = new Pawn(true);
            this.getCell("c2").CurrentPiece = new Pawn(true);
            this.getCell("d2").CurrentPiece = new Pawn(true);
            this.getCell("e2").CurrentPiece = new Pawn(true);
            this.getCell("f2").CurrentPiece = new Pawn(true);
            this.getCell("g2").CurrentPiece = new Pawn(true);
            this.getCell("h2").CurrentPiece = new Pawn(true);

            //black back rank
            this.getCell("a8").CurrentPiece = new Rook(false);
            this.getCell("b8").CurrentPiece = new Knight(false);
            this.getCell("c8").CurrentPiece = new Bishop(false);
            this.getCell("d8").CurrentPiece = new Queen(false);
            this.getCell("e8").CurrentPiece = new King(false);
            this.getCell("f8").CurrentPiece = new Bishop(false);
            this.getCell("g8").CurrentPiece = new Knight(false);
            this.getCell("h8").CurrentPiece = new Rook(false);

            this.getCell("a7").CurrentPiece = new Pawn(false);
            this.getCell("b7").CurrentPiece = new Pawn(false);
            this.getCell("c7").CurrentPiece = new Pawn(false);
            this.getCell("d7").CurrentPiece = new Pawn(false);
            this.getCell("e7").CurrentPiece = new Pawn(false);
            this.getCell("f7").CurrentPiece = new Pawn(false);
            this.getCell("g7").CurrentPiece = new Pawn(false);
            this.getCell("h7").CurrentPiece = new Pawn(false);
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
            return Board[Rows - parsedRow][parsedColumn - 1];
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
            return parsedRow;
        }

        private int determineIfColumnLetterValid(char columnLetter)
        {
            if (columnLetter < 'a' || columnLetter > 'h')
            {
                return -1;
            }
            int parsedColumn;
            if (!Cell.ColumnLettersMappedToNumbers.TryGetValue(columnLetter, out parsedColumn))
            {
                return -1;
            }
            return parsedColumn;
        }
        public void displayBoard()
        {
            for (var i = Rows; i > 0; i--)
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
            for (var j = 1; j <= Columns; j++)
            {
                Console.Write(Board[Rows - rowNumber][j - 1].ToString() + " ");
            }
        }
        public void displayFooter()
        {
            Console.WriteLine(" ");
            Console.Write("  A B C D E F G H");
        }
        internal bool TryMovePiece(string algebraicCoord)
        {
            if (algebraicCoord == null)
            {
                return false;
            }

            var lastTwoLetters = algebraicCoord.Substring(algebraicCoord.Length - 2);
            var cell = this.getCell(lastTwoLetters);
            if (cell == null)
            {
                return false;
            }

            if (cell.CurrentPiece != null)
            {
                return false;
            }
            if (algebraicCoord.Length > 2)
            {
                char pieceLetter = algebraicCoord[0];
                if (pieceLetter != 'B' && pieceLetter != 'K' && 
                    pieceLetter != 'N' && pieceLetter != 'R' && pieceLetter != 'Q')
                {
                    return false;
                }
                
            }
            return true;
        }
    }
}