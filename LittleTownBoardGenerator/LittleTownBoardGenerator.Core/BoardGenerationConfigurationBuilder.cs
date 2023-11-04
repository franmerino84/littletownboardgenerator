namespace LittleTownBoardGenerator.Core;

public static class BoardGenerationConfigurationBuilder
{
    private static readonly Random _random = new();

    public static BoardGenerationConfiguration BuildConfiguration(BoardGenerationGeneralConfiguration generalConfiguration)
    {
        return new BoardGenerationConfiguration
        {
            Width = _random.Next(generalConfiguration.MinWidth, generalConfiguration.MaxWidth + 1),
            Height = _random.Next(generalConfiguration.MinHeight, generalConfiguration.MaxHeight + 1),
            Mountains = _random.Next(generalConfiguration.MinMountains, generalConfiguration.MaxMountains + 1),
            Lakes = _random.Next(generalConfiguration.MinLakes, generalConfiguration.MaxLakes + 1),
            Woods = _random.Next(generalConfiguration.MinWoods, generalConfiguration.MaxWoods + 1),
            MinSurroundingResources = generalConfiguration.MinSurroundingResources,
            MaxSurroundingResources = generalConfiguration.MaxSurroundingResources,
            AllowInaccessibleResources = GetAllowInaccessibleResources(generalConfiguration)
        };
    }

    public static List<BoardGenerationConfiguration> BuildConfigurations(BoardGenerationGeneralConfiguration generalConfiguration)
    {
        var configurations = new List<BoardGenerationConfiguration>();
        var widths = Shuffler.Shuffle(Enumerable.Range(generalConfiguration.MinWidth, generalConfiguration.MaxWidth - generalConfiguration.MinWidth + 1).ToList());
        var heights = Shuffler.Shuffle(Enumerable.Range(generalConfiguration.MinHeight, generalConfiguration.MaxHeight - generalConfiguration.MinHeight + 1).ToList());
        var mountains = Shuffler.Shuffle(Enumerable.Range(generalConfiguration.MinMountains, generalConfiguration.MaxMountains - generalConfiguration.MinMountains + 1).ToList());
        var lakes = Shuffler.Shuffle(Enumerable.Range(generalConfiguration.MinLakes, generalConfiguration.MaxLakes - generalConfiguration.MinLakes + 1).ToList());
        var woods = Shuffler.Shuffle(Enumerable.Range(generalConfiguration.MinWoods, generalConfiguration.MaxWoods - generalConfiguration.MinWoods + 1).ToList());
        var allowInnaccessibleResources = Shuffler.Shuffle(GetAllowInaccessibleResourcesList(generalConfiguration));

        return widths.SelectMany(width =>
            heights.SelectMany(height =>
            mountains.SelectMany(mountain =>
            lakes.SelectMany(lake =>
            woods.SelectMany(wood =>
            allowInnaccessibleResources.Select(allow =>
                new BoardGenerationConfiguration(width, height, mountain, lake, wood, generalConfiguration.MinSurroundingResources, generalConfiguration.MaxSurroundingResources, allow)))))))
            .ToList();
    }

    private static List<bool> GetAllowInaccessibleResourcesList(BoardGenerationGeneralConfiguration generalConfiguration)
    {
        var list = new List<bool>();

        if (generalConfiguration.AllowInaccessibleResourcesCanBeFalse)
            list.Add(false);

        if (generalConfiguration.AllowInaccessibleResourcesCanBeTrue)
            list.Add(true);

        if (list.Count == 0)
            throw new InvalidConfigurationException();

        return list;
    }

    private static bool GetAllowInaccessibleResources(BoardGenerationGeneralConfiguration generalConfiguration)
    {
        if (generalConfiguration.AllowInaccessibleResourcesCanBeFalse && !generalConfiguration.AllowInaccessibleResourcesCanBeTrue)
            return false;

        if (!generalConfiguration.AllowInaccessibleResourcesCanBeFalse && generalConfiguration.AllowInaccessibleResourcesCanBeTrue)
            return true;

        if (generalConfiguration.AllowInaccessibleResourcesCanBeFalse && generalConfiguration.AllowInaccessibleResourcesCanBeTrue)
            return _random.Next(2) == 0;

        throw new InvalidConfigurationException();
    }
}
