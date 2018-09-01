using System;
using RPG.Characters;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace RPG.CameraUI
{
    [Serializable]
    public class AbilityBook : MonoBehaviour
    {
        [Header("Ability UI")]
        public Image abilityIcon;
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        public TextMeshProUGUI pageText;

        public GameObject statParent;
        public GameObject statPrefab;

        [SerializeField] PageButton nextButton;
        [SerializeField] PageButton backButton;

        AbilitySystem playerAbilitySystem;
        List<GameObject> stats = new List<GameObject>();

        int pageIndex = 1;

        void Awake()
        {
            playerAbilitySystem = FindObjectOfType<PlayerControl>().gameObject.GetComponent<AbilitySystem>();
            MainMenuUI.OnAbilityBookToggledOn += Init;
            MainMenuUI.OnAbilityBookToggledOff += Close;
            nextButton.InvokeOnPageButtonClicked += ChangePage;
            backButton.InvokeOnPageButtonClicked += ChangePage;
        }

        void Init()
        {
            LoadAbility(playerAbilitySystem.Abilities[pageIndex - 1]);
            UpdatePageNumberText();
        }

        void Close()
        {
            pageIndex = 1;
            UpdatePageNumberText();
            DestroyStats();
        }

        void ChangePage(GameObject button)
        {
            switch (button.name)
            {
                case "NextButton":
                    if (pageIndex < playerAbilitySystem.Abilities.Length)
                    {
                        DestroyStats();
                        pageIndex++;
                        //Debug.Log(string.Format("index: {0}  ability length: {1}", pageIndex, playerAbilitySystem.Abilities.Length)); // For later testing if more abilities are added to the ability array.
                        LoadAbility(playerAbilitySystem.Abilities[pageIndex - 1]);
                        UpdatePageNumberText();
                    }
                    break;
                case "BackButton":
                    if (pageIndex > 1)
                    {
                        DestroyStats();
                        pageIndex--;
                        LoadAbility(playerAbilitySystem.Abilities[pageIndex - 1]);
                        UpdatePageNumberText();
                    }
                    break;
            }
        }

        void UpdatePageNumberText()
        {
            pageText.text = pageIndex.ToString();
        }

        void LoadAbility(Ability ability)
        {
            abilityIcon.sprite = ability.Icon;
            title.text = ability.name;
            description.text = ability.Description;
            ability.AttachAbilityUITo(this.gameObject);
            CreateStats(ability);
        }

        void CreateStats(Ability ability)
        {
            foreach (AbilityStat stat in ability.AbilityUI.Stats)
            {
                var statObject = Instantiate(statPrefab, statParent.transform);
                var statOperatorParent = statObject.transform.GetChild(2).gameObject;
                var statDisplay = statObject.GetComponent<StatDisplay>();
                statDisplay.NameText.text = stat.statName;
                statDisplay.ValueText.text = stat.Value.ToString();
                stats.Add(statObject);
            }
        }

        private void DestroyStats()
        {
            foreach (GameObject statObject in stats)
            {
                Destroy(statObject);
            }

            stats.Clear();
            Destroy(GetComponent<AbilityUI>());
        }
    }
}