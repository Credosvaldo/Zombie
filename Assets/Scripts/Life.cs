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
            }

        }

        [PunRPC]
        protected void TirarVida()
        {
            Debug.Log(gameObject.name + " perdeu vida");
            life -= dano;

            if (life <= 0)
                Death();

        }

        void Death()
        {
            PhotonNetwork.Destroy(gameObject);
            //POR FAVOR OTIMIZA ISSO LOGOOOOOOOOO
            if(GetComponent<Player>() != null)
            {
                GameManager.instancia.zombiesInScene--;
            }
            else
            {
                GameManager.instancia.photonView.RPC("AtualizarPlayerList", RpcTarget.All);
            }

        }


    }
}

