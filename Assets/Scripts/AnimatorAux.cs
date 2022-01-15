using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class AnimatorAux : MonoBehaviour
    {
        Player player;
        void Start()
        {
            player = transform.parent.GetComponent<Player>();
        }

        void AuxAnimAct(string function)
        {
            player.AnimAct(function);
        }

    }
}

