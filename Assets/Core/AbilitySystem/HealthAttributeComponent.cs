using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAttributeComponent : AttributesComponent
{
    public override void PostAttributeChanged(string name, float oldValue, float newValue)
    {
        base.PostAttributeChanged(name, oldValue, newValue);

        if(name == "Health")
        {
            attributes[name] = Mathf.Clamp(newValue, 0f, attributes["MaxHealth"]);
        }
    }
}
