using System.Collections;
using TMPro;
using UnityEngine;

public class CombatText : MonoBehaviour {

    float speed;
    Vector2 direction;
    
    void Update()
    {
        float translation = speed * Time.deltaTime;
        transform.Translate(direction * translation);
    }

    public void Initialise(float speed, Vector2 direction)
    {
        this.speed = speed;
        this.direction = direction;

        Animator controller = GetComponent<Animator>();
        float animationLength = controller.runtimeAnimatorController.animationClips[0].length;

        Destroy(gameObject, animationLength);
    }
}
