using RPG.Characters;
using RPG.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.CameraUI
{
    public class GuildAbilityButton : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            var guildAbilityUI = FindObjectOfType<UIManager>().GuildAbilityUI;
            var abilityButtons = guildAbilityUI.AbilityButtons;
            var button = abilityButtons.Find(x => x.gameObject == gameObject);
            var abilityIndex = guildAbilityUI.SelectedAbilityIndex = abilityButtons.IndexOf(button);
            var ability = guildAbilityUI.CurrentGuild.GuildAbilities[abilityIndex];

            guildAbilityUI.upgradeButton.onClick.RemoveAllListeners();
            guildAbilityUI.upgradeButton.onClick.AddListener(() => guildAbilityUI.LoadAbilityStats(ability));
        }
    }
}