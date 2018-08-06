using System;
using RPG.Characters;
using UnityEngine.UI;
using UnityEngine;

namespace RPG.CameraUI
{
    [Serializable]
    public class AbilityBookUI
    {
        [Header("Ability UI")]
        public Image abilityIcon;
        public Text title;
        public Text description;
        public Text level;
        public Text type;
        public Text Range;
        public Text damage;
        public Text attackSpeed;
        public Text critChance;
        public Text critDamage;
        public Text energy;
        public Text cooldown;
        public Text pageText;

        [Header("Scripts")]
        [SerializeField] PageButton nextButton;
        [SerializeField] PageButton backButton;

        AbilitySystem playerAbilitySystem;

        int pageIndex = 1;

        public void Initialize(AbilitySystem abilitySystem)
        {
            playerAbilitySystem = abilitySystem;
            nextButton.InvokeOnPageButtonClicked += UpdatePageNumber;
            backButton.InvokeOnPageButtonClicked += UpdatePageNumber;
            LoadAbility(playerAbilitySystem.Abilities[pageIndex - 1]);
            UpdatePageNumberText();
        }

        public void ResetPageIndex()
        {
            pageIndex = 1;
            UpdatePageNumberText();
            LoadAbility(playerAbilitySystem.Abilities[pageIndex - 1]);
        }

        void UpdatePageNumber(GameObject button)
        {
            switch (button.name)
            {
                case "NextButton":
                    if (pageIndex <= 2 ) //TODO hook up to ability array length.
                    {
                        pageIndex++;
                        LoadAbility(playerAbilitySystem.Abilities[pageIndex - 1]);
                        UpdatePageNumberText();
                    }
                    break;
                case "BackButton":
                    if (pageIndex > 1)
                    {
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

        public void LoadAbility(Ability ability)
        {
            level.text = "later";
            abilityIcon.sprite = ability.Icon;
            title.text = ability.name;
            description.text = ability.Description;
            Range.text = ability.Weapon.AttackRange.ToString() + "m";
            attackSpeed.text = ability.AttackSpeed.Value.ToString("F2") + " seconds";

            energy.text = ability.Energy.Value.ToString("F2");
            cooldown.text = ability.Cooldown.ToString("F2") + " seconds";
        }
    }
}