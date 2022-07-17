using System;
using System.Collections.Generic;
using System.Linq;
using Game.General.Effects;

namespace Game.General
{
    public class Spell
    {
        public string AttackDescription;
        public string DefenceDescription;
        public Func<Move, Effect> EffectMaker;
    }

    public class SpellBook
    {
        private Dictionary<List<DiceType>, Spell> _spells = new();

        public void Add(List<DiceType> combo, Spell spell)
        {
            _spells.Add(combo, spell);
        }

        public Effect Find(Move move)
        {
            return _spells
                .Where(x => x.Key.Count == move.DiceTypes.Count && x.Key.All(move.DiceTypes.Contains))
                .Select(x => x.Value.EffectMaker(move))
                .FirstOrDefault() ?? new DefaultEffect(move);
        }
    }
}
