using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPoolController : MonoBehaviour
{
    public float duration;
    public float acidTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        acidTimer += Time.deltaTime;
        if(acidTimer > duration)
        {
            Destroy(gameObject);
        }
    }
}
