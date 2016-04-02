using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public static readonly List<Task> Tasks = new List<Task>();

    void OnEnable()
    {
        Tasks.Add(this);
    }

    void OnDisable()
    {
        Tasks.Add(this);
    }
}
