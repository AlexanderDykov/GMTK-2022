namespace Game.General.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Views.BodyParts;

    public interface IPlayerAssignedMoveCollector
    {
        Dictionary<Target, List<Move>> CreateAssignedMove();
    }

    public class PlayerAssignedMoveCollector : IPlayerAssignedMoveCollector
    {
        public Dictionary<Target, List<Move>> CreateAssignedMove()
        {
            var allBodyParts = Object.FindObjectsOfType<BodyPartView>();

            return allBodyParts.Select(bodyPart => bodyPart.CreateAssignedMove()).Where(x => x.Item2.Count > 0)
                .ToDictionary(valueTuple => valueTuple.Item1, valueTuple => valueTuple.Item2);
        }
    }
}