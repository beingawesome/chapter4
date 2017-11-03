namespace Chapter4.EventStore
{
    internal class EventType
    {
        public EventType(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
