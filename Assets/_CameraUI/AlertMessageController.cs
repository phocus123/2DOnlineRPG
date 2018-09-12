using UnityEngine;
using TMPro;
using System.Collections;

namespace RPG.CameraUI
{
    public enum AlertMessageType
    {
        NoTarget,
        TargetNotInRange,
        TargetNotInLineOfSight,
        TargetNotAlive,
        CharacterNotAlive,
        CorrectWeaponNotEquipped,
        AbilityOnCooldown,
        IsAttacking,
        IsMoving,
        None
    }

    public class AlertMessageController : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        TextMeshProUGUI alertText;
        string message;
        bool isActive = false;

        void Start()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            if (alertText == null)
                alertText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void TriggerAlert(AlertMessageType messageType)
        {
            if (!isActive)
            {
                alertText.text = DetermineMessage(messageType);
                StartCoroutine(FadeAlertIn());
            }
        }

        IEnumerator FadeAlertIn()
        {
            isActive = true;
            float rate = 5f;
            float progress = 0.0f;

            while (progress <= 1.0)
            {
                canvasGroup.alpha = progress;
                progress += rate * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(.5f);
            StartCoroutine(FadeAlertOut());
        }

        IEnumerator FadeAlertOut()
        {
            float rate = 5f;
            float progress = 1f;

            while (progress >= 0)
            {
                canvasGroup.alpha = progress;
                progress -= rate * Time.deltaTime;
                yield return null;
            }

            alertText.text = string.Empty;
            isActive = false;
        }

        string DetermineMessage(AlertMessageType messageType)
        {
            switch (messageType)
            {
                case AlertMessageType.NoTarget:
                    message = "No target selected.";
                    break;
                case AlertMessageType.TargetNotInRange:
                    message = "Target not in range.";
                    break;
                case AlertMessageType.TargetNotInLineOfSight:
                    message = "Target not in line of sight.";
                    break;
                case AlertMessageType.TargetNotAlive:
                    message = "Target is dead.";
                    break;
                case AlertMessageType.CharacterNotAlive:
                    message = "You are dead.";
                    break;
                case AlertMessageType.CorrectWeaponNotEquipped:
                    message = "Correct weapon not equipped.";
                    break;
                case AlertMessageType.AbilityOnCooldown:
                    message = "Ability is on cooldown.";
                    break;
                default:
                    break;
            }

            return message;
        }
    }
}