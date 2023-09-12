using AttributeData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttributesContainerComponent))]
public class HealthAttributeDriver : MonoBehaviour
{
    [SerializeField] AttributesContainerComponent attributesContainer;

    [SerializeField] S_AttributeData defaultAttributeData;
    [SerializeField] List<Attribute> defaultAttributes = new List<Attribute>();

    private void Awake()
    {
        attributesContainer = GetComponent<AttributesContainerComponent>();
        attributesContainer.AttributeAboutToChange += AttributeAboutToChange;

        if (defaultAttributeData)
        {
            defaultAttributes = defaultAttributeData.attributes;
        }

        Dictionary<string, float> localAttributes = new Dictionary<string, float>();
        foreach (Attribute attribute in defaultAttributes)
        {
            localAttributes[attribute.name] = attribute.value;
        }

        attributesContainer.AddAttributeField("Health", localAttributes);
    }

    private void AttributeAboutToChange(object sender, AttributesContainerComponent.AttributeAboutToChangeArgs e)
    {
        if(e.name == "Health")
        {
            attributesContainer.Attributes["Health"] = Mathf.Clamp(attributesContainer.GetAttribute("Health"), 0f, attributesContainer.GetAttribute("MaxHealth"));
        }
    }
}
