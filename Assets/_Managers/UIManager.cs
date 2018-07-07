using UnityEngine;
using RPG.Characters;
using RPG.Core;

namespace RPG.CameraUI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject targetFrame;
        [SerializeField] GameManager gameManager;

        HealthSystem currentTargetHealthSystem;

        private void Start()
        {
             gameManager.InvokeOnEnemyClicked += new GameManager.OnEnemyClicked(ShowTargetFrame);
             gameManager.InvokeOnNonEnemyClicked += new GameManager.OnNonEnemyClicked(HideTargetFrame);         
        }

        public void ShowTargetFrame(GameObject enemy)
        {
            currentTargetHealthSystem = enemy.GetComponentInParent<HealthSystem>();
            targetFrame.SetActive(true);
            currentTargetHealthSystem.ShowEnemyHealthBar();

            currentTargetHealthSystem.Initialize(currentTargetHealthSystem.CurrentHealthPoints, currentTargetHealthSystem.MaxHealthPoints);
            currentTargetHealthSystem.InvokeOnStatChange += new HealthSystem.OnStatChange(UpdateTargetFrame);

            //target.InvokeOnCharacterRemoved += new CharacterOld.OnCharacterRemoved(HideTargetFrame);
        }

        public void HideTargetFrame()
        {
            if (currentTargetHealthSystem != null)
            {
                targetFrame.SetActive(false);
                currentTargetHealthSystem.HideEnemyHealthBar();
                currentTargetHealthSystem = null;
            }
        }

        public void UpdateTargetFrame(float value, HealthSystem healthSystem)
        {
            healthSystem.CurrentHealthPoints = value;
            healthSystem.UpdateTargetFrameHealthbar();
        }
    }
}