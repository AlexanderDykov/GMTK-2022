namespace Game.General.Views.BodyParts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Effects;
    using Services;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using Zenject;

    public class BodyPartView : MonoBehaviour, IPointerClickHandler
    {
        [Inject]
        private IDiceSelector _selector;

        private BodyPart _bodyPart;

        private string _id;

        private Dictionary<string, List<DiceType>> _moves = new();

        public void Create(BodyPart bodyPart, string id)
        {
            _id = id;
            _bodyPart = bodyPart;
            _moves.Clear();
        }

        public void AddDice(string sourceId, DiceType diceTypes)
        {
            if (!_moves.ContainsKey(sourceId))
            {
                _moves[sourceId] = new List<DiceType>();
            }

            _moves[sourceId].Add(diceTypes);
        }

        public (Target, List<Move>) CreateAssignedMove()
        {
            return (new Target()
            {
                Id = _id,
                BodyPart = _bodyPart
            }, _moves.Select(x => new Move()
            {
                DiceTypes = x.Value,
                SourceId = x.Key
            }).ToList());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var selectorSelected = _selector.Selected;
            if (selectorSelected != null)
            {
                AddDice(selectorSelected.sourceId, selectorSelected.diceType);
                //todo move selectorSelected to this transform position
                selectorSelected.transform.position = transform.position;
                _selector.Deselect();
            }
        }
    }
}