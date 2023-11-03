namespace LittleTownBoardGenerator.Core
{
    public static class BoardGenerator
    {
        private static readonly Random Random = new Random();

        public async static Task<Board> Generate() => await Generate(new BoardGenerationConfiguration()).ConfigureAwait(false);

        public async static Task<Board> Generate(BoardGenerationConfiguration configuration)
        {
            var board = new Board(configuration.Width, configuration.Height);
            var resources = GetResourceList(configuration);
            var positions = GetPositions(configuration);

            var isValid = await FillUpBoard(board, resources, positions, configuration).ConfigureAwait(false);

            if (!isValid)
                return null;

            return board;
        }

        private async static Task<bool> FillUpBoard(Board board, List<Square> resources, List<Position> positions, BoardGenerationConfiguration configuration)
        {
            if (resources.Count == 0)
                return board.IsValid(configuration, true);

            if (positions.Count == 0)
                return false;

            var resource = resources[0];
            var remainingResources = resources.Skip(1).ToList();
            var position = positions[0];
            var remainingPositions = positions.Skip(1).ToList();

            board.Squares[position.X, position.Y] = resource;

            if (board.IsValid(configuration, false))
                return await FillUpBoard(board, remainingResources, remainingPositions, configuration).ConfigureAwait(false);

            board.Squares[position.X, position.Y] = Square.Nothing;

            return await FillUpBoard(board, resources, remainingPositions, configuration).ConfigureAwait(false);
        }

        private static List<Position> GetPositions(BoardGenerationConfiguration configuration)
        {
            var positions = new List<Position>();
            for (int i = 0; i < configuration.Width; i++)
                for (int j = 0; j < configuration.Height; j++)
                    positions.Add(new Position(i, j));

            return ShufflePositions(positions);
        }

        private static List<Position> ShufflePositions(List<Position> positions)
        {
            var n = positions.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Next(n + 1);
                var value = positions[k];
                positions[k] = positions[n];
                positions[n] = value;
            }
            return positions;
        }

        private static List<Square> GetResourceList(BoardGenerationConfiguration configuration)
        {
            var resources = new List<Square>();

            resources.AddRange(Enumerable.Repeat(Square.Mountain, configuration.Mountains));
            resources.AddRange(Enumerable.Repeat(Square.Lake, configuration.Lakes));
            resources.AddRange(Enumerable.Repeat(Square.Wood, configuration.Woods));

            return resources;
        }
    }
}