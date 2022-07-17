using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DropedItem : MonoBehaviour
    {
        public int indice;
        public int quantidade;
        
        [SerializeField] private SpriteRenderer sp;

        void Start()
        {
            sp = GetComponent<SpriteRenderer>();
        }

        public void SetUp(int indice, int quantidade)
        {
            this.indice = indice;
            this.quantidade = quantidade;
        }

    }


}

