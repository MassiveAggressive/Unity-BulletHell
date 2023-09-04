using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySystemComponent : MonoBehaviour
{
    Dictionary<GameObject, AbilityBase> abilities = new Dictionary<GameObject, AbilityBase>();
    public void AddAbility(GameObject ability)
    {
        AbilityBase abilityBase = GetComponent<AbilityBase>();

        if(abilityBase)
        {
            abilities[ability] = abilityBase;
        }

        print(abilities.Keys.Count);
    }
}
