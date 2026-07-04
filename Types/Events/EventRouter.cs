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
                field = value;

                Invalidated?.Invoke();

                value?.Invalidated += OnInvalidated;
            }
        }

        private void OnInvalidated()
        {
            Invalidated?.Invoke();
        }

        public event Action? Invalidated;
    }
}