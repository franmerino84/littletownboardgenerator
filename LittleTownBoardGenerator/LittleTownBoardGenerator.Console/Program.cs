// See https://aka.ms/new-console-template for more information

using LittleTownBoardGenerator.Core;

var options = new BoardGenerationConfigurationOptions();

var board = await BoardGenerator.Generate(BoardGenerationConfigurationBuilder.Build(options));
Console.WriteLine(board);




