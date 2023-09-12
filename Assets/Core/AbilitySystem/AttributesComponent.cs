using AttributeData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AttributesContainerComponent : MonoBehaviour
{
    [SerializeField] S_AttributeData defaultAttributeData;
    [SerializeField] List<Attribute> defaultAttributes = new List<Attribute>();

    protected Dictionary<string, float> attributes = new Dictionary<string, float>();
    public Dictionary<string, float> Attributes { get { return attributes; } set { attributes = value; } }

    protected Dictionary<string, Dictionary<string, float>> allAttributes = new Dictionary<string, Dictionary<string, float>>();

    public event EventHandler<AttributeChangedArgs> AttributeChanged;
    public class AttributeChangedArgs : EventArgs
    {
        public string name;
        public float newValue;
        public float oldValue;
    }
    public AttributeChangedArgs attributeChangedArgs = new AttributeChangedArgs();

    public event EventHandler<AttributeAboutToChangeArgs> AttributeAboutToChange;
    public class AttributeAboutToChangeArgs : EventArgs
    {
        public string name;
    }
    public AttributeAboutToChangeArgs attributeAboutToChangeArgs = new AttributeAboutToChangeArgs();

    public event EventHandler<AttributesChangedArgs> AttributesChanged;
    public class AttributesChangedArgs : EventArgs
    {
        public Dictionary<string, float> attributes;
    }
    public AttributesChangedArgs attributesChangedArgs = new AttributesChangedArgs();

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

    public Dictionary<string, float> GetAttributes()
    {
        return attributes;
    }

    public float GetAttribute(string name)
    {
        return attributes[name];
    }

    public void SetAttribute(string name, float value)
    {
        float oldValue = attributes[name];

        attributes[name] = value;

        attributeAboutToChangeArgs.name = name;
        AttributeAboutToChange?.Invoke(this, attributeAboutToChangeArgs);

        attributeChangedArgs.name = name;
        attributeChangedArgs.oldValue = oldValue;
        attributeChangedArgs.newValue = attributes[name];

        print(attributes[name]);

        AttributeChanged?.Invoke(this, attributeChangedArgs);
    }

    public void AddAttributeField(string fieldName, Dictionary<string, float> newAttributes)
    {
        allAttributes[fieldName] = newAttributes;

        foreach(string name in allAttributes.Keys) 
        {
            print(name);
        }

        CalculateAllAttributes();
    }

    private void CalculateAllAttributes()
    {
        attributes.Clear();

        foreach (string fieldName in allAttributes.Keys) 
        {
            foreach(string attributeName in allAttributes[fieldName].Keys)
            {
                float currentValue = 0f;

                if(attributes.ContainsKey(attributeName))
                {
                    currentValue = attributes[attributeName];
                }

                currentValue += allAttributes[fieldName][attributeName];

                attributes[attributeName] = currentValue;
            }
        }

        foreach(string attributeName in attributes.Keys)
        {
            print(attributeName + ": " + attributes[attributeName]);
        }

        attributesChangedArgs.attributes = attributes;
        AttributesChanged?.Invoke(this, attributesChangedArgs);
    }
}
