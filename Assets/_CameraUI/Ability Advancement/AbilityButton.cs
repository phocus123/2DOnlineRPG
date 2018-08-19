using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.CameraUI
{
    public class AbilityButton : MonoBehaviour
    {
        public Text AbilityNameText;
        public Text AbilityLevelText;

        void OnValidate()
        {
            Text[] texts = GetComponentsInChildren<Text>();
            AbilityNameText = texts[0];
            AbilityLevelText = texts[1];
        }

        public event Action<AbilityButton> OnButtonDestroyed;

        private void OnDestroy()
        {
            if (OnButtonDestroyed != null)
            {
                OnButtonDestroyed(this);
            }
        }
    }
}