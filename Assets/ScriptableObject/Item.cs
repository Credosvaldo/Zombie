using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Item", menuName = "Create Item")]
public class Item : ScriptableObject
{
    public Sprite icon;
    public string nome;
    public string descreption;
    public int ID { get; private set; }

    private void OnEnable() => ID = GetInstanceID();

}
