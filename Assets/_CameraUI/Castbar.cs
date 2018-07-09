using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RPG.Characters;

namespace RPG.CameraUI
{
    [Serializable]
    public class Castbar : MonoBehaviour
    {
        public Image castingBar;
        public Text casttime;
        public Text skillName;
        public Image icon;
        public CanvasGroup castbarCanvas;

        private Coroutine castRoutine;
        private Coroutine fadeRoutine;

        public void TriggerCastBar(Ability ability)
        {
            SetupCastbar(ability);
            castRoutine = StartCoroutine(Progress((ability))); 
            fadeRoutine = StartCoroutine(FadeBar());
        }

        public void SetupCastbar(Ability ability)
        {
            castingBar.fillAmount = 0;
            castingBar.color = ability.GuildMaterial.color;
            skillName.text = ability.name;
            icon.sprite = ability.Icon;
        }

        public void StopCasting()
        {
            StopAllCoroutines();
            HideCastbar();
        }

        private IEnumerator Progress(Ability ability)
        {
            float timePassed = Time.deltaTime;
            float rate = 1.0f / ability.AttackSpeed;
            float progress = 0.0f;

            while (progress <= 1.0)
            {
                castingBar.fillAmount = Mathf.Lerp(0, 1, progress);
                progress += rate * Time.deltaTime;
                timePassed += Time.deltaTime;
                casttime.text = (ability.AttackSpeed - timePassed).ToString("F2");

                if (ability.AttackSpeed - timePassed < 0)
                {
                    casttime.text = "0.00";
                }
                yield return null;
            }

            HideCastbar();
        }

        private IEnumerator FadeBar()
        {
            float rate = 1.0f / 0.25f;
            float progress = 0.0f;

            while (progress <= 1.0)
            {
                castbarCanvas.alpha = Mathf.Lerp(0, 1, progress);
                progress += rate * Time.deltaTime;
                yield return null;
            }
        }

        private void HideCastbar()
        {
            castbarCanvas.alpha = 0;
        }
    }
}