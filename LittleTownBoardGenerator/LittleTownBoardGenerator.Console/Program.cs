// See https://aka.ms/new-console-template for more information

using LittleTownBoardGenerator.Core;

var generalConfiguration = new BoardGenerationGeneralConfiguration(9,9,6,6,4,4,5,5,6,6,2,3,15,15,true,false);


var board = await BoardGenerator.Generate(BoardGenerationConfigurationBuilder.BuildConfigurations(generalConfiguration)[0], CancellationToken.None);
Console.WriteLine(board);




