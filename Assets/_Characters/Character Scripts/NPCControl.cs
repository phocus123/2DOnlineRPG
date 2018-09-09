using UnityEngine;
using RPG.CameraUI;
using RPG.Core;

namespace RPG.Characters
{
    public class NPCControl : CharacterMovementController
    {
        [SerializeField] float interactionRange;
        [SerializeField] Dialogue npcDialogue;

        UIManager uIManager;
        PlayerControl player;

        public bool IsPlayerInRange { get { return Vector2.Distance(transform.position, player.transform.position) <= interactionRange; } }
        public Dialogue NpcDialogue { get { return npcDialogue; } }

        void Start()
        {
            player = GameManager.Instance.player; // TODO Will this cause issues when there are multiple players?
            uIManager = GameManager.Instance.uIManager;
        }
    }
}