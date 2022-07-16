namespace Game.General
{
    using System.Collections.Generic;

    public class Turn
    {
        public Dictionary<Target, List<Move>> AssignedMoves;
    }

    public class Target
    {
        public string Id;
        public BodyPart BodyPart;
    }
}