﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalisburyChessEngine.Board;
using SalisburyChessEngine.Board.Positions;

namespace SalisburyChessEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Salisbury Chess Engine v1.0";
            InitText(args);

            List<string> command = args.ToList();
            while (!command.Contains("-q")) 
            {
                if (command.Contains("-help"))
                {
                    DisplayHelp();
                    continue;
                }

                var white = command.IndexOf("-white");
                var black = command.IndexOf("-black");
                
                if (white == -1)
                {
                    Console.WriteLine("-white (human OR ai) required. /help for all commands");
                    command = Console.ReadLine().Split(' ').ToList();
                    continue;
                }
                if (black == -1)
                {
                    Console.WriteLine("-black (human OR ai) required. /help for all commands");
                    command = Console.ReadLine().Split(' ').ToList();
                    continue;
                }
                var whiteMode = command[white + 1].ToLower();
                var blackMode = command[black + 1].ToLower();

                if (whiteMode != "human" && whiteMode != "ai")
                {
                    Console.WriteLine("The argument following -white should be (human or ai");
                    command = Console.ReadLine().Split(' ').ToList();
                    continue;
                }
                if (blackMode != "human" && blackMode != "ai")
                {
                    Console.WriteLine("The argument following -black should be (human or ai");
                    command = Console.ReadLine().Split(' ').ToList();
                    continue;
                }

                //init fen parser -- need a way to import custom board states.

                var fen = command.IndexOf("-fen");
                if (fen > -1)
                {
                    const int FEN_FIELD_LENGTH = 6;
                    var fenContents = command.GetRange(fen + 1, FEN_FIELD_LENGTH); //important that we do not ToLower here. We need the exact string contents
                    FENNotationPosition fnp = new FENNotationPosition(fenContents);
                    var game = new Game(whiteMode, blackMode, fnp);
                    return;
                }
                else
                {
                    var game = new Game(whiteMode, blackMode);
                }
             
                
                command = Console.ReadLine().Split(' ').ToList();

            } 
            
        }

        static void InitText(string[] args)
        {
            Console.WriteLine("__________");
            Console.WriteLine("Welcome to the Salisbury Chess Engine");
            Console.WriteLine("__________");

            if (args.Length == 0)
            {
                Console.WriteLine("Enter a command.");
            }
        }

        static void DisplayHelp()
        {
            Console.WriteLine("__________");
            Console.WriteLine("Example command:");
            Console.WriteLine("-white human -black ai");
            Console.WriteLine("__________");
        }
    }
}
