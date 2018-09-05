using System.Collections;
using TMPro;
using UnityEngine;
using TMPro;

public enum CombatTextType
{
    NormalDamage,
    Heal,
    CriticalDamage

}

public class CombatText : MonoBehaviour {

    float speed;
    Vector2 direction;
    CombatTextType combatTextType;
    
    void Update()
    {
        float translation = speed * Time.deltaTime;
        transform.Translate(direction * translation);
    }

    public void Initialise(float speed, Vector2 direction, CombatTextType combatTextType)
    {
        this.speed = speed;
        this.direction = direction;
        this.combatTextType = combatTextType;

        Animator controller = GetComponent<Animator>();
        GetComponent<TextMeshProUGUI>().color = GetTextColour();
        float animationLength = controller.runtimeAnimatorController.animationClips[0].length;

        Destroy(gameObject, animationLength);
    }

    Color GetTextColour()
    {
        var tempColor = Color.white;

        switch (combatTextType)
        {
            case CombatTextType.NormalDamage:
                break;
            case CombatTextType.CriticalDamage:
                tempColor = Color.yellow;
                break;
            case CombatTextType.Heal:
                tempColor = Color.green;
                break;
        }

        return tempColor;
    }
}
