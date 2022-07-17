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

    private List<GameObject> bodyStationarySprites;
    private List<GameObject> legSprites;
    private GameObject PlayerSpriteRoll;
    private GameObject PlayerSpriteDead;
    

    public float invulDuration = 0.5f;
    public float rollDuration = 0.75f;
    private float invulCounter;
    private float rollCounter;
    
    private bool invul;
    private bool rolling;
    private bool dead;

    public int maxHealth = 100;
    private int currHealth;
    
    private void Start()
    {
        bodyStationarySprites = new List<GameObject>();
        legSprites = new List<GameObject>();
        for (int i = 0; i < transform.Find("PlayerSpriteStationary").childCount; i++)
        {
            bodyStationarySprites.Add(transform.Find("PlayerSpriteStationary").GetChild(i).gameObject);
        }
        // 0 = Idle, 1 = Horizontal, 2 = Vertical
        for (int i = 0; i < transform.Find("PlayerLegs").childCount; i++)
        {
            legSprites.Add(transform.Find("PlayerLegs").GetChild(i).gameObject);
        }

        PlayerSpriteRoll = transform.Find("PlayerSpriteRoll").gameObject;
        PlayerSpriteDead = transform.Find("PlayerSpriteDead").gameObject;

        foreach (var body in bodyStationarySprites)
        {
            body.SetActive(false);
        }

        foreach (var legs in legSprites)
        {
            legs.SetActive(false);
        }
        PlayerSpriteDead.SetActive(false);
        PlayerSpriteRoll.SetActive(false);
        legSprites[0].SetActive(true);
        bodyStationarySprites[0].SetActive(true);
        
        
        invulCounter = invulDuration;
        rollCounter = rollDuration;
        currHealth = maxHealth;
        invul = false;
        rolling = false;
        dead = false;
    }

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

        if (invul)
        {
            if (invulCounter >= 0)
            {
                invulCounter -= Time.deltaTime;
            }
            else
            {
                invul = false;
                invulCounter = invulDuration;
            }
        }
        
        if (rolling)
        {
            if (rollCounter >= 0)
            {
                rollCounter -= Time.deltaTime;
            }
            else
            {
                rolling = false;
                rollCounter = rollDuration;
            }
        }

    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void TakeDamage(int val)
    {
        if (rolling || invul)
            return;

        if (currHealth > 0 || !dead || currHealth - val > 0) {
            currHealth = currHealth - val;
            invul = true;
        }
        else {
            currHealth = 0;
            dead = true;
        }
    }
}
