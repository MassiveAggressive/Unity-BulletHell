using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIInventoryEquipmentPanel : MonoBehaviour
{
    [SerializeField] protected GameObject UIItemSlotPrefab;
    [SerializeField] protected Transform parentGridLayoutGroup;
    protected List<UIItemSlot> slots = new List<UIItemSlot>();

    public abstract void SetPanelType(byte type);
    public abstract List<UIItemSlot> CreateSlots(int slotAmount);
}
