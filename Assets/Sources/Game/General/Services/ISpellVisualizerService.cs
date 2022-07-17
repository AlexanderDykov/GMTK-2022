namespace Game.General.Services
{
    using Effects;
    using UnityEngine;
    using Views;
    using Zenject;

    public interface ISpellVisualizerService
    {
        void Show(Effect spell, bool attack, Vector2 position);
    }


    class SpellVisualizerService : ISpellVisualizerService
    {
        [Inject]
        private ISpellIconProvider spellIconProvider;

        private SpellVisualizer view;

        public void Show(Effect spell, bool attack, Vector2 position)
        {
            if (view == null)
            {
                view = Resources.Load<SpellVisualizer>("SpellView");
            }

            var instance = Object.Instantiate(view, position, Quaternion.identity);
            instance.Setup(spellIconProvider.GetIcon(spell, attack));
        }
    }
}