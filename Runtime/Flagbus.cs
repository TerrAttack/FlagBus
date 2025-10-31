namespace Utils.Flags
{
    public static class FlagBus<T> where T : IFlag
    {
        public static bool IsSet { get; private set; }

        public static void Set(bool value)
        {
            IsSet = value;
        }

        public static void Clear()
        {
            IsSet = false;
        }
    }
}
