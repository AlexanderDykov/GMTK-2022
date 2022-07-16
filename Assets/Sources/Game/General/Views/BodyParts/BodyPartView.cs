namespace Game.General.Views.BodyParts
{
    using System.Collections.Generic;
    using System.Linq;
    using Effects;
    using Services;
    using UnityEngine;
    using Zenject;

    public class BodyPartView : MonoBehaviour
    {
        [Inject]
        private IDiceSelector _selector;

        [SerializeField]
        private Transform parentForDices;

        private BodyPart _bodyPart;

        private string _id;

        private readonly HashSet<DiceElement> _dices = new();


        private void Start()
        {
            _selector.ElementRemovedToHand += OnElementRemovedToHand;
        }

        private void OnElementRemovedToHand(DiceElement diceElement)
        {
            Remove(diceElement);
        }

        private void Remove(DiceElement diceElement)
        {
            _dices.Remove(diceElement);
        }

        public void Create(BodyPart bodyPart, string id)
        {
            _id = id;
            _bodyPart = bodyPart;
            _dices.Clear();
        }

        public (Target, List<Move>) CreateAssignedMove()
        {
            Dictionary<string, List<DiceType>> moves = new();

            foreach (var diceElement in _dices)
            {
                if (!moves.ContainsKey(diceElement.sourceId))
                {
                    moves[diceElement.sourceId] = new List<DiceType>();
                }

                moves[diceElement.sourceId].Add(diceElement.diceType);
            }

            return (new Target()
            {
                Id = _id,
                BodyPart = _bodyPart
            }, moves.Select(x => new Move()
            {
                DiceTypes = x.Value,
                SourceId = x.Key
            }).ToList());
        }

        private void OnDestroy()
        {
            _selector.ElementRemovedToHand -= OnElementRemovedToHand;
        }

        public void Add(DiceElement selectorSelected)
        {
            _dices.Add(selectorSelected);
            //todo move selectorSelected to this transform position
            selectorSelected.transform.parent = parentForDices;
            selectorSelected.transform.position = transform.position;
        }

        public void ResetBodyPart()
        {
            _dices.Clear();
        }
    }
}