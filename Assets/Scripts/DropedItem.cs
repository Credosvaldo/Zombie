using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace MyGame
{
    public class DropedItem : MonoBehaviourPunCallbacks
    {
        public int indice;
        public int quantidade;
        
        [SerializeField] private SpriteRenderer sprite;

        private int playerID;
        private bool podePegar;

        [PunRPC]
        public void SetUp(int indice, int quantidade, int playerID)
        {
            this.indice = indice;
            this.quantidade = quantidade;
            this.sprite.sprite = GameManager.instancia.inventario.slots[indice].image;
            this.playerID = playerID;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (!photonView.IsMine)
                return;

            if(collision.CompareTag("Player"))
            {
                if(collision.GetComponent<Player>().GetInstanceID() != playerID || podePegar)
                {
                    collision.GetComponent<Player>().photonView.RPC("GetDropedItem", RpcTarget.All, indice, quantidade);
                    PhotonNetwork.Destroy(gameObject);
                }
            }

        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (!photonView.IsMine)
                return;

            if(collision.CompareTag("Player"))
            {
                if (collision.GetComponent<Player>().GetInstanceID() == playerID)
                    podePegar = true;
            }

        }

    }


}

