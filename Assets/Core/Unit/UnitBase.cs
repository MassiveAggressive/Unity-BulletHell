using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    [SerializeField] AttributesComponent attributesComponent;

    private void Awake()
    {
        attributesComponent = GetComponent<AttributesComponent>();
        attributesComponent.AttributeChanged += AtributeChanged;
    }

    private void AtributeChanged(object sender, AttributesComponent.AttributeChangedArgs e)
    {
        if(e.name == "Health" && e.newValue <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
