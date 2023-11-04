// See https://aka.ms/new-console-template for more information

using LittleTownBoardGenerator.Core;

var generalConfiguration = new BoardGenerationGeneralConfiguration(5,9,6,6,4,4,5,7,6,6,1,3,true,true);

var _ = BoardGenerationConfigurationBuilder.BuildConfigurations(generalConfiguration);

var board = await BoardGenerator.Generate(BoardGenerationConfigurationBuilder.BuildConfiguration(generalConfiguration), CancellationToken.None);
Console.WriteLine(board);




