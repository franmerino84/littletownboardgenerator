using LittleTownBoardGenerator.Console;

namespace LittleTownBoardGenerator.Core;

public static class BoardGenerationConfigurationBuilder
{
    private static readonly Random Random = new Random();

    public static BoardGenerationConfiguration Build(BoardGenerationConfigurationOptions options)
    {
        return new BoardGenerationConfiguration
        {
            Width = Random.Next(options.MinWidth, options.MaxWidth + 1),
            Height = Random.Next(options.MinHeight, options.MaxHeight + 1),
            Mountains = Random.Next(options.MinMountains, options.MaxMountains + 1),
            Lakes = Random.Next(options.MinLakes, options.MaxLakes + 1),
            Woods = Random.Next(options.MinWoods, options.MaxWoods + 1),
            MinSurroundingResources = options.MinSurroundingResources,
            MaxSurroundingResources = options.MaxSurroundingResources,
            AllowInaccessibleResources = GetAllowInaccessibleResources(options)
        };
    }

    private static bool GetAllowInaccessibleResources(BoardGenerationConfigurationOptions options)
    {
        if (options.AllowInaccessibleResourcesCanBeFalse && !options.AllowInaccessibleResourcesCanBeTrue)
            return false;

        if (!options.AllowInaccessibleResourcesCanBeFalse && options.AllowInaccessibleResourcesCanBeTrue)
            return true;

        if (options.AllowInaccessibleResourcesCanBeFalse && options.AllowInaccessibleResourcesCanBeTrue)
            return Random.Next(2) == 0;

        throw new InvalidConfigurationException();
    }
}
