﻿using System;
using System.Collections.Generic;
using SalisburyChessEngine.Board;

namespace SalisburyChessEngine.Pieces
{
    public class Knight : PieceBase
    {
   
        public Knight(bool isWhite, Func<string, Cell> getCell, string coordinates): base(isWhite, coordinates, getCell)
        {
            this.TypeOfPiece = pieceType.Knight;
        }
        public override void determineValidMoves(string coords, ValidBoardMove checkingMove)
        {
            ValidMoves = new List<ValidBoardMove>();
            PiecePressure = new List<ValidBoardMove>();
            addToValidMoves(coords);

            this.FilterMovesIfChecked(checkingMove);

        }
        public void determineValidMoves(string coords, List<ValidBoardMove> pinnedMoves)
        {
            ValidMoves = new List<ValidBoardMove>();
            PiecePressure = new List<ValidBoardMove>();
            addToValidMoves(coords);
        }

        public override void addToValidMoves(string coords)
        {
            var currentCell = this.getCell(coords);

            var twoColumnsLeft = getColumnLetter(currentCell, -2);
            var oneColumnLeft = getColumnLetter(currentCell, -1);
            var oneColumnRight = getColumnLetter(currentCell, 1);
            var twoColumnsRight = getColumnLetter(currentCell, 1);

            //8 moves knight can make
            var twoUpOneLeftCell = this.getCell(oneColumnLeft.ToString() + (currentCell.Row + 2));
            var twoUpOneRightCell = this.getCell(oneColumnRight.ToString() + (currentCell.Row + 2));
            var oneUpTwoRightCell = this.getCell(twoColumnsRight.ToString() + (currentCell.Row + 1));
            var oneDownTwoRightCell = this.getCell(twoColumnsRight.ToString() + (currentCell.Row - 1));
            var twoDownOneRightCell = this.getCell(oneColumnRight.ToString() + (currentCell.Row - 2));
            var twoDownOneLeftCell = this.getCell(oneColumnLeft.ToString() + (currentCell.Row - 2));
            var oneDownTwoLeftCell = this.getCell(twoColumnsLeft.ToString() + (currentCell.Row - 1));
            var oneUpTwoLeftCell = this.getCell(twoColumnsLeft.ToString() + (currentCell.Row + 1));


            if (cellIsValidForPiece(currentCell, twoUpOneLeftCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, twoUpOneLeftCell.Coordinates, ValidBoardMove.movePath.Invalid, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, twoUpOneRightCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, twoUpOneRightCell.Coordinates, ValidBoardMove.movePath.Invalid, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, oneUpTwoRightCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, oneUpTwoRightCell.Coordinates, ValidBoardMove.movePath.Invalid, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, oneDownTwoRightCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, oneDownTwoRightCell.Coordinates, ValidBoardMove.movePath.Invalid, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, twoDownOneRightCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, twoDownOneRightCell.Coordinates, ValidBoardMove.movePath.Invalid, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, twoDownOneLeftCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, twoDownOneLeftCell.Coordinates, ValidBoardMove.movePath.Invalid, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, oneDownTwoLeftCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, oneDownTwoLeftCell.Coordinates, ValidBoardMove.movePath.Invalid, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, oneUpTwoLeftCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, oneUpTwoLeftCell.Coordinates, ValidBoardMove.movePath.Invalid, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            this.ValidMovesSet = true;
        }

        public override string ToString()
        {
            return "N";
        }
    }
}