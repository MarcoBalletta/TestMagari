using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseState
{
    [SerializeField] private string name;

    public string Name { get => name; set => name = value; }

    public virtual void Enter(GameMode gm) { }
    public virtual void Exit(GameMode gm) { }
}
