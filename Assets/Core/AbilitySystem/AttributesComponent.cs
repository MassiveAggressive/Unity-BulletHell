using AttributeData;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttributesComponent : MonoBehaviour
{
    [SerializeField] S_AttributeData defaultAttributeData;
    [SerializeField] List<Attribute> defaultAttributes = new List<Attribute>();

    protected Dictionary<string, float> attributes = new Dictionary<string, float>();

    protected Dictionary<string, Dictionary<string, float>> allAttributes = new Dictionary<string, Dictionary<string, float>>();

    public event EventHandler<AttributeChangedArgs> AttributeChanged;
    public class AttributeChangedArgs : EventArgs
    {
        public string name;
        public float newValue;
        public float oldValue;
    }
    public AttributeChangedArgs attributeChangedArgs = new AttributeChangedArgs();

    private void Awake()
    {
        if(defaultAttributeData)
        {
            defaultAttributes = defaultAttributeData.attributes;
        }

        Dictionary<string, float> baseAttributes = new Dictionary<string, float>();
        foreach (Attribute attribute in defaultAttributes)
        {
            baseAttributes[attribute.name] = attribute.value;
        }

        AddAttributeField("BaseAttributes", baseAttributes);
    }

    public void AddAttribute(string name, float value)
    {
        allAttributes["BaseAttributes"][name] = value;
    }

    public float GetAttribute(string name)
    {
        return allAttributes["BaseAttributes"][name];
    }

    public void SetAttribute(string name, float value)
    {
        float oldValue = allAttributes["BaseAttributes"][name];

        allAttributes["BaseAttributes"][name] = value;

        PostAttributeChanged(name, oldValue, value);

        attributeChangedArgs.name = name;
        attributeChangedArgs.oldValue = oldValue;
        attributeChangedArgs.newValue = allAttributes["BaseAttributes"][name];

        AttributeChanged?.Invoke(this, attributeChangedArgs);
    }

    public virtual void PostAttributeChanged(string name, float oldValue, float newValue)
    {
        
    }

    public void AddAttributeField(string fieldName, Dictionary<string, float> newAttributes)
    {
        allAttributes[fieldName] = newAttributes;

        CalculateAllAttributes();
    }

    private void CalculateAllAttributes()
    {
        attributes.Clear();

        foreach (string fieldName in allAttributes.Keys) 
        {
            foreach(string attributeName in allAttributes[fieldName].Keys)
            {
                //print(attributeName);
                float currentValue = 0f;

                if(attributes.ContainsKey(attributeName))
                {
                    currentValue = attributes[attributeName];
                }

                currentValue += allAttributes[fieldName][attributeName];

                attributes[attributeName] = currentValue;
            }
        }

        foreach (string attributeName in attributes.Keys)
        {
            print(attributeName + ": " + attributes[attributeName]);
        }
    }
}
