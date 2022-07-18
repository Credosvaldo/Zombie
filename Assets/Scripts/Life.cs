using UnityEngine;
using Photon.Pun;

namespace MyGame
{
    public class Life : MonoBehaviourPunCallbacks
    { 
        [SerializeField] protected float life;
        [SerializeField] float maxLife;
        [SerializeField] string tagDano;
        [SerializeField] float dano;


        protected virtual void Start()
        {
            life = maxLife;
            GameObject player = gameObject;
        }



        
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag(tagDano))
            {
                gameObject.GetPhotonView().RPC("TirarVida", RpcTarget.All);
                //TirarVida();
            }

        }

        [PunRPC]
        protected void TirarVida()
        {
            life -= dano;

            if (life <= 0)
                Death();

        }

        void Death()
        {
            if(GetComponent<Player>() != null)
            {
                GameManager.instancia.photonView.RPC("AtualizarPlayerList", RpcTarget.All);
                gameObject.SetActive(false);
            }
            else
            {
                GameManager.instancia.zombiesInScene--;
                PhotonNetwork.Destroy(gameObject);
            }

        }


    }
}

