namespace Game.General
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class CreatureConfig
    {
        public string SpriteName;

        public List<BodyPart> BodyParts;

        public List<Dice> Dices;

        public int MaxHealth;
    }

    public class Creature
    {
        private int _currentHealth;

        public readonly CreatureConfig Config;

        public string Id { get; private set; }

        public Creature(CreatureConfig config, string id)
        {
            _currentHealth = config.MaxHealth;
            Config = config;
            Id = id;
        }

        public bool IsDead => _currentHealth <= 0;
        public event Action<int> CurrentHealthChanged;
        public event Action Dead;

        public void ApplyDamage(int damage)
        {
            _currentHealth -= damage;
            Debug.LogError("Creature " + Id + " received = " + damage + " and health = " + _currentHealth);
            _currentHealth = Math.Max(0, _currentHealth);
            CurrentHealthChanged?.Invoke(_currentHealth);

            if (IsDead)
            {
                Dead?.Invoke();
            }
        }
    }
}
