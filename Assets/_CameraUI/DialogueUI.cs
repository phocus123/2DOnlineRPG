using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RPG.Characters;
using System.Collections.Generic;

namespace RPG.CameraUI
{
    [Serializable]
    public class DialogueUI
    {
        public CanvasGroup dialogueBox;
        public CanvasGroup buttonOptions;
        public CanvasGroup dialogueProgress;
        public GameObject buttonParent;
        public Text npcName;
        public Text mainText;
        public Button continueButton;
        public Button returnButton;
        public GameObject dialogueButtonPrefab;
        public Scrollbar scrollbar;

        [SerializeField] int linesIndex = 0;
        [SerializeField] List<Button> optionsButtons = new List<Button>();

        NPCControl npc;

        public int LinesIndex
        {
            get { return linesIndex; }
            set { linesIndex = value; }
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

        public void Initialize(NPCControl npc)
        {
            this.npc = npc;
            string name = npc.GetComponent<Character>().CharacterName;
            npcName.text = name;
            mainText.text = npc.NpcDialogue.NPCIntroduction;
            buttonOptions.alpha = 1;
            buttonOptions.blocksRaycasts = true;
            GenerateOptionButtons();
        }

        void GenerateOptionButtons()
        {
            var options = npc.NpcDialogue.DialogueButtons.buttonOptions;

            for (int i = 0; i < options.Length; i++)
            {
                var prefab = GameObject.Instantiate(dialogueButtonPrefab, buttonParent.transform);
                prefab.GetComponentInChildren<Text>().text = options[i].optionText;
                var button = prefab.GetComponent<Button>();
                optionsButtons.Add(button);

                if (options[i].dialogueEvent != null && !options[i].dialogueEvent.QueryEvent())
                {
                    button.enabled = false;
                    button.GetComponentInChildren<Text>().text = string.Empty;
                }
            }
            AddOptionButtonListeners();
        }

        void AddOptionButtonListeners()
        {
            foreach (Button btn in optionsButtons)
            {
                btn.onClick.AddListener(() => OpenDialogueProcessCanvas(npc, btn)); ;
            }
            optionsButtons[optionsButtons.Count - 1].onClick.RemoveAllListeners();
            optionsButtons[optionsButtons.Count - 1].onClick.AddListener(CloseDialogue); // Last option will always be exit dialogue.
        }

        void OpenDialogueProcessCanvas(NPCControl npc, Button button)
        {
            ToggleCanvases();
            int buttonIndex = optionsButtons.FindIndex(x => x == button);
            mainText.text = npc.NpcDialogue.DialogueLines[buttonIndex, linesIndex];
            UpdateContinuebutton(buttonIndex, linesIndex + 1);
            continueButton.onClick.AddListener(() => NextDialogueLine(buttonIndex));
            returnButton.onClick.AddListener(() => OpenDialogueOptionsCanvas());
            scrollbar.gameObject.SetActive(false);

            if (npc.NpcDialogue.DialogueButtons.buttonOptions[buttonIndex].dialogueEvent)
            {
                npc.NpcDialogue.DialogueButtons.buttonOptions[buttonIndex].dialogueEvent.PerformEventAction(npc);
                button.enabled = false;
                button.GetComponentInChildren<Text>().text = string.Empty;
            }
        }

        void NextDialogueLine(int optionIndex)
        {
            linesIndex++;
            mainText.text = npc.NpcDialogue.DialogueLines[optionIndex, linesIndex];
            UpdateContinuebutton(optionIndex, linesIndex + 1);
        }

        void UpdateContinuebutton(int optionIndex, int index)
        {
            var dialogueLength = npc.NpcDialogue.DialogueLines[optionIndex].Length;

            if (index < dialogueLength)
            {
                continueButton.enabled = true;
                continueButton.GetComponentInChildren<Text>().text = "Continue";
            }
            else
            {
                continueButton.enabled = false;
                continueButton.GetComponentInChildren<Text>().text = string.Empty;
            }
        }

        void OpenDialogueOptionsCanvas()
        {
            ToggleCanvases();
            continueButton.onClick.RemoveAllListeners();
            returnButton.onClick.RemoveAllListeners();
            mainText.text = npc.NpcDialogue.NPCIntroduction;
            LinesIndex = 0;
            scrollbar.gameObject.SetActive(false);
        }

        void ToggleCanvases()
        {
            buttonOptions.alpha = buttonOptions.alpha > 0 ? 0 : 1;
            buttonOptions.blocksRaycasts = buttonOptions.blocksRaycasts == true ? false : true;
            dialogueProgress.alpha = dialogueProgress.alpha > 0 ? 0 : 1;
            dialogueProgress.blocksRaycasts = dialogueProgress.blocksRaycasts == true ? false : true;
        }

        void CloseDialogue()
        {
            linesIndex = 0;
            CloseAllCanvases();
            ClearButtons();
        }

        void ClearButtons()
        {
            foreach (Button btn in optionsButtons)
            {
                GameObject.Destroy(btn.gameObject);
            }
            optionsButtons.Clear();
        }

        void CloseAllCanvases()
        {
            dialogueBox.alpha = 0;
            buttonOptions.alpha = 0;
            dialogueProgress.alpha = 0;
            dialogueBox.blocksRaycasts = false;
            buttonOptions.blocksRaycasts = false;
            dialogueProgress.blocksRaycasts = false;
            scrollbar.value = 1;
        }
    }
}