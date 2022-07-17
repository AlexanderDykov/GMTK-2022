namespace Game.General
{
    using System.Collections.Generic;

    public class Turn
    {
        public Dictionary<Target, List<Move>> AssignedMoves { get; private set; }

        public Turn()
        {
            AssignedMoves = new Dictionary<Target, List<Move>>(new Target.EqualityComparer());
        }

        public void AssignMoves(Dictionary<Target, List<Move>> newMoves)
        {
            foreach (var movesPerTarget in newMoves)
            {
                var target = movesPerTarget.Key;
                var moves = movesPerTarget.Value;
                if (!AssignedMoves.ContainsKey(target))
                {
                    AssignedMoves.Add(target, new List<Move>(moves));
                }
                else
                {
                    AssignedMoves[target].AddRange(moves);
                }
            }
        }
    }

    public class Target
    {
        public string Id;
        public BodyPart BodyPart;

        public class EqualityComparer : IEqualityComparer<Target>
        {
            public bool Equals(Target x, Target y)
            {
                return ((x.Id == y.Id) && (x.BodyPart == y.BodyPart));
            }

            public int GetHashCode(Target obj)
            {
                var combined = obj.Id + obj.BodyPart;
                return combined.GetHashCode();
            }
        }
    }
}
