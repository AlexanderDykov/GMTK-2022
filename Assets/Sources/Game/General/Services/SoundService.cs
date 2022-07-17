namespace Game.General.Services
{
    using UnityEngine;
    using Zenject;

    public interface ISoundService
    {
        void Play(SFX name);
    }

    public enum SFX
    {
        DiceRoll,
        DicePlace,
        Attack,
        GetDamage,
        ButtonClick,
        SelectDice
    }

    public class SoundService : MonoInstaller<SoundService>, ISoundService
    {
        [System.Serializable]
        public class AudioDictionary : SerializableDictionary<SFX, AudioClip>
        {
        }


        [SerializeField]
        private AudioDictionary audioDictionary;

        [SerializeField]
        private AudioSource audioSource;

        public override void InstallBindings()
        {
            Container.Bind<ISoundService>().FromInstance(this).AsSingle();
        }

        public void Play(SFX name)
        {
            if (audioDictionary.ContainsKey(name))
            {
                audioSource.PlayOneShot(audioDictionary[name]);
            }
        }
    }
}