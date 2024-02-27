using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MinionController : MonoBehaviour, Damagable
{

    public Transform player;
    public int moveSpeed = 4;
    int maxDist = 10;
    int minDist = 0;

    public void TakeDamage(float damage)
    {
        
    }

    void Start()
    {

    }

    void Update()
    {
        transform.LookAt(player);

        if (Vector3.Distance(transform.position, player.position) >= minDist)
        {

            transform.position += transform.forward * moveSpeed * Time.deltaTime;



            if (Vector3.Distance(transform.position, player.position) <= maxDist)
            {
                
            }

        }
    }
}
