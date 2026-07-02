namespace Zion
{
    public sealed class EventRouter
    {
        public EventBus? Sender
        {
            get;
            set
            {
                if (field == value) { return; }

                field?.Invalidated -= OnInvalidated;
                value?.Invalidated += OnInvalidated;

                field = value;

                Invalidated?.Invoke();
            }
        }

        private void OnInvalidated()
        {
            Invalidated?.Invoke();
        }

        public event Action? Invalidated;
    }
}