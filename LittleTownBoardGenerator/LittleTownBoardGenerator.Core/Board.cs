using LittleTownBoardGenerator.Console;
using System.Text;

namespace LittleTownBoardGenerator.Core;

public class Board
{
    public Board() : this(9, 6) { }

    public Board(int width, int height)
    {
        Width = width;
        Height = height;
        //initialize Squares to Nothing
        Squares = new Square[width, height];
    }
    public int Width { get; }
    public int Height { get; }
    public Square[,] Squares { get; }
    public override string ToString()
    {
        var builder = new StringBuilder();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                builder.Append(SquarePrinter.Print(Squares[x, y]));
            }
            builder.AppendLine();
        }
        return builder.ToString();
    }

    public bool IsValid(BoardGenerationConfiguration configuration, bool considerMinSurroundings)
    {
        for (int i = 0; i < Squares.GetLength(0); i++)
            for (int j = 0; j < Squares.GetLength(1); j++)
                if (!IsValid(i, j, configuration, considerMinSurroundings))
                    return false;

        return true;
    }

    private bool IsValid(int i, int j, BoardGenerationConfiguration configuration, bool considerMinSurroundings)
    {
        if (Squares[i, j] == Square.Nothing || !configuration.AllowInaccessibleResources)
        {
            var resourcesCount = 0;
            var freeSpaces = 0;

            for (int x = i - 1; x <= i + 1; x++)
                for (int y = j - 1; y <= j + 1; y++)
                {
                    if (x >= 0 && x < Squares.GetLength(0) && y >= 0 && y < Squares.GetLength(1))
                        if (Squares[x, y] == Square.Nothing)
                            freeSpaces++;
                        else
                            resourcesCount++;
                }

            if (Squares[i, j] == Square.Nothing)
                return resourcesCount <= configuration.MaxSurroundingResources &&
                    (!considerMinSurroundings || resourcesCount >= configuration.MinSurroundingResources);

            return freeSpaces > 0;
        }

        return true;
    }
}