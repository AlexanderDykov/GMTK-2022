using System.Collections.Generic;
using Game.General.Effects;

namespace Game.General.Services
{
    public interface ISpellBookService
    {
        SpellBook Get();
    }

    public class SpellBookService : ISpellBookService
    {
        private readonly SpellBook _spellBook = new SpellBook();

        public SpellBookService()
        {
            _spellBook.Add(new List<DiceType>
            {
                DiceType.One,
                DiceType.One,
            }, new Spell
            {
                AttackDescription = "+10 to damage",
                DefenceDescription = "Block strongest attack dice",
                EffectMaker = move => new OneOneComboEffect(move)
            });
            _spellBook.Add(new List<DiceType>
            {
                DiceType.Two,
                DiceType.Two,
            }, new Spell
            {
                AttackDescription = "Double damage",
                DefenceDescription = "Double resist",
                EffectMaker = move => new TwoTwoComboEffect(move)
            });
            _spellBook.Add(new List<DiceType>
            {
                DiceType.Three,
                DiceType.Three,
            }, new Spell
            {
                AttackDescription = "Ignore all opponent resist",
                DefenceDescription = "Block first double dices damage and their effect (if any)",
                EffectMaker = move => new ThreeThreeComboEffect(move)
            });
            _spellBook.Add(new List<DiceType>
            {
                DiceType.Four,
                DiceType.Four,
            }, new Spell
            {
                AttackDescription = "+X to Damage where X is the weakest defence dice",
                DefenceDescription = "Nothing special... for now",
                EffectMaker = move => new FourFourComboEffect(move)
            });
        }

        public SpellBook Get()
        {
            return _spellBook;
        }
    }
}
