using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 10f;
    Vector2 inputVector = Vector2.zero;

    [SerializeField] float rotationSpeed = 360f;
    Quaternion targetRotation = Quaternion.identity;

    PlayerInputActions playerInputActions;

    [SerializeField] WeaponBase weapon;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Movement.Enable();
        playerInputActions.Action.Enable();

        playerInputActions.Action.LeftClick.started += LeftClick_started;
        playerInputActions.Action.LeftClick.canceled += LeftClick_canceled;
    }

    private void LeftClick_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        weapon.StopShooting();
    }

    private void LeftClick_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        weapon.StartShooting();
    }

    public SItem item;
    public SItem item2;
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.T)) 
        {
            GetComponent<AttributesContainerComponent>().SetAttribute("MaxFireRate", GetComponent<AttributesContainerComponent>().GetAttribute("MaxFireRate").value + 1);
            //GetComponent<InventoryEquipmentComponent>().AddItemToInventory(item);
        }
        if(Input.GetKeyDown(KeyCode.Y)) 
        {
            GetComponent<AttributesContainerComponent>().SetAttribute("BarrelCount", GetComponent<AttributesContainerComponent>().GetAttribute("BarrelCount").value + 1);
            //GetComponent<InventoryEquipmentComponent>().AddItemToInventory(item2);
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            Time.timeScale = 0.1f;
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            Time.timeScale = 1f;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            targetRotation = Quaternion.LookRotation((hit.point - transform.position).normalized);
        }

        inputVector = playerInputActions.Movement.InputVector.ReadValue<Vector2>();
        Vector2 moveDelta = inputVector.normalized * walkSpeed * Time.deltaTime;

        transform.position += new Vector3(moveDelta.x, 0f, moveDelta.y);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        float Angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;

        float rotationDelta = rotationSpeed * Time.deltaTime;
        //Quaternion.Euler(0, 0, Angle)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation , rotationDelta);
    }
}
