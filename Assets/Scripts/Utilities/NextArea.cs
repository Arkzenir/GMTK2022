using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextArea : MonoBehaviour
{
    // Start is called before the first frame update
    public float minInteractDistance;
    
    private List<GameObject> rootObjects;
    private List<Enemy> enemies;
    private GameObject prompt;
    private Transform playerTransform;
    private float distanceToPlayer;
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        prompt = transform.Find("PromptGraphics").gameObject;
        prompt.SetActive(false);
        rootObjects = new List<GameObject>();
        enemies = new List<Enemy>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects( rootObjects );

        foreach (var e in rootObjects)
        {
            if (e.CompareTag("Enemy"))
            {
                enemies.Add(e.GetComponent<Enemy>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= minInteractDistance && AllDead() || enemies.Count == 0)
        {
            prompt.SetActive(AllDead() || enemies.Count == 0);
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (SceneManager.sceneCountInBuildSettings > nextSceneIndex && Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
        }
        
        
        
    }

    private bool AllDead()
    {
        foreach (var e in enemies)
        {
            if (e.state != Enemy.AIState.Dead)
            {
                return false;
            }
        }
        return true;
    }
}
