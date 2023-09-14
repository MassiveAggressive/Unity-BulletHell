using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINPCHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private AttributesContainerComponent attributesComponent;

    private void Awake()
    {
        healthBar = GetComponent<Image>();
    }

    private void Update()
    {
        healthBar.fillAmount = attributesComponent.GetAttribute("Health").value / attributesComponent.GetAttribute("MaxHealth").value;
    }
}
