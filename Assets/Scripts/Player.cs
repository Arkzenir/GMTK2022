using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] InventoryController inventoryController;

    private Vector2 pointerInput, movementInput;

    public Vector2 PointerInput => pointerInput;

    [SerializeField] 
    private InputActionReference movement, attack, pointerPosition, inventory;

    private WeaponRotation weaponRotation;


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        weaponRotation = GetComponentInChildren<WeaponRotation>();
        inventoryController = GetComponent<InventoryController>();
    }


    private void Update()
    {
        pointerInput = GetPointerInput();
        weaponRotation.mousePosition = pointerInput;
        movementInput = movement.action.ReadValue<Vector2>();
        playerMovement.MovementInput = movementInput;
        if (inventory.action.WasReleasedThisFrame())
        {
            inventoryController.UseInventory();
        }

    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
