namespace Game.General.Services
{
    using Cysharp.Threading.Tasks;
    using Effects;
    using UnityEngine;
    using Views;
    using Zenject;

    public interface ISpellVisualizerService
    {
        UniTask Show(Effect spell, bool attack, Vector2 position);
    }


    class SpellVisualizerService : ISpellVisualizerService
    {
        [Inject]
        private ISpellIconProvider spellIconProvider;

        private SpellVisualizer view;

        public UniTask Show(Effect spell, bool attack, Vector2 position)
        {
            if (view == null)
            {
                view = Object.FindObjectOfType<SpellVisualizer>();
            }

            if (view != null)
            {
                view.Setup(spellIconProvider.GetIcon(spell, attack));
                view.transform.position = position;
                //
            }
            return UniTask.Delay(600);
        }
    }
}