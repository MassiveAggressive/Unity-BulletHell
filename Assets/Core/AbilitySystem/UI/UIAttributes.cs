using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAttributes : MonoBehaviour
{
    [SerializeField] AttributesContainerComponent attributesComponent;
    private Dictionary<string, UIAttribute> attributes = new Dictionary<string, UIAttribute>();

    [SerializeField] GameObject UIAttributePrefab;
    [SerializeField] Transform UIAttributePanel;

    private void Start()
    {
        attributesComponent.AttributesChanged += AttributesChanged;
        AttributesChanged(attributesComponent, new AttributesContainerComponent.AttributesChangedArgs { attributes = attributesComponent.GetAttributes() });
    }

    private void AttributesChanged(object sender, AttributesContainerComponent.AttributesChangedArgs e)
    {
        foreach(string attributeName in attributes.Keys) 
        {
            Destroy(attributes[attributeName].gameObject);
        }
        attributes.Clear();

        foreach (string attributeName in e.attributes.Keys) 
        {
            GameObject UIAttributeObject = Instantiate(UIAttributePrefab);
            UIAttributeObject.transform.SetParent(UIAttributePanel.GetChild(0).transform, false);
            UIAttributeObject.transform.position = Vector3.zero;

            UIAttribute uIAttribute = UIAttributeObject.GetComponent<UIAttribute>();
            uIAttribute.attributeName = attributeName;
            uIAttribute.attributeValue = e.attributes[attributeName];

            attributes[attributeName] = uIAttribute;
        }
    }
}
