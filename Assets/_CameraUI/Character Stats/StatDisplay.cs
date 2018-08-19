using TMPro;
using UnityEngine;

namespace RPG.CameraUI
{
    public class StatDisplay : MonoBehaviour
    {
        public TextMeshProUGUI NameText;
        public TextMeshProUGUI ValueText;

        void OnValidate()
        {
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            NameText = texts[0];
            ValueText = texts[1];
        }
    }
}