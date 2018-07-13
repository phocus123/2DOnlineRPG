using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class HealthSystem : MonoBehaviour
    {
        public delegate void OnHealthChange(HealthSystem healthSystem);
        public event OnHealthChange InvokeOnHealthChange;

        public delegate void OnCharacterDeath();
        public event OnCharacterDeath InvokeOnCharacterDeath;

        [Header("Health")]
        [SerializeField] float maxHealthPoints;
        [SerializeField] Image healthBar;
        [SerializeField] Text playerHealthbarText;

        [Header("Enemy Target Frame")]
        [SerializeField] Image targetFrameHealthbar;
        [SerializeField] CanvasGroup enemyHealthCanvas;
        [SerializeField] Text targetHealthbarText;

        Animator animator;
        Character character;
        [SerializeField] float currentHealthPoints;

        public float MaxHealthPoints { get { return maxHealthPoints; } }
        public float HealthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }
        public float CurrentHealthPoints
        {
            get { return currentHealthPoints; }
            set
            {
                currentHealthPoints = Mathf.Clamp(value, 0, maxHealthPoints);
                if (InvokeOnHealthChange != null)
                {
                    InvokeOnHealthChange(this);
                }
            }
        }

        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();
            currentHealthPoints = maxHealthPoints;
        }

        void Update()
        {
            UpdateHealthBar();
        }

        public void Initialize(float currentValue, float maxValue)
        {
            maxHealthPoints = maxValue;
            currentHealthPoints = currentValue;
            targetHealthbarText.text = Mathf.Round(currentHealthPoints) + "/" + maxHealthPoints;
            UpdateTargetFrameHealthbar();
        }

        public void UpdateTargetFrameHealthbar()
        {
            if (targetFrameHealthbar)
            {
                targetFrameHealthbar.fillAmount = HealthAsPercentage;
                targetHealthbarText.text = Mathf.Round(currentHealthPoints) + "/" + maxHealthPoints;
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
            CurrentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);

            if (characterDies)
            {
                KillCharacter();
            }
        }

        public void KillCharacter()
        {
            character.KillCharacter();
            animator.SetTrigger("Die");
            InvokeOnCharacterDeath();
        }

        void UpdateHealthBar()
        {
            if (healthBar) // Enemies may not have health bars to update
            {
                healthBar.fillAmount = HealthAsPercentage;
            }
            if (playerHealthbarText)
            {
                playerHealthbarText.text = Mathf.Round(currentHealthPoints) + "/" + maxHealthPoints;
            }
            if (InvokeOnHealthChange != null)
            {
                InvokeOnHealthChange(this);
            }
        }
    }
}