using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int objectiveCount = 4;
    public bool timeUp = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        CheckObjectives();
        CheckTimeUp();
    }
    void CheckObjectives()
    {
       if(objectiveCount <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
    void CheckTimeUp()
    {
        if (timeUp)
        {
            SceneManager.LoadScene(0);
        }
    }
}
