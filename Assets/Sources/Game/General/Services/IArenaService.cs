namespace Game.General.Services
{
    public interface IArenaService
    {
        void Add(string id, Creature creature);

        void RestartArena();

        void ApplyTurn(Turn turn);
    }

    public class ArenaService : IArenaService
    {
        private readonly Arena _arena = new Arena();

        public void Add(string id, Creature creature)
        {
            _arena.Creatures.Add(id, creature);
        }

        public void RestartArena()
        {
            _arena.Creatures.Clear();
        }

        public void ApplyTurn(Turn turn)
        {
            _arena.ApplyTurn(turn);
        }
    }
}