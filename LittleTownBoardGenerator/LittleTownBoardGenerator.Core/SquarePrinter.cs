namespace LittleTownBoardGenerator.Core
{
    public static class SquarePrinter
    {
        public static string Print(Square square)
        {
            return square switch
            {
                Square.Nothing => "~",
                Square.Lake => "L",
                Square.Wood => "W",
                Square.Mountain => "M",
                _ => throw new NotImplementedException()
            };
        }
    }
}
