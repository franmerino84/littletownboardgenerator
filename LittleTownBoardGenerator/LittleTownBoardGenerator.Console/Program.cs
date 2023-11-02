// See https://aka.ms/new-console-template for more information

using LittleTownBoardGenerator.Console;
using LittleTownBoardGenerator.Core;

var board = BoardGenerator.Generate(new BoardGenerationConfiguration());
Console.WriteLine(board);




