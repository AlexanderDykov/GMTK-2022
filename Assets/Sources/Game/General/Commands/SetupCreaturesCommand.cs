namespace Game.General.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using Effects;
    using Views;
    using static Effects.DiceType;
    using static UnityEngine.Object;

    public class SetupCreaturesCommand : ICommand
    {
        public UniTask Execute()
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

            return UniTask.CompletedTask;
        }
    }
}