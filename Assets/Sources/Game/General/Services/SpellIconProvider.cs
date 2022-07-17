namespace Game.General.Services
{
    using Effects;
    using UnityEngine;
    using Zenject;

    public interface ISpellIconProvider
    {
        Sprite GetIcon(Effect spell, bool isAttack);
    }

    public class SpellIconProvider : MonoInstaller<SpellIconProvider>, ISpellIconProvider
    {
        [System.Serializable]
        public class IconsDictionary : SerializableDictionary<string, Sprite>
        {
        }

        [SerializeField]
        public IconsDictionary _dicesToSprites;

        public override void InstallBindings()
        {
            Container.Bind<ISpellIconProvider>().FromInstance(this).AsSingle();
        }
        
        public Sprite GetIcon(Effect spell, bool isAttack)
        {
            var key = spell.SpellType + (isAttack ? "Attack" : "Defend");
            return _dicesToSprites[key];
        }
    }
}