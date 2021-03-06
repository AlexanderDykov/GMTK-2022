namespace Game.General.Views.BodyParts
{
    using System.Collections.Generic;
    using System.Linq;
    using Effects;
    using Services;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class BodyPartView : MonoBehaviour
    {
        [Inject]
        private IDiceSelector _selector;

        [Inject]
        private IPlayerProvider playerProvider;

        [Inject]
        private IEnemyProvider enemyProvider;

        [SerializeField]
        private Transform parentForDices;

        [SerializeField]
        private Transform parentForEnemyDices;

        [SerializeField]
        private DiceElement prefab;

        [SerializeField]
        private Image bg;

        [SerializeField]
        private Sprite shield;

        [SerializeField]
        private Sprite sword;

        [SerializeField]
        private Image enemyIcon;

        [SerializeField]
        private Animator animator;

        public Transform shieldParent;

        private BodyPart _bodyPart;

        private string _id;

        private readonly HashSet<DiceElement> _dices = new();

        private List<DiceElement> enemies = new List<DiceElement>();

        public BodyPart BodyPart => _bodyPart;
        public string Id => _id;

        private void Start()
        {
            animator.keepAnimatorControllerStateOnDisable = true;
            _selector.ElementRemovedToHand += OnElementRemovedToHand;
        }

        private void OnElementRemovedToHand(DiceElement diceElement)
        {
            Remove(diceElement);
        }

        private void Remove(DiceElement diceElement)
        {
            _dices.Remove(diceElement);
        }

        public void Create(BodyPart bodyPart, Creature creature)
        {
            _id = creature.Id;
            _bodyPart = bodyPart;

            var isPlayerPart = creature.Id == playerProvider.Id;
           
            bg.sprite = isPlayerPart ? shield : sword;

            _dices.Clear();
        }

        public (Target, List<Move>) CreateAssignedMove()
        {
            Dictionary<string, List<DiceType>> moves = new();

            foreach (var diceElement in _dices)
            {
                if (!moves.ContainsKey(diceElement.sourceId))
                {
                    moves[diceElement.sourceId] = new List<DiceType>();
                }

                moves[diceElement.sourceId].Add(diceElement.diceType);
            }

            return (new Target()
            {
                Id = _id,
                BodyPart = _bodyPart
            }, moves.Select(x => new Move()
            {
                DiceTypes = x.Value,
                SourceId = x.Key
            }).ToList());
        }

        private void OnDestroy()
        {
            _selector.ElementRemovedToHand -= OnElementRemovedToHand;
        }

        public void AddEnemyDices(DiceType dices, string enemyId, string enemySpriteName)
        {
            var isPlayerPart = enemyId == playerProvider.Id;
            enemyIcon.sprite =
                Resources.Load<Sprite>(isPlayerPart ? enemyProvider.Current.SpriteName : enemySpriteName);
            parentForEnemyDices.gameObject.SetActive(true);
            var diceElement = Instantiate(prefab, parentForEnemyDices);
            diceElement.Apply(dices, "");
            enemies.Add(diceElement);
            
        }
        public void PlayAttack()
        {
            if (animator != null)
            {
                animator.SetTrigger("Fight");
            }
        }

        public void Add(DiceElement selectorSelected)
        {
            _dices.Add(selectorSelected);
            //todo move selectorSelected to this transform position
            selectorSelected.transform.SetParent(parentForDices);
            selectorSelected.transform.position = transform.position;
        }

        public void ResetBodyPart()
        {
            parentForEnemyDices.gameObject.SetActive(false);
            for (var index = enemies.Count - 1; index >= 0; index--)
            {
                var diceElement = enemies[index];
                Destroy(diceElement.gameObject);
                enemies.RemoveAt(index);
            }

            gameObject.SetActive(false);
            gameObject.SetActive(true);
            _dices.Clear();
        }
    }
}