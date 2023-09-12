using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIItemSlot : MonoBehaviour
{
    private SItemSlot itemSlot;
    public SItemSlot ItemSlot { get { return itemSlot; } set { itemSlot = value; } }

    bool isAvailable = true;
    public bool IsAvailable { get { return isAvailable; } private set { isAvailable = value; } }

    [SerializeField] private GameObject visual;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;

    public event EventHandler<EventArgs> UIItemSlotClicked;

    public void SetItemSlot(SItemSlot itemSlot)
    {
        if (itemSlot)
        {
            this.itemSlot = itemSlot;

            nameText.text = itemSlot.item.itemName;

            isAvailable = false;
            visual.SetActive(true);
        }
        else
        {
            this.itemSlot = null;

            isAvailable = true;
            visual.SetActive(false);
        }
    }

    public void OnButtonPressed()
    {
        if(itemSlot)
        {
            UIItemSlotClicked?.Invoke(this, new EventArgs());
        }
    }
}
