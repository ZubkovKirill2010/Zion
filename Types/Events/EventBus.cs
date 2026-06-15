namespace Zion
{
    public sealed class EventBus
    {
        public event Action? Invalidated;

        public void Invalidate()
        {
            Invalidated?.Invoke();
        }
    }
}