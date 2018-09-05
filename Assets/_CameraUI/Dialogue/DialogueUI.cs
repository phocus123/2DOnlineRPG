using RPG.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.CameraUI
{
    [Serializable]
    public class DialogueUI : MonoBehaviour
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

        int linesIndex = 0;
        List<Button> optionsButtons = new List<Button>();
        NPCControl npc;

        public void Initialize(NPCControl npc)
        {
            this.npc = npc;
            npcName.text = npc.GetComponent<Character>().CharacterName; 
            mainText.text = npc.NpcDialogue.NPCIntroduction;
            UIHelper.ToggleCanvasGroup(buttonOptions);

            GenerateOptionButtons();
        }

        public void OpenDialogue(NPCControl npc)
        {
            UIHelper.ToggleCanvasGroup(dialogueBox);
            Initialize(npc);
        }

        public void CloseDialogue()
        {
            UIHelper.CloseAllCanvasGroups(dialogueBox, buttonOptions, dialogueProgress);
            linesIndex = 0;
            scrollbar.value = 1;
            ClearButtons();
            npc = null;
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
                // TODO Come up with a solution for hiding a button when event has been completed. ie player was just invited to a guild.
            }

            foreach (Button btn in optionsButtons)
            {
                btn.onClick.AddListener(() => OpenDialogueProcessCanvas(npc, btn)); ;
            }
        }

        void OpenDialogueProcessCanvas(NPCControl npc, Button button)
        {
            ToggleCanvases();
            int buttonIndex = optionsButtons.FindIndex(x => x == button);
            mainText.text = npc.NpcDialogue.DialogueLines[buttonIndex, linesIndex];
            scrollbar.gameObject.SetActive(false);
            UpdateContinueButton(buttonIndex, linesIndex + 1);
            continueButton.onClick.AddListener(() => NextDialogueLine(buttonIndex));
            returnButton.onClick.AddListener(() => OpenDialogueOptionsCanvas());
            if (npc.NpcDialogue.DialogueButtons.buttonOptions[buttonIndex].dialogueEvent)
            {
                npc.NpcDialogue.DialogueButtons.buttonOptions[buttonIndex].dialogueEvent.PerformEventAction(npc);
            }
        }

        void NextDialogueLine(int optionIndex)
        {
            linesIndex++;
            mainText.text = npc.NpcDialogue.DialogueLines[optionIndex, linesIndex];
            UpdateContinueButton(optionIndex, linesIndex + 1);
        }

        void UpdateContinueButton(int optionIndex, int index)
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
            linesIndex = 0;
            scrollbar.gameObject.SetActive(false);
        }

        void ToggleCanvases()
        {
            UIHelper.ToggleCanvasGroup(buttonOptions);
            UIHelper.ToggleCanvasGroup(dialogueProgress);
        }

        void ClearButtons()
        {
            foreach (Button btn in optionsButtons)
            {
                btn.onClick.RemoveAllListeners();
                GameObject.Destroy(btn.gameObject);
            }
            continueButton.onClick.RemoveAllListeners();
            returnButton.onClick.RemoveAllListeners();
            optionsButtons.Clear();
        }
    }
}