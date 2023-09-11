using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAttribute : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [HideInInspector] public string attributeName;
    [HideInInspector] public float attributeValue;

    private void Start()
    {
        text.text = attributeName + ": " + attributeValue;
    }
}
