namespace Smoker.Services
{
    public class ValueStore
    {
        private static readonly Lazy<ValueStore> _instance = new(() => new ValueStore());
        private readonly Dictionary<string, string> _store = new();

        public static ValueStore Instance => _instance.Value;

        private ValueStore() { }

        public static void Set(string key, string value) =>
            Instance._store[key] = value;

        public string? Get(string key) =>
            _store.TryGetValue(key, out var value) ? value : null;
    }
}
