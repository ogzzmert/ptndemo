    public enum ResourceType
    {
        Gold = 0,
        Wood = 1,
        Brick = 2,
        Iron = 3,
        Gem = 4
    }

    [System.Serializable]
    public class Belonging
    {
        public ResourceType resourceType;
        public int amount;
    }
