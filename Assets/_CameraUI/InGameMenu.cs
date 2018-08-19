using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] Button keybindsButton;
    [SerializeField] CanvasGroup keybindsCanvasGroup;
    [SerializeField] Button abilityBookButton;
    [SerializeField] CanvasGroup abilityBookCanvasGroup;

    private void Start()
    {
        keybindsButton.onClick.AddListener(() => UIHelper.ToggleCanvasGroup(keybindsCanvasGroup));
        abilityBookButton.onClick.AddListener(() => UIHelper.ToggleCanvasGroup(abilityBookCanvasGroup));
    }
}
