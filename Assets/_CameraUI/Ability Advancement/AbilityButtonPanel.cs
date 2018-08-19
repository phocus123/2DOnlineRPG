using RPG.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.CameraUI
{
    public class AbilityButtonPanel : MonoBehaviour
    {
        public CanvasGroup guildAbilityCanvas;
        public Text title;
        public GameObject buttonPrefab;

        [SerializeField] CanvasGroup abilityAdvancementCanvasGroup;
        [SerializeField] CanvasGroup upgradeCanvasGroup;
        [SerializeField] CanvasGroup abilityDetailsCanvasGroup;
        [SerializeField] CanvasGroup abilityStatsCanvasGroup;
        [Space]
        [SerializeField] AbilityDetailsPanel abilityDetailsPanel;
        [SerializeField] AbilityStatsPanel abilityStatsPanel;

        Guild currentGuild;
        List<GameObject> abilityButtons = new List<GameObject>();

        public event Action<GameObject> OnAbilityButtonCreated;
        public event Action<Ability> OnAbilityCanvasClosed;

        void Start()
        {
            abilityStatsPanel.OnAcceptButtonClicked += RefreshButtonDetails;
        }

        public void Init(Guild currentGuild)
        {
            this.currentGuild = currentGuild;

            title.text = currentGuild.name;
            UIHelper.ToggleCanvasGroup(guildAbilityCanvas);
            LoadAbilitiesButtons();
        }

        void LoadAbilitiesButtons()
        {
            GameObject button = null;

            foreach (Ability ability in currentGuild.GuildAbilities)
            {
                button = Instantiate(buttonPrefab, gameObject.transform);
                var abilityButton = button.GetComponent<AbilityButton>();
                abilityButton.AbilityNameText.text = ability.name;
                abilityButton.AbilityLevelText.text = "Level: " + ability.Level.ToString(); 
                button.GetComponent<Button>().onClick.AddListener(() => ShowAbilityDetailsPanel(ability));
                abilityButtons.Add(button);
                ability.AttachAbilityUITo(button);
            }

            OnAbilityButtonCreated(button);
        }

        public void CloseGuildAbilitiesCanvas()
        {
            foreach (GameObject button in abilityButtons)
            {
                button.GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(button.gameObject);
            }

            if (abilityDetailsPanel.CurrentSelectedAbility != null)
            {
                OnAbilityCanvasClosed(abilityDetailsPanel.CurrentSelectedAbility);
            }

            UIHelper.CloseAllCanvasGroups(abilityAdvancementCanvasGroup, upgradeCanvasGroup, abilityDetailsCanvasGroup, abilityStatsCanvasGroup);
            abilityButtons.Clear();
        }

        void ShowAbilityDetailsPanel(Ability ability)
        {
            abilityDetailsPanel.Init(ability);
        }

        void RefreshButtonDetails()
        {
            var button = abilityButtons.Find(x => x.GetComponent<AbilityUI>().Ability == abilityDetailsPanel.CurrentSelectedAbility);
            var abilityButton = button.GetComponent<AbilityButton>();
            abilityButton.AbilityNameText.text = abilityDetailsPanel.CurrentSelectedAbility.name;
            abilityButton.AbilityLevelText.text = "Level: " + abilityDetailsPanel.CurrentSelectedAbility.Level.ToString();
        }
    }
}