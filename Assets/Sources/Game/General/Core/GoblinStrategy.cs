namespace Game.General
{
    using System.Collections.Generic;
    using Effects;

    public class GoblinStrategy : IChooseMovesStrategy
    {
        public Dictionary<Target, List<Move>> ChooseMoves(Creature self, Arena arena, Turn turn)
        {
            var result = new Dictionary<Target, List<Move>>();
            var maxNumOfDices = 2;
            var rolls = IChooseMovesStrategy.RollDices(self.Config.Dices, maxNumOfDices);
            var player = IChooseMovesStrategy.FindPlayer(arena);
            var move = new Move
            {
                SourceId = self.Id,
                DiceTypes = rolls
            };
            var effect = arena.SpellBook.Find(move);
            var isCombo = (effect is DefaultEffect is false);
            if (isCombo)
            {
                var (target, moves) = IChooseMovesStrategy.MakeMove(self, player, move.DiceTypes);
                result.Add(target, moves);
            }
            else
            {
                var numOfDicesToAttack = 1;
                if ((self.GetHealthPercents() < 40) && IChooseMovesStrategy.MaybeTrue(50))
                {
                    numOfDicesToAttack += 1;
                }
                var attackDices = new List<DiceType>();
                var defenceDices = new List<DiceType>();
                for (int i = 0; i < rolls.Count; ++i)
                {
                    if (i < numOfDicesToAttack)
                    {
                        attackDices.Add(rolls[i]);
                    }
                    else
                    {
                        defenceDices.Add(rolls[i]);
                    }
                }
                if (attackDices.Count != 0)
                {
                    var (target, moves) = IChooseMovesStrategy.MakeMove(self, player, attackDices);
                    result.Add(target, moves);
                }
                if (defenceDices.Count != 0)
                {
                    var (target, moves) = IChooseMovesStrategy.MakeMove(self, self, defenceDices);
                    result.Add(target, moves);
                }
            }
            return result;
        }
    }
}