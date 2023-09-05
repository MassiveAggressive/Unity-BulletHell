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

    public S_Item item;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) 
        {
            GetComponent<InventoryEquipmentComponent>().AddItemToInventory(item);
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            Time.timeScale = 0.1f;
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            Time.timeScale = 1f;
        }

        Vector2 inputVector = playerInputActions.Movement.InputVector.ReadValue<Vector2>();
        Vector2 moveDelta = inputVector.normalized * walkSpeed * Time.deltaTime;

        transform.position += new Vector3(moveDelta.x, moveDelta.y);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;

        Vector3 direction = mousePosition - transform.position;

        float step = rotationSpeed * Time.deltaTime;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = transform.rotation.eulerAngles.z;

        float deltaAngle = currentAngle + step;

        transform.rotation = Quaternion.Euler(0f, 0f, deltaAngle);
    }
}
