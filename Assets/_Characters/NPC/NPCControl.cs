using UnityEngine;
using RPG.CameraUI;
using RPG.Core;

namespace RPG.Characters
{
    public class NPCControl : CharacterController
    {
        [SerializeField] float interactionRange;
        [SerializeField] Dialogue npcDialogue;

        CameraRaycaster cameraRaycaster;
        UIManager uIManager;
        PlayerControl player;

        public bool IsPlayerInRange { get { return Vector2.Distance(transform.position, player.transform.position) <= interactionRange; } }

        public override DirectionParams GetDirectionParams()
        {
            DirectionParams directionParams = new DirectionParams(Vector2.zero);
            return directionParams;
        }

        void Start()
        {
            player = FindObjectOfType<PlayerControl>(); // TODO Will this cause issues when there are multiple players?
            uIManager = FindObjectOfType<UIManager>();
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.InvokeOnMouseOverInteractable += OnMouseOverInteractable;
        }

        void OnMouseOverInteractable(NPCControl npc)
        {
            if (Input.GetMouseButtonDown(1) && IsPlayerInRange)
            {
                var characterName = npc.gameObject.GetComponent<Character>().CharacterName;
                DialogueParams dialogueParams = new DialogueParams(characterName, npc.npcDialogue.DialogueLines, npc.npcDialogue.AcceptButtonLines);
                uIManager.OpenDialogue(dialogueParams);
            }
        }
    }
}