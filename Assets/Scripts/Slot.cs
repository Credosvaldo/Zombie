using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] ItemObject item;
    [SerializeField] Image icon;
    [SerializeField] Text textAmont;

    void Start()
    {
        icon.sprite = item.icon;

    }

    void Update()
    {
        
    }
}
