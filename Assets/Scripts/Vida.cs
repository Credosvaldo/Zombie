using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace MyGame
{
    public class Vida : MonoBehaviourPunCallbacks
    {
        protected virtual void Start()
        {
        }

        protected void Intermediario()
        {
            
        }

        [PunRPC]
        protected void Testoooo()
        {
            Debug.Log("TESTADISSIMOOOOO AEEEEEEEEEEEEEEEEEEEE");
        }

        void Update()
        {

        }
    }
}

