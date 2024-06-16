using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Managing;
using FishNet.Object;

public class server_obsverver_rpc_script : NetworkBehaviour
{
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.A) && base.IsOwner)
            server_color_all_pls(Random.ColorHSV());
        if(Input.GetKeyDown(KeyCode.S) && base.IsOwner)
            server_color_all_but_me(Random.ColorHSV());
    }

    [ServerRpc]
    public void server_color_all_pls(Color col)
    {
        color_all(col);
    }

    [ServerRpc]
    public void server_color_all_but_me(Color col)
    {
        color_all_but_me(col);
    }

    [ObserversRpc(ExcludeOwner = true)]
    public void color_all_but_me(Color col)
    {
        color_me_this(col); 
    }

    [ObserversRpc]
    public void color_all(Color col)
    {
        color_me_this(col);
    }

    public void color_me_this(Color col)
    {
        this.GetComponent<Renderer>().material.color = col;
    }
}
