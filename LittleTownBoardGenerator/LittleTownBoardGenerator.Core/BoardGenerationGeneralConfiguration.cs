namespace LittleTownBoardGenerator.Core;

public record BoardGenerationGeneralConfiguration(
    int MinWidth = 9,
    int MaxWidth = 9,
    int MinHeight = 6,
    int MaxHeight = 6,
    int MinMountains = 4,
    int MaxMountains = 4,
    int MinLakes = 5,
    int MaxLakes = 5,
    int MinWoods = 6,
    int MaxWoods = 6,
    int MinSurroundingResources = 0,
    int MaxSurroundingResources = 3,
    bool AllowInaccessibleResourcesCanBeFalse = true,
    bool AllowInaccessibleResourcesCanBeTrue = false);
