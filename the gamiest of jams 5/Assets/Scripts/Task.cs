using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public static readonly List<Task> Tasks = new List<Task>();
    
    public float time;
    public Transform[] actions;
    [System.NonSerialized]
    public Child[] childen;

    void OnEnable()
    {
        Tasks.Add(this);
        childen = new Child[actions.Length];
    }

    void OnDisable()
    {
        Tasks.Add(this);
    }

    public static Task GetTask()
    {
        return Tasks[Random.Range(0, Tasks.Count)];
    }
}
