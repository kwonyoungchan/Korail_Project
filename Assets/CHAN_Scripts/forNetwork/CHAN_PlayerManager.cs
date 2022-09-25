using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CHAN_PlayerManager : MonoBehaviourPun
{
    public static GameObject LocalPlayerInstance;
    // Start is called before the first frame update
    void Awake()
    {
        if (photonView.IsMine)
        {
            CHAN_PlayerManager.LocalPlayerInstance = this.gameObject;
        }
        DontDestroyOnLoad(this.gameObject);
        var obj = FindObjectsOfType<CHAN_PlayerManager>();
        if (obj.Length == 2)
        {
            DontDestroyOnLoad(gameObject);
        }
        else if(obj.Length > 2)
        {
            Destroy(gameObject);
        }

    }
    private void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name=="GameScene")
        {
            gameObject.GetComponent<PlayerMaterial>().enabled = true;
            gameObject.GetComponent<PlayerItemDown>().enabled = true;
            gameObject.GetComponent<PlayerForwardRay>().enabled = true;
        }

    }

}
