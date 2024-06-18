using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet;

public class target_rpc_test_script : NetworkBehaviour
{

    public GameObject obj_to_intsantiate;

    // Update is called once per frame
    void Update()
    {
        if (base.IsOwner)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                print_clients_and_owned_objects(base.LocalConnection);
            }
            if (Input.GetKeyUp(KeyCode.N))
            {
                spawn_new_owned_obj(obj_to_intsantiate, base.LocalConnection);
            }
        }
    }

    [ServerRpc(RequireOwnership = true)]
    public void spawn_new_owned_obj(GameObject _yourprefab, NetworkConnection conn)
    {
        if(_yourprefab != null)
        {
            GameObject go = Instantiate(_yourprefab);
            this.NetworkObject.ServerManager.Spawn(go, conn);
        }
    }

    [ServerRpc(RequireOwnership = true)]
    public void send_data_to_server()
    {

    }

    [TargetRpc]
    public void send_data_to_goal(NetworkConnection conn)
    {
        
    }

    [TargetRpc]
    public void color_me_this_rpc(NetworkConnection conn, NetworkObject nob)
    {
        nob.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
    }

    [ServerRpc]
    public void print_clients_and_owned_objects(NetworkConnection conn)
    {

        List<NetworkConnection> conn_list = new List<NetworkConnection>();
        Debug.Log("ello");
        Debug.Log(conn.NetworkManager.ServerManager.Clients.Count); //use networkmanager to servermanager to clients to get all connected clients !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        Debug.Log(this.NetworkManager.ServerManager.Clients.Count);
        /*
        foreach(NetworkConnection c in conn.NetworkManager.ServerManager.Clients.Values)
        {
            Debug.Log(c);
            //Debug.Log(c.Objects.Count);
            if (c != conn)
            {
                foreach (NetworkObject owned_obj in conn.Objects)
                {
                    Debug.Log(owned_obj);
                    //Debug.Log(owned_obj.GetComponent<target_rpc_test_script>()); //cannot get component off of network object on other client/connection !!!!!!!!!!!!!!!!!!!!!!!!
                    // USE TARGETRPCS ONCE THE RECEIVING TARGET IS FOUND
                    color_me_this_rpc(c, owned_obj);
                }
            }
        }
        */
    }



    [ServerRpc(RequireOwnership = true)]
    public void set_listening_sending_tags()
    {

    }


    [ObserversRpc(ExcludeOwner = true)]
    public void greet_me_back()
    {
        hello_back(base.LocalConnection);
    }

    [Client]
    public void hello_back(NetworkConnection conn)
    {
        send_hello_back(conn, base.LocalConnection, "hi to you too :)");
    }

    [TargetRpc]
    public void send_hello_back(NetworkConnection conn, NetworkConnection sending_conn, string msg)
    {
        Debug.Log("got message: " + msg + " from: " + sending_conn);
    }
}
