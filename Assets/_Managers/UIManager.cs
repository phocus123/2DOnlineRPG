using RPG.Characters;
using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;

namespace RPG.Core
{
    public class UIManager : MonoBehaviour
    {
        [Header("Main Menu")]
        [SerializeField] GameObject targetFrame;
        [SerializeField] CanvasGroup inGameMenu;
        [SerializeField] CanvasGroup keybindMenu;
        [SerializeField] CanvasGroup abilityMenu;

        [Header("Action Bar")]
        [SerializeField] ActionButton[] actionButtons;
        [SerializeField ]GameObject[] keybindButtons;

        [Header("Ability UI")]
        [SerializeField] AbilityUI abilityUI;

        HealthSystem currentTargetHealthSystem;

        public ActionButton[] ActionButtons { get { return actionButtons; } }

        void Awake()
        {
            keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
            var playerAbilitySystem = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilitySystem>();

            abilityUI.Initialize(playerAbilitySystem);
        }

        public void ToggleInGameMenu()
        {
            inGameMenu.alpha = inGameMenu.alpha > 0 ? 0 : 1;
            inGameMenu.blocksRaycasts = inGameMenu.blocksRaycasts == true ? false : true;
        }

        public void ShowTargetFrame(GameObject enemy)
        {
            currentTargetHealthSystem = enemy.GetComponentInParent<HealthSystem>();
            currentTargetHealthSystem.ShowEnemyHealthBar();
            targetFrame.SetActive(true);
            RegisterForHealthEvents();
            currentTargetHealthSystem.Initialize(currentTargetHealthSystem.CurrentHealthPoints, currentTargetHealthSystem.MaxHealthPoints);
        }

        public void CloseCanvases()
        {
            keybindMenu.alpha = 0;
            keybindMenu.blocksRaycasts = false;
            abilityMenu.alpha = 0;
            abilityMenu.blocksRaycasts = false;
            abilityUI.ResetPageIndex();
        }

        public void ToggleCanvasGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
            canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
            ToggleInGameMenu();
        }

        public void UpdateKeyText(string key, KeyCode code)
        {
            Text temp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
            temp.text = code.ToString();
        }

        public void ClickActionButton(string buttonName)
        {
            Array.Find(ActionButtons, x => x.gameObject.name == buttonName).Button.onClick.Invoke();
        }

        public void HideTargetFrame()
        {
            if (currentTargetHealthSystem != null)
            {
                currentTargetHealthSystem.HideEnemyHealthBar();
                targetFrame.SetActive(false);
                DeregisterForHealthEvents();
                currentTargetHealthSystem = null;
            }
        }

        public void UpdateTargetFrame(HealthSystem healthSystem)
        {
            healthSystem.UpdateTargetFrameHealthbar();
        }

        void RegisterForHealthEvents()
        {
            currentTargetHealthSystem.InvokeOnHealthChange += UpdateTargetFrame;
            currentTargetHealthSystem.InvokeOnCharacterDestroyed += HideTargetFrame;
        }

        void DeregisterForHealthEvents()
        {
            currentTargetHealthSystem.InvokeOnHealthChange -= UpdateTargetFrame;
            currentTargetHealthSystem.InvokeOnCharacterDestroyed -= HideTargetFrame;
        }
    }
}