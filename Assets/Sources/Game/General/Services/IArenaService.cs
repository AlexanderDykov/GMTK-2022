using Zenject;

namespace Game.General.Services
{
    using System.Collections.Generic;

    public interface IArenaService
    {
        Arena Get();

        void Add(string id, Creature creature);

        void ClearArena();

        void ApplyTurn(Turn turn);
        Dictionary<string, Creature> Creatures { get; }
    }

    public class ArenaService : IArenaService
    {
        private readonly Arena _arena;

        public ArenaService(ISpellBookService spellBookService, ISpellVisualizerService spellVisualizerService)
        {
            _arena = new Arena(spellBookService.Get(), spellVisualizerService);
        }

        public Arena Get()
        {
            return _arena;
        }

        public void Add(string id, Creature creature)
        {
            if (_arena.Creatures.ContainsKey(id))
            {
                return;
            }

            _arena.Creatures.Add(id, creature);
        }

        public void ClearArena()
        {
            _arena.Creatures.Clear();
        }

        public void ApplyTurn(Turn turn)
        {
            _arena.ApplyTurn(turn);
        }

        public Dictionary<string, Creature> Creatures => _arena.Creatures;
    }
}