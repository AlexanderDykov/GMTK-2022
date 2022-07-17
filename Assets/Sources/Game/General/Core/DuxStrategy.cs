namespace Game.General
{
    using System.Collections.Generic;
    using System.Linq;
    using Effects;

    public class DuxStrategy : IChooseMovesStrategy
    {
        public Dictionary<Target, List<Move>> ChooseMoves(Creature self, Arena arena, Turn turn)
        {
            var result = new Dictionary<Target, List<Move>>();
            var anyDice = self.Config.Dices.Last();
            if (anyDice != null)
            {
                bool isDefenceMove = false;
                if (self.GetHealthPercents() < 40)
                {
                    isDefenceMove = IChooseMovesStrategy.MaybeTrue(75);
                }
                else
                {
                    isDefenceMove = IChooseMovesStrategy.MaybeTrue(50);
                }
                var targetCreature = self;
                if (!isDefenceMove)
                {
                    targetCreature = IChooseMovesStrategy.FindPlayer(arena);
                }
                var diceTypes = new List<DiceType>
                {
                    anyDice.Random()
                };
                var (target, moves) = IChooseMovesStrategy.MakeMove(self, targetCreature, diceTypes);
                result.Add(target, moves);
            }
            return result;
        }
    }
}