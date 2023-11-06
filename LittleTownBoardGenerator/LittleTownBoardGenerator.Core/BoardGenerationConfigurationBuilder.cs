namespace LittleTownBoardGenerator.Core;

public static class BoardGenerationConfigurationBuilder
{
    private static readonly Random _random = new();
    
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
            .Where(configuration=> configuration.IsValid(generalConfiguration))
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
}
