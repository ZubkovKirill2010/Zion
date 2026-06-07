using System.Collections;

namespace Zion
{
    public class ExecutableQueue<T> : IEnumerable<T>
    {
        private readonly object LockObject = new object();
        private bool TaskRunning = false;

        private CancellationTokenSource CompletionCancellation = new CancellationTokenSource();

        private readonly Queue<T> Tasks;
        private readonly AsyncAction<T> Function;

        public AsyncAction? Completion { get; init; }

        private int _Delay { get; init; } = 1000;
        private int _DelayToCompletion { get; init; } = 3000;

        public int Delay
        {
            get => _Delay;
            init => _Delay = Math.Max(value, 0);
        }
        public int DelayToCompletion
        {
            get => _DelayToCompletion;
            init => _DelayToCompletion = Math.Max(value, 0);
        }

        public int Count => Tasks.Count;

        public ExecutableQueue(Action<T> Action) : this(Action.ToAsync(), 5) { }
        public ExecutableQueue(AsyncAction<T> Action) : this(Action, 5) { }

        public ExecutableQueue(Action<T> Action, int Capacity) : this(Action.ToAsync(), Capacity) { }
        public ExecutableQueue(AsyncAction<T> Action, int Capacity)
        {
            Tasks = new Queue<T>(Capacity);
            Function = Action;
        }

        public ExecutableQueue(IEnumerable<T> Tasks, Action<T> Action) : this(Tasks, Action.ToAsync()) { }
        public ExecutableQueue(IEnumerable<T> Tasks, AsyncAction<T> Action)
        {
            this.Tasks = new Queue<T>(Tasks);
            Function = Action;
        }

        public void Add(T Item)
        {
            lock (LockObject)
            {
                Tasks.Enqueue(Item);

                CompletionCancellation.Cancel();
                CompletionCancellation = new CancellationTokenSource();

                if (!TaskRunning)
                {
                    TaskRunning = true;
                    Task.Run(ProcessQueue);
                }
            }
        }

        public void Clear()
        {
            lock (LockObject)
            {
                Tasks.Clear();
                CompletionCancellation.Cancel();
            }
        }

        public bool Contains(T Item)
        {
            lock (LockObject)
            {
                return Tasks.Contains(Item);
            }
        }

        private async Task ProcessQueue()
        {
            while (true)
            {
                T CurrentTask;

                lock (LockObject)
                {
                    if (Tasks.Count == 0)
                    {
                        if (Completion is not null)
                        {
                            _ = DelayAndCompleteAsync(CompletionCancellation.Token);
                        }
                        TaskRunning = false;
                        break;
                    }

                    CurrentTask = Tasks.Dequeue();
                }

                await Function(CurrentTask);

                if (Delay > 0)
                {
                    await Task.Delay(Delay);
                }
            }
        }

        private async Task DelayAndCompleteAsync(CancellationToken CancellationToken)
        {
            await Task.Delay(DelayToCompletion, CancellationToken);

            if (!CancellationToken.IsCancellationRequested)
            {
                await Completion!.Invoke();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            T[] Items;
            lock (LockObject)
            {
                Items = Tasks.ToArray(Tasks.Count);
            }

            foreach (T item in Items)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}