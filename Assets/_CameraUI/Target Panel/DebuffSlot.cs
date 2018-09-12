using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.Characters;
using System.Collections;

namespace RPG.CameraUI
{
    public class DebuffSlot : MonoBehaviour
    {
        public Image DebuffIcon;
        public Image DurationDarkmask;
        public TextMeshProUGUI DurationText;

        void OnValidate()
        {
            if (DebuffIcon == null)
                DebuffIcon = GetComponent<Image>();
            if (DurationDarkmask == null)
                DurationDarkmask = GetComponentInChildren<Image>();
            if (DurationText == null)
                DurationText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Init(Ability ability, float currentDuration, float maxDuration)
        {
            DebuffIcon.sprite = ability.icon;
            DebuffIcon.color = Color.grey;
            DurationDarkmask.sprite = ability.icon;
            DurationDarkmask.color = Color.white;

            StartCoroutine(DisplayAbilityDebuff(currentDuration, maxDuration));
        }

        IEnumerator DisplayAbilityDebuff(float currentDuration, float maxDuration)
        {
            while (maxDuration > currentDuration)
            {
                currentDuration += Time.deltaTime;
                DurationDarkmask.fillAmount = currentDuration / maxDuration;
                DurationText.text = Mathf.Round(maxDuration - currentDuration).ToString();
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}