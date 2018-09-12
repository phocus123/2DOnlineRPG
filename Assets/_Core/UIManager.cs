using RPG.CameraUI;
using RPG.Characters;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Core
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("User Interface")]
        public AbilityBook abilityUI; 
        public DialogueUI dialogueUI;
        public TargetPanel targetPanel;
        public MainMenuUI mainMenuUI;
        public CanvasGroup CharacterPanel;
        public DebuffPanel targetDebuffPanel;
        public DebuffPanel playerDebuffPanel;
        public AlertMessageController alertMessageController;
        public Castbar castbar;

        [Header("Ability Advancement")]
        public AbilityButtonPanel abilityButtonPanel;
        public AbilityDetailsPanel abilityDetailsPanel;
        public AbilityStatsPanel abilityStatsPanel;

        [Header("Experience")]
        [SerializeField] Text playerExperienceText;

        [Header("Action Bar")]
        [SerializeField] ActionButton[] actionButtons;
        [SerializeField] GameObject[] keybindButtons;

        [Header("Combat Text")]
        [SerializeField] GameObject combatTextPrefab;
        [SerializeField] Canvas combatTextCanvas;
        [SerializeField] float speed;

        public ActionButton[] ActionButtons { get { return actionButtons; } }
        public DialogueUI DialogueUI { get { return dialogueUI; } }

        void Awake()
        {
            keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
            UpdateExperienceText();
        }

        public void ShowTargetFrame(GameObject enemy)
        {
            targetPanel.ShowTargetPanel(enemy);
        }

        public void HideTargetFrame()
        {
            targetPanel.HideTargetPanel();
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

        public void UpdateExperienceText()
        {
            playerExperienceText.text = "Experience: " + GameManager.Instance.PlayerExperience.ToString();
        }

        public void TriggerCombatText(Vector2 position, float healthValue, CombatTextType combatTextType)
        {
            GameObject combatText = Instantiate(combatTextPrefab, position, Quaternion.identity, combatTextCanvas.transform);
            combatText.GetComponent<CombatText>().Initialise(combatTextType);
            combatText.GetComponent<TextMeshProUGUI>().text = healthValue.ToString();
        }
    }
}