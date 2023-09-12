using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    [SerializeField] AttributesContainerComponent attributesComponent;

    private void Awake()
    {
        attributesComponent = GetComponent<AttributesContainerComponent>();
        attributesComponent.AttributeChanged += AtributeChanged;
    }

    private void AtributeChanged(object sender, AttributesContainerComponent.AttributeChangedArgs e)
    {
        if(e.name == "Health" && e.newValue <= 0)
        {
            //Destroy(this.gameObject);
        }
    }
}
