using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class Player : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] InventoryController inventoryController;

    private Vector2 pointerInput, movementInput;

    public Vector2 PointerInput => pointerInput;

    [SerializeField] 
    private InputActionReference movement, attack, pointerPosition, inventory, roll;

    private WeaponRotation weaponRotation;


    private List<GameObject> bodyStationarySprites;
    private List<GameObject> legSprites;
    private GameObject PlayerBodyParent;
    private GameObject PlayerSpriteRoll;
    private GameObject PlayerSpriteDead;
    private PlayerMovement moveScript;
    
    private Random rand;
    
    private int dieFace;

    public float invulDuration = 0.5f;
    public float rollDuration = 0.15f;
    public float rollCooldown = 0.6f;
    private float invulCounter;
    private float rollCounter;
    private float rollCooldownCounter;
    
    private bool invul;
    public bool rolling;
    private bool dead;

    public int maxHealth = 100;
    public int currHealth;
    
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
        
        PlayerBodyParent = transform.Find("PlayerSpriteStationary").gameObject;
        PlayerSpriteRoll = transform.Find("PlayerSpriteRoll").gameObject;
        PlayerSpriteDead = transform.Find("PlayerSpriteDead").gameObject;
        moveScript = GetComponent<PlayerMovement>();

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

        rand = new Random((int)DateTime.Now.Ticks); //Randomise seed
        dieFace = 0;
        bodyStationarySprites[dieFace].SetActive(true);
        
        invulCounter = invulDuration;
        rollCounter = rollDuration;
        rollCooldownCounter = rollCooldown;
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
        if (dead)
        {
            return;
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
#endif

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        transform.rotation = Quaternion.identity;
        pointerInput = GetPointerInput();
        weaponRotation.mousePosition = pointerInput;
        movementInput = movement.action.ReadValue<Vector2>();
        playerMovement.MovementInput = movementInput;
        if (inventory.action.WasReleasedThisFrame())
        {
            inventoryController.UseInventory();
        }
		
		if (attack.action.WasPressedThisFrame())
        {
            weaponRotation.Attack();
        }

        if (moveScript.MovementInput.x != 0)
        {
            legSprites[1].transform.localScale = new Vector3(moveScript.MovementInput.x,legSprites[1].transform.localScale.y, legSprites[1].transform.localScale.z);
        }

        if (moveScript.MovementInput.x != 0)
        {
            legSprites[0].SetActive(false);
            legSprites[1].SetActive(true);
            legSprites[2].SetActive(false);
        }
        else if (moveScript.MovementInput.y != 0)
        {
            legSprites[0].SetActive(false);
            legSprites[1].SetActive(false);
            legSprites[2].SetActive(true);
        }
        else if(!dead)
        {
            legSprites[0].SetActive(true);
            legSprites[1].SetActive(false);
            legSprites[2].SetActive(false);
        }
        
        
        PlayerSpriteRoll.SetActive(rolling);
        PlayerBodyParent.SetActive(!rolling);
        if (roll.action.WasReleasedThisFrame())
        {
            playerMovement.direction = playerMovement.Roll(pointerInput);
            rolling = true;
            playerMovement.roll = true;
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

        if (rolling && rollCooldownCounter <= 0)
        {
            foreach (var l in legSprites)
            {
                l.SetActive(false);
            }

            if (rollCounter >= 0)
            {
                rollCounter -= Time.deltaTime;
            }
            else
            {
                rolling = false;
                rollCounter = rollDuration;
                rollCooldownCounter = rollCooldown;
                dieFace = rand.Next(0, 6);
                foreach (var face in bodyStationarySprites)
                {
                    face.SetActive(false);
                }
                bodyStationarySprites[dieFace].SetActive(true);
            }
        }
        else if(rollCooldownCounter > 0)
            rollCooldownCounter -= Time.deltaTime;

    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void Die()
    {
        currHealth = 0;
        dead = true;
            
        foreach (var body in bodyStationarySprites)
        {
            body.SetActive(false);
        }

        foreach (var legs in legSprites)
        {
            legs.SetActive(false);
        }
        PlayerSpriteRoll.SetActive(false);
        PlayerSpriteDead.SetActive(true);
        weaponRotation.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void TakeDamage(int val)
    {
        if (rolling || invul)
            return;

        if (currHealth > 0 && !dead && currHealth - val > 0) {
            currHealth = currHealth - val;
            invul = true;
        }
        else {
            Die();
        }
    }
}
