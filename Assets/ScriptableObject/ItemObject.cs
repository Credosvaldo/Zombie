using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Item", menuName = "Create Item")]
public class ItemObject : ScriptableObject
{
    public string nome;
    public Sprite icon;
    public int ID { get; private set; }

    private void OnEnable() => ID = GetInstanceID();

}
