using System.Collections.Concurrent;

public class ObjectPool<T> : IDisposable
{
    private readonly ConcurrentBag<T> Objects;
    private readonly Func<T> Create;
    private readonly Action<T>? Remove;
    private readonly Func<T, T>? Getter;

    private bool IsDisposed;

    public int Count => Objects.Count;
    public int CurrentSize { get; private set; }
    public int MaxSize { get; private init; }


    public ObjectPool(Func<T> Create, Action<T>? Remove = null, Func<T, T>? Getter = null)
        : this(Create, Remove, Getter, -1) { }

    public ObjectPool(Func<T> Create, Action<T>? Remove = null, Func<T, T>? Getter = null, int MaxSize = -1)
    {
        Objects = new ConcurrentBag<T>();
        this.Create = Create ?? throw new ArgumentNullException(nameof(Create));
        this.Remove = Remove;
        this.Getter = Getter;
        this.MaxSize = MaxSize;
        CurrentSize = 0;
    }

    public T Get()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(ObjectPool<T>));
        }

        if (Objects.TryTake(out T Item))
        {
            return Getter is null ? Item : Getter(Item);
        }

        if (int.IsNegative(MaxSize) || CurrentSize < MaxSize)
        {
            int Size = 0;
            Interlocked.Increment(ref Size);
            CurrentSize = Size;
            return Getter is null ? Create() : Getter(Create());
        }

        SpinWait SpinWait = new SpinWait();
        while (!Objects.TryTake(out Item) && !IsDisposed)
        {
            SpinWait.SpinOnce();
        }

        return Item;
    }

    public void Add(T Item)
    {
        if (IsDisposed)
        {
            return;
        }

        if (Item is null)
        {
            throw new ArgumentNullException(nameof(Item));
        }

        Remove?.Invoke(Item);
        Objects.Add(Item);
    }

    public void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        IsDisposed = true;

        if (typeof(IDisposable).IsAssignableFrom(typeof(T)))
        {
            foreach (T? Object in Objects)
            {
                (Object as IDisposable)?.Dispose();
            }
        }

        Objects.Clear();
    }
}