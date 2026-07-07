using System.Collections;

namespace Zion
{
    public class ExecutableQueue<T> : IDisposable, IEnumerable<T>
    {
        #region Data
        private readonly Lock Lock = new Lock();

        private readonly Queue<T> Tasks;
        private readonly AsyncAction<T> Function;

        private CancellationTokenSource CompletionCancellation
        {
            get;
            set
            {
                field.Dispose();
                field = value;
            }
        } = new();

        private bool TaskRunning = false;

        #endregion

        #region Properties
        public AsyncAction? Completion { get; init; }

        public int Delay
        {
            get;
            init => field = Math.Max(value, 0);
        } = 1000;

        public int DelayToCompletion
        {
            get;
            init => field = Math.Max(value, 0);
        } = 3000;

        public int Count => Tasks.Count;

        #endregion

        #region Constructors
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

        #endregion

        #region PublicMethods
        public void Add(T Item)
        {
            lock (Lock)
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
            lock (Lock)
            {
                Tasks.Clear();
                CompletionCancellation.Cancel();
            }
        }

        public bool Contains(T Item)
        {
            lock (Lock)
            {
                return Tasks.Contains(Item);
            }
        }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            CompletionCancellation.Dispose();
        }

        #endregion

        #region IEnumerable
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            T[] Items;
            lock (Lock)
            {
                Items = Tasks.ToArray(Tasks.Count);
            }
            return Items.Enumerate();
        }

        #endregion

        #region PrivateMethods
        private async Task ProcessQueue()
        {
            while (true)
            {
                T CurrentTask;

                lock (Lock)
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
            try
            {
                await Task.Delay(DelayToCompletion, CancellationToken);
            }
            catch (TaskCanceledException)
            {
                return;
            }

            lock (Lock)
            {
                if (Tasks.Count > 0)
                {
                    return;
                }
            }

            await Completion!.Invoke();
        }

        #endregion
    }
}