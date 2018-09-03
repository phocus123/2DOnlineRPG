using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("All Characters")]
        [SerializeField] string characterName;
        [SerializeField] Text nameText;

        [Header("Player and Enemy")]
        [SerializeField] float moveSpeed;
        [SerializeField] float maxHealthPoints;
        [SerializeField] float maxEnergyPoints;
        [SerializeField] CharacterStat[] characterStats;
        [Space]
        [SerializeField] Transform[] exitPoints;
        [SerializeField] Vector2 colliderOffset;
        [SerializeField] Vector2 colliderSize;
        [SerializeField] StatPanel statPanel;

        [Header("Enemy")]
        [SerializeField] int experienceWorth;
        [SerializeField] float aggroRange;

        public CharacterStat[] CharacterrStats { get { return characterStats; } }
        public StatPanel StatPanel { get { return statPanel; } }

        bool isAttacking;
        private bool isAlive = true;
        BoxCollider2D obstacleCollider;

        public float MoveSpeed { get { return moveSpeed; } }
        public float AggroRange { get { return aggroRange; } }
        public string CharacterName { get { return characterName; } }
        public int ExperienceWorth { get { return experienceWorth; } }
        public Transform[] ExitPoints { get { return exitPoints; } }
        public float MaxHealthPoints { get { return maxHealthPoints; } }
        public float MaxEnergyPoints { get { return maxEnergyPoints; } }
        public int ExitIndex { get { return GetComponent<CharacterMovementController>().ExitIndex; } }

        public bool IsAttacking
        {
            get { return isAttacking; }
            set { isAttacking = value; }
        }

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        void Awake()
        {
            nameText.text = characterName;
            AddBoxColliderComponent();

            if (StatPanel != null)
            {
                statPanel.SetPrimaryStats(characterStats);
                statPanel.UpdateStatValues();
            }
        }

        public void KillCharacter()
        {
            var enemy = GetComponent<EnemyAI>();

            if (enemy && isAlive)
            {
                var target = enemy.Target;
                enemy.AwardExperience(target);
            }
            isAlive = false;
        }

        void AddBoxColliderComponent()
        {
            obstacleCollider = gameObject.AddComponent<BoxCollider2D>();
            obstacleCollider.size = colliderSize;
            obstacleCollider.offset = colliderOffset;
        }
    }
}