using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame
{
    public class Bullet : MonoBehaviour
    {
        Rigidbody2D rig;
        [SerializeField] float velocidade;
        [System.NonSerialized] public float dano;

        void Start()
        {
            rig = GetComponent<Rigidbody2D>();
            rig.velocity = transform.up * velocidade;
            Destroy(gameObject, 2f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Destroy(gameObject);
        }

    }

}

