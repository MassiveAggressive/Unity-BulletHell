using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] float rotationSpeed = 360f;

    PlayerInputActions playerInputActions;

    public GameObject weaponObject;
    public WeaponBase weapon;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Movement.Enable();
        playerInputActions.Action.Enable();

        playerInputActions.Action.LeftClick.started += LeftClick_started;
        playerInputActions.Action.LeftClick.canceled += LeftClick_canceled;

        weapon = weaponObject.GetComponent<WeaponBase>();
    }

    private void LeftClick_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        weapon.StopShooting();
    }

    private void LeftClick_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        weapon.StartShooting();
    }

    public S_Item item;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) 
        {
            GetComponent<InventoryEquipmentComponent>().AddItemToInventory(item);
        }

        Vector2 inputVector = playerInputActions.Movement.InputVector.ReadValue<Vector2>();
        Vector2 moveDelta = inputVector.normalized * walkSpeed * Time.deltaTime;

        transform.position += new Vector3(moveDelta.x, moveDelta.y);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;

        Vector3 direction = mousePosition - transform.position;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float step = rotationSpeed * Time.deltaTime;
        transform.up = direction.normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetAngle - 90), step);
    }
}
