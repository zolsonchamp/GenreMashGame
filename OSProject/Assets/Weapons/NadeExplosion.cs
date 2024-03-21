using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NadeExplosion : MonoBehaviour
{
    public GameObject projectile;
    public GameObject Explosion;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        if (collision.gameObject.tag != "Target"&&collision.gameObject.tag!="explosion")
        {
           
            GameObject boom = Instantiate(Explosion);
            boom.transform.position = gameObject.transform.position;

        }
    }
}
