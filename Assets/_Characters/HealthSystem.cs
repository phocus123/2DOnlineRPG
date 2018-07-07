using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class HealthSystem : MonoBehaviour
    {
        public delegate void OnStatChange(float value, HealthSystem healthSystem);
        public event OnStatChange InvokeOnStatChange;

        [Header("Health")]
        [SerializeField] float maxHealthPoints;
        [SerializeField] float currentHealthPoints;
        [SerializeField] Image healthBar;

        //Animator animator;

        [Header("Enemy Target Frame")]
        [SerializeField] Image targetFrameHealthbar;
        [SerializeField] CanvasGroup enemyHealthCanvas;
        [SerializeField] Text targetHealthbarText;

        public float CurrentHealthPoints
        {
            get { return currentHealthPoints; }
            set
            {
                currentHealthPoints = Mathf.Clamp(value, 0, maxHealthPoints);
                if (targetHealthbarText != null)
                {
                    targetHealthbarText.text = Mathf.Round(currentHealthPoints) + "/" + maxHealthPoints;
                }
            }
        }

        public float MaxHealthPoints { get { return maxHealthPoints; } }
        public float HealthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        void Start()
        {
            //animator = GetComponent<Animator>();
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
            print(currentHealthPoints);
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            InvokeOnStatChange(currentHealthPoints, this);
        }

        void UpdateHealthBar()
        {
            if (healthBar) // Enemies may not have health bars to update
            {
                healthBar.fillAmount = HealthAsPercentage;
            }
        }
    }
}