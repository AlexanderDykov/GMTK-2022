namespace Game.General.Views
{
    using System.Collections.Generic;
    using Effects;
    using Services;
    using UnityEngine;
    using Zenject;
    using static Effects.DiceType;

    public class Root : MonoBehaviour
    {
        [SerializeField]
        private DicesPanel playerPanel;

        [SerializeField]
        private Transform bodiesParent;

        private void Start()
        {
            var player = new CreatureConfig
            {
                BodyParts = new List<BodyPart>
                {
                    BodyPart.Head
                },
                Dices = new List<Dice>
                {
                    new(new List<DiceType>
                    {
                        One,
                        Two,
                        Three,
                        Four,
                        Five,
                        Six,
                    })
                },
                MaxHealth = 20
            };
            var playerView = FindObjectOfType<CreatureView>();
            playerView.ApplyConfig(player);
        }
    }
}