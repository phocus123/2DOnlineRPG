using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class HealthController : MonoBehaviour
    {

        [Header("Health")]
        [SerializeField] Image healthBar;
        [SerializeField] Text playerHealthbarText;

        [Header("Enemy Target Frame")]
        [SerializeField] Image targetFrameHealthbar;
        [SerializeField] CanvasGroup enemyHealthCanvas;
        [SerializeField] Text targetHealthbarText;

        Character characterManager;
        [SerializeField] float currentHealthPoints;

        public float HealthAsPercentage { get { return currentHealthPoints / characterManager.MaxHealthPoints; } }
        public float CurrentHealthPoints
        {
            get { return currentHealthPoints; }
            set
            {
                currentHealthPoints = Mathf.Clamp(value, 0, characterManager.MaxHealthPoints);
                OnHealthChange(this);

            }
        }

        public event Action<HealthController> OnHealthChange = delegate { };
        public event Action OnCharacterDeath = delegate { };

        void Start()
        {
            characterManager = GetComponent<Character>();
            currentHealthPoints = characterManager.MaxHealthPoints;
        }

        void Update()
        {
            UpdateHealthBar();
        }

        public void Initialize(float currentValue)
        {
            currentHealthPoints = currentValue;
            targetHealthbarText.text = Mathf.Round(currentHealthPoints) + "/" + characterManager.MaxHealthPoints;
            UpdateTargetFrameHealthbar();
        }

        public void UpdateTargetFrameHealthbar()
        {
            if (targetFrameHealthbar)
            {
                targetFrameHealthbar.fillAmount = HealthAsPercentage;
                targetHealthbarText.text = Mathf.Round(currentHealthPoints) + "/" + characterManager.MaxHealthPoints;
            }
        }

        public void ShowEnemyHealthBar()
        {
            enemyHealthCanvas.alpha = 1;
        }

        public void HideEnemyHealthBar()
        {
            enemyHealthCanvas.alpha = 0;
        }

        public void TakeDamage(float damage)
        {
            bool characterDies = (currentHealthPoints - damage <= 0);
            CurrentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, characterManager.MaxHealthPoints);

            if (characterDies)
            {
                KillCharacter();
            }
        }

        public void KillCharacter()
        {
            characterManager.KillCharacter();
            OnCharacterDeath();
        }

        void UpdateHealthBar()
        {
            if (healthBar)
            {
                healthBar.fillAmount = HealthAsPercentage;
            }
            if (playerHealthbarText)
            {
                playerHealthbarText.text = Mathf.Round(currentHealthPoints) + "/" + characterManager.MaxHealthPoints;
            }

            OnHealthChange(this);
        }
    }
}