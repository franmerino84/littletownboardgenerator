namespace LittleTownBoardGenerator.Core
{
    public static class Shuffler
    {
        private static readonly Random _random = new();

        public static List<T> Shuffle<T>(List<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = _random.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
            return list;
        }

    }
}
