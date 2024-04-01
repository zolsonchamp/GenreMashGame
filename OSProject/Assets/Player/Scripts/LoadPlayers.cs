using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;

public class LoadPlayers : NetworkBehaviour
{

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsServer && base.IsOwner)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.GetComponent<moveCamera>().inUse = true;
        }
        else if (IsServer && !base.IsOwner)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(0).gameObject.GetComponent<PlayerController>().inUse = true;
        }
        else if (!IsServer && base.IsOwner)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(0).gameObject.GetComponent<PlayerController>().inUse = true;
        }
        else if (!IsServer && !base.IsOwner)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.GetComponent<moveCamera>().inUse = true;
        }
    }
}
