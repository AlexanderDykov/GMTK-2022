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


        public void Setup(string sourceId, List<DiceType> dices)
        {
            for (var index = 0; index < dices.Count; index++)
            {
                var diceType = dices[index];
                var diceElement = Instantiate(_diceElementPrefab, parent);
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