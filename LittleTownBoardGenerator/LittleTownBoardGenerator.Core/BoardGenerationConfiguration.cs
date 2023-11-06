using System.Runtime.Versioning;

namespace LittleTownBoardGenerator.Core;

public record BoardGenerationConfiguration(
int Width = 9,
int Height = 6,
int Mountains = 4,
int Lakes = 5,
int Woods = 6,
int MinSurroundingResources = 0,
int MaxSurroundingResources = 3,
bool AllowInaccessibleResources = false)
{
    public int Resources => Mountains + Lakes + Woods;
    public bool IsValid(BoardGenerationGeneralConfiguration generalConfiguration)
    {
        return Resources >= generalConfiguration.MinResources &&
            Resources <= generalConfiguration.MaxResources &&
            Resources <= Width * Height;
    }
}
