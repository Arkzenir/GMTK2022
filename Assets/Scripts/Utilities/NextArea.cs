using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextArea : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> rootObjects;
    private List<Enemy> enemies;
    private GameObject prompt;
    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.K))
        {
            prompt.SetActive(AllDead());
            Debug.Log("All dead: " + AllDead());
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
