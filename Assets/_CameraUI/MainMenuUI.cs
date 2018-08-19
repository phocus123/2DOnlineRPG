using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Core;

namespace RPG.CameraUI
{
    public class MainMenuUI :MonoBehaviour
    {
        [SerializeField] CanvasGroup inGameMenu;
        [SerializeField] CanvasGroup keybindMenu;
        [SerializeField] CanvasGroup abilityMenu;
        [SerializeField] Button keybindsButton;
        [SerializeField] Button abilityBookButton;

        UIManager uIManager;

        private void Start()
        {
            uIManager = FindObjectOfType<UIManager>();
            keybindsButton.onClick.AddListener(ToggleKeybindsMenu);
            abilityBookButton.onClick.AddListener(ToggleAbilityBookMenu);
        }

        public void ToggleInGameMenu()
        {
            inGameMenu.alpha = inGameMenu.alpha > 0 ? 0 : 1;
            inGameMenu.blocksRaycasts = inGameMenu.blocksRaycasts == true ? false : true;
        }

        public void CloseCanvases()
        {
            keybindMenu.alpha = 0;
            keybindMenu.blocksRaycasts = false;
            abilityMenu.alpha = 0;
            abilityMenu.blocksRaycasts = false;
            uIManager.abilityUI.ResetPageIndex();
        }

        void ToggleKeybindsMenu()
        {
            UIHelper.ToggleCanvasGroup(keybindMenu);
            ToggleInGameMenu();
        }

        void ToggleAbilityBookMenu()
        {
            UIHelper.ToggleCanvasGroup(abilityMenu);
            ToggleInGameMenu();
        }
    }
}