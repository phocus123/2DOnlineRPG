using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.CameraUI
{
    public struct DialogueParams
    {
        public string npcName;
        public string[] mainText;
        public string[] acceptButtonText;

        public DialogueParams(string npcName, string[] mainText, string[] acceptButtonText)
        {
            this.npcName = npcName;
            this.mainText = mainText;
            this.acceptButtonText = acceptButtonText;
        }
    }

    [Serializable]
    public class DialogueUI
    {
        public CanvasGroup dialogueBox;
        public Text npcName;
        public Text mainText;
        public Text acceptButtonText;
        public Button acceptButton;
        public Button exitButton;

        [SerializeField ] int linesIndex;
        DialogueParams dialogueParams;

        bool IndexWithinRange { get { return linesIndex < dialogueParams.mainText.Length - 1; } }

        public void Initialize(DialogueParams dialogueParams)
        {
            this.dialogueParams = dialogueParams;
            npcName.text = dialogueParams.npcName;
            UpdateText();
            acceptButton.onClick.AddListener(NextDialogue);
            exitButton.onClick.AddListener(CloseDialogue);
        }

        public IEnumerator FadeBox()
        {
            float rate = 1.0f / 0.25f;
            float progress = 0.0f;

            while (progress <= 1.0)
            {
                dialogueBox.alpha = Mathf.Lerp(0, 1, progress);
                progress += rate * Time.deltaTime;
                yield return null;
            }
        }

        void UpdateText()
        {
            mainText.text = dialogueParams.mainText[linesIndex];
            acceptButtonText.text = dialogueParams.acceptButtonText[linesIndex];
        }

        void CloseDialogue()
        {
            dialogueBox.blocksRaycasts = false;
            dialogueBox.alpha = 0;
            linesIndex = 0;
        }

        void NextDialogue()
        {
            if (IndexWithinRange)
            {
                linesIndex++;
                UpdateText();
            }
        }
    }
}