namespace Game.General.Services
{
    using System;
    using Effects;
    using UnityEngine;
    using Zenject;

    public class DiceIconProvider : MonoInstaller, IDiceIconProvider
    {
        [System.Serializable]
        public class IconsDictionary : SerializableDictionary<DiceType, Sprite>
        {
        }

        [SerializeField]
        public IconsDictionary _dicesToSprites;

        public override void InstallBindings()
        {
            Container.Bind<IDiceIconProvider>().FromInstance(this).AsSingle();
        }

        public Sprite GetIcon(DiceType diceType)
        {
            return _dicesToSprites[diceType];
        }
    }
}