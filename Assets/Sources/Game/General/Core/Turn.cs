namespace Game.General
{
    using System.Collections.Generic;
    using Effects;

    public class Turn
    {
        public List<DiceType> PlayerRolledDiceTypes;

        public Dictionary<string, List<DiceType>> CreaturesRolledDiceTypes;

        public Dictionary<Target, List<Move>> AssignedMoves;
    }

    public class Target
    {
        public string Id;
        public BodyPart BodyPart;
    }
}