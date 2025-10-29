using System.Collections.Concurrent;

namespace Zion
{
    public static class PathFinder
    {
        public static TPoint[] Pave<TPoint>(PathInfo<TPoint> Info)
        {
            if (Info.EqualsPoints(Info.Start, Info.End))
            {
                return Array.Empty<TPoint>();
            }

            ConcurrentDictionary<TPoint, byte> Visited = new ConcurrentDictionary<TPoint, byte>();
            ConcurrentBag<Structure<TPoint>> Results = new ConcurrentBag<Structure<TPoint>>();
            int MaxDistance = Info.MaxDistance == -1 ? int.MaxValue : Info.MaxDistance;

            bool Found = Pave(Info, 0, new Structure<TPoint>(Info.Start), Visited, Results, MaxDistance);

            if (Found && Results.TryTake(out Structure<TPoint> Result))
            {
                return ToArray(Result);
            }

            return Array.Empty<TPoint>();
        }

        public static TPoint[][] PaveMultiple<TPoint>(MultiplePathInfo<TPoint> Info)
        {
            ConcurrentBag<TPoint[]> Results = new ConcurrentBag<TPoint[]>();
            ConcurrentDictionary<TPoint, byte> GlobalVisited = Info.ExcludeVisited ? new ConcurrentDictionary<TPoint, byte>() : null;
            int MaxDistance = Info.MaxDistance == -1 ? int.MaxValue : Info.MaxDistance;

            PaveMultiple(Info, 0, new Structure<TPoint>(Info.Start), new ConcurrentDictionary<TPoint, byte>(), GlobalVisited, Results);

            return Results.ToArray();
        }


        private static bool Pave<TPoint>(PathInfo<TPoint> Info, int Length, Structure<TPoint> Branch,
            ConcurrentDictionary<TPoint, byte> Visited, ConcurrentBag<Structure<TPoint>> Results, int MaxDistance)
        {
            if (Length >= MaxDistance)
            {
                return false;
            }

            Visited[Branch.Value] = 0;

            TPoint LastPosition = default;
            Structure<TPoint> Parent = Branch.Parent;

            if (Parent is not null)
            {
                LastPosition = Parent.Value;
            }

            IEnumerable<TPoint> Neighbors = Info.GetAllPath(Branch.Value, LastPosition);
            List<TPoint> ValidNeighbors = new List<TPoint>();

            foreach (TPoint Cell in Neighbors)
            {
                if (!Visited.ContainsKey(Cell))
                {
                    ValidNeighbors.Add(Cell);
                }
            }

            bool Found = false;

            if (Length < 3) // Многопоточность на первых уровнях
            {
                ParallelOptions Options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
                Parallel.ForEach(ValidNeighbors, Options, (Cell, State) =>
                {
                    Structure<TPoint> Child = new Structure<TPoint>(Cell);
                    Branch.Add(Child);

                    if (Info.EqualsPoints(Cell, Info.End))
                    {
                        Results.Add(Child);
                        Found = true;
                        State.Stop();
                    }
                    else
                    {
                        if (Pave(Info, Length + 1, Child, Visited, Results, MaxDistance))
                        {
                            Found = true;
                            State.Stop();
                        }
                    }
                });
            }
            else
            {
                foreach (TPoint Cell in ValidNeighbors)
                {
                    Structure<TPoint> Child = new Structure<TPoint>(Cell);
                    Branch.Add(Child);

                    if (Info.EqualsPoints(Cell, Info.End))
                    {
                        Results.Add(Child);
                        return true;
                    }

                    if (Pave(Info, Length + 1, Child, Visited, Results, MaxDistance))
                    {
                        return true;
                    }
                }
            }

            Visited.TryRemove(Branch.Value, out _);
            return Found;
        }

        private static void PaveMultiple<TPoint>(MultiplePathInfo<TPoint> Info, int Length, Structure<TPoint> Branch,
            ConcurrentDictionary<TPoint, byte> LocalVisited, ConcurrentDictionary<TPoint, byte> GlobalVisited,
            ConcurrentBag<TPoint[]> Results)
        {
            if (Length >= Info.MaxDistance)
            {
                return;
            }

            LocalVisited[Branch.Value] = 0;
            if (GlobalVisited != null)
            {
                GlobalVisited[Branch.Value] = 0;
            }

            TPoint LastPosition = default;
            Structure<TPoint> Parent = Branch.Parent;

            if (Parent is not null)
            {
                LastPosition = Parent.Value;
            }

            IEnumerable<TPoint> Neighbors = Info.GetAllPath(Branch.Value, LastPosition);
            List<TPoint> ValidNeighbors = new List<TPoint>();

            foreach (TPoint Cell in Neighbors)
            {
                if (!LocalVisited.ContainsKey(Cell) && (GlobalVisited == null || !GlobalVisited.ContainsKey(Cell)))
                {
                    ValidNeighbors.Add(Cell);
                }
            }

            if (Length < 2)
            {
                ParallelOptions Options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
                Parallel.ForEach(ValidNeighbors, Options, Cell =>
                {
                    Structure<TPoint> Child = new Structure<TPoint>(Cell);
                    Branch.Add(Child);

                    if (Info.EqualsPoints(Cell, Info.End))
                    {
                        Results.Add(ToArray(Child));
                    }

                    PaveMultiple
                    (
                        Info, Length + 1, Child, new ConcurrentDictionary<TPoint, byte>(LocalVisited), GlobalVisited, Results
                    );
                });
            }
            else
            {
                foreach (TPoint Cell in ValidNeighbors)
                {
                    Structure<TPoint> Child = new Structure<TPoint>(Cell);
                    Branch.Add(Child);

                    if (Info.EqualsPoints(Cell, Info.End))
                    {
                        Results.Add(ToArray(Child));
                    }

                    PaveMultiple
                    (
                        Info, Length + 1, Child, new ConcurrentDictionary<TPoint, byte>(LocalVisited), GlobalVisited, Results
                    );
                }
            }
        }

        private static TPoint[] ToArray<TPoint>(Structure<TPoint> Structure)
        {
            List<TPoint> Path = new List<TPoint>();
            Structure<TPoint> Current = Structure;

            while (Current != null)
            {
                Path.Add(Current.Value);
                Current = Current.Parent;
            }

            Path.Reverse();
            return Path.ToArray();
        }
    }
}