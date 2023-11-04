using System.Threading;

namespace LittleTownBoardGenerator.Core
{
    public static class BoardGenerator
    {
        public async static Task<Board?> Generate(BoardGenerationGeneralConfiguration generalConfiguration, CancellationToken cancellationToken)
        {
            var configurations = BoardGenerationConfigurationBuilder.BuildConfigurations(generalConfiguration);

            return await Generate(configurations, cancellationToken);
        }

        private static async Task<Board?> Generate(List<BoardGenerationConfiguration> configurations, CancellationToken cancellationToken)
        {
            if (configurations == null || configurations.Count == 0)
                return null;

            var board = await Generate(configurations[0], cancellationToken);

            if (board != null)
                return board;
            
            return await Generate(configurations.Skip(1).ToList(), cancellationToken);
        }

        public async static Task<Board?> Generate(CancellationToken cancellationToken) => await Generate(new BoardGenerationConfiguration(), cancellationToken).ConfigureAwait(false);

        public async static Task<Board?> Generate(BoardGenerationConfiguration configuration, CancellationToken cancellationToken)
        {
            var board = new Board(configuration.Width, configuration.Height);
            var resources = GetResourceList(configuration);
            var positions = GetPositions(configuration);

            var isValid = await FillUpBoard(board, resources, positions, configuration, cancellationToken).ConfigureAwait(false);

            if (!isValid)
                return null;

            return board;
        }

        private async static Task<bool> FillUpBoard(Board board, List<Square> resources, List<Position> positions, BoardGenerationConfiguration configuration, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new BoardGenerationCancelledException();

            if (resources.Count == 0)
                return board.IsValid(configuration, true);

            if (positions.Count == 0)
                return false;

            var resource = resources[0];
            var remainingResources = resources.Skip(1).ToList();
            var position = positions[0];
            var remainingPositions = positions.Skip(1).ToList();

            board.Squares[position.X, position.Y] = resource;

            if (board.IsValid(configuration, false) &&
                await FillUpBoard(board, remainingResources, remainingPositions, configuration, cancellationToken).ConfigureAwait(false))
                return true;

            board.Squares[position.X, position.Y] = Square.Nothing;

            return await FillUpBoard(board, resources, remainingPositions, configuration, cancellationToken).ConfigureAwait(false);
        }

        private static List<Position> GetPositions(BoardGenerationConfiguration configuration)
        {
            var positions = new List<Position>();
            for (int i = 0; i < configuration.Width; i++)
                for (int j = 0; j < configuration.Height; j++)
                    positions.Add(new Position(i, j));

            return Shuffler.Shuffle(positions);
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