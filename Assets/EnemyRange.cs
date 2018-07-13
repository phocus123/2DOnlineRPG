using UnityEngine;

public class EnemyRange : MonoBehaviour
{
    public delegate void OnAggroRangeEntered(GameObject collisionObject);
    public event OnAggroRangeEntered InvokeOnAggroRangeEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (InvokeOnAggroRangeEntered != null)
            {
                InvokeOnAggroRangeEntered(collision.gameObject);
            }
        }
    }

}
