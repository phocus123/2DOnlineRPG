using UnityEngine;

namespace RPG.Characters
{
    public class PlayerControl : CharacterController
    {
        Vector2 direction;
        private Vector3 min, max;

        public override Vector2 GetDirection()
        {
            return direction;
        }

        private void Update()
        {
            GetInput();
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);
        }

        public void GetInput()
        {
            direction = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                //exitIndex = 0;
                direction += Vector2.up;
            }
            if (Input.GetKey(KeyCode.A))
            {
                //exitIndex = 3;
                direction += Vector2.left;
            }
            if (Input.GetKey(KeyCode.S))
            {
                //exitIndex = 2;
                direction += Vector2.down;
            }
            if (Input.GetKey(KeyCode.D))
            {
                //exitIndex = 1;
                direction += Vector2.right;
            }
        }

        public void SetLimits(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }
    }
}