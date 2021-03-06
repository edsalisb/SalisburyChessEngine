﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalisburyChessEngine.Board
{
    public class BoardProperties
    {
        public static int Rows { get; } = 8;
        public static int Columns { get; } = 8;
        public static Dictionary<int, char> ColumnNumbersMappedToLetters { get; } =
        new Dictionary<int, char>()
        {
           { 1, 'a'},
           { 2, 'b'},
           { 3, 'c'},
           { 4, 'd'},
           { 5, 'e'},
           { 6, 'f'},
           { 7, 'g'},
           { 8, 'h'},
        };
        public static Dictionary<char, int> ColumnLettersMappedToNumbers { get; } =
        new Dictionary<char, int>()
        {
           { 'a', 1},
           { 'b', 2},
           { 'c', 3},
           { 'd', 4},
           { 'e', 5},
           { 'f', 6},
           { 'g', 7},
           { 'h', 8},
        };
    }
}
