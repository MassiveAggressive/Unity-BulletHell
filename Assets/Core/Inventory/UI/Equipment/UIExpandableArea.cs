using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIExpandableArea : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] GameObject UIContent;

    public void SetTitleText(string newTitleText)
    {
        titleText.text = newTitleText;
    }

    public void OnTitleButtonPressed()
    {
        UIContent.SetActive(!UIContent.activeSelf);
    }
}
