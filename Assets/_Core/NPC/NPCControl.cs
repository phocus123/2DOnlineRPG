using UnityEngine;
using RPG.CameraUI;
using RPG.Core;

namespace RPG.Characters
{
    public class NPCControl : CharacterController
    {
        [SerializeField] float interactionRange;
        [SerializeField] Dialogue npcDialogue;

        UIManager uIManager;
        PlayerControl player;

        public bool IsPlayerInRange { get { return Vector2.Distance(transform.position, player.transform.position) <= interactionRange; } }
        public Dialogue NpcDialogue { get { return npcDialogue; } }

        public override DirectionParams GetDirectionParams()
        {
            DirectionParams directionParams = new DirectionParams(Vector2.zero);
            return directionParams;
        }

        void Start()
        {
            player = FindObjectOfType<PlayerControl>(); // TODO Will this cause issues when there are multiple players?
            uIManager = FindObjectOfType<UIManager>();
        }
    }
}