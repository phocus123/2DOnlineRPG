using RPG.CameraUI;
using RPG.Characters;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Core
{
    public class UIManager : MonoBehaviour
    {
        [Header("User Interface")]
        public AbilityBookUI abilityUI;
        public DialogueUI dialogueUI;
        public TargetFrameUI targetFrameUI;
        public MainMenuUI mainMenuUI;

        [Header("Ability Advancement")]
        public AbilityButtonPanel abilityButtonPanel;
        public AbilityDetailsPanel abilityDetailsPanel;
        public AbilityStatsPanel abilityStatsPanel;

        [Header("Experience")]
        [SerializeField] Text playerExperienceText;

        [Header("Action Bar")]
        [SerializeField] ActionButton[] actionButtons;
        [SerializeField ]GameObject[] keybindButtons;

        GameManager gameManager;

        public ActionButton[] ActionButtons { get { return actionButtons; } }
        public DialogueUI DialogueUI { get { return dialogueUI; } }

        void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
            var playerAbilitySystem = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilitySystem>();

            UpdateExperienceText();
            abilityUI.Initialize(playerAbilitySystem);
        }

        public void ToggleInGameMenu()
        {
            mainMenuUI.ToggleInGameMenu();
        }

        public void CloseCanvases()
        {
            mainMenuUI.CloseCanvases(abilityUI);
        }

        public void ShowTargetFrame(GameObject enemy)
        {
            targetFrameUI.ShowTargetFrame(enemy);
        }

        public void HideTargetFrame()
        {
            targetFrameUI.HideTargetFrame();
        }

        public void ShowGuildAbilities(Guild guild)
        {
            abilityButtonPanel.Init(guild);
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

        public void OpenDialogue(NPCControl npc)
        {
            StartCoroutine(UIHelper.FadeCanvasGroup(dialogueUI.dialogueBox));
            dialogueUI.dialogueBox.blocksRaycasts = true;
            dialogueUI.Initialize(npc);
        }

        public void UpdateExperienceText()
        {
            playerExperienceText.text = "Experience: " + gameManager.PlayerExperience.ToString();
        }

        public void ToggleCanvasGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
            canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
            ToggleInGameMenu();
        }
    }
}