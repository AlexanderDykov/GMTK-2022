namespace Game.General.Views
{
    using System;
    using System.Collections.Generic;
    using Effects;
    using UnityEngine;

    public class DicesPanel : MonoBehaviour
    {
        [SerializeField]
        private DiceElement _diceElementPrefab;

        [SerializeField]
        private Transform parent;

        private List<DiceElement> _pool = new List<DiceElement>();

        public void Setup(string sourceId, List<DiceType> dices)
        {
            for (var index = 0; index < dices.Count; index++)
            {
                var diceType = dices[index];
                DiceElement diceElement = null;
                if (index < _pool.Count)
                {
                    diceElement = _pool[index];
                }
                else
                {
                    diceElement = Instantiate(_diceElementPrefab, parent);
                    _pool.Add(diceElement);
                }

                ApplyDice(sourceId, diceElement, diceType);
            }
        }

        private async void ApplyDice(string sourceId, DiceElement diceElement, DiceType diceType)
        {
            await diceElement.SpeenAnimation();
            diceElement.Apply(diceType, sourceId);
        }
    }
}