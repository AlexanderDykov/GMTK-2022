namespace Game.General.Views
{
    using System.Collections.Generic;
    using System.Linq;
    using Effects;
    using Services;
    using UnityEngine;
    using Zenject;
    using static Effects.DiceType;

    public class Root : MonoBehaviour
    {
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
            var rat = new CreatureConfig
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
            
            
            var playerView = FindObjectsOfType<CreatureView>().FirstOrDefault(x => x.CompareTag("Player"));
            if (playerView != null)
            {
                playerView.ApplyConfig(player);
            }
            
            var enemyView = FindObjectsOfType<CreatureView>().FirstOrDefault(x => x.CompareTag("Enemy"));
            if (enemyView != null)
            {
                enemyView.ApplyConfig(rat);
            }
        }
    }
}