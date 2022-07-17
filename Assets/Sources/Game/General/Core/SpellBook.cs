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

    public enum SpellType
    {
        None,
        Water,
        Fire,
        Air,
        Ground
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
                .Where(x => IsMatches(x.Key, move.DiceTypes))
                .Select(x => x.Value.EffectMaker(move))
                .FirstOrDefault() ?? new DefaultEffect(move);
        }

        private static bool IsMatches(List<DiceType> pattern, List<DiceType> candidate)
        {
            if (pattern.Count != candidate.Count)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < pattern.Count; i++)
                {
                    if (pattern[i] != candidate[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}