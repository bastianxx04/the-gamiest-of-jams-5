using System.Collections.Generic;
using UnityEngine;

public class ChildTask : MonoBehaviour
{
    public static readonly List<ChildTask> Tasks = new List<ChildTask>();

    void OnEnable()
    {
        Tasks.Add(this);
    }

    void OnDisable()
    {
        Tasks.Add(this);
    }

    public static ChildTask GetTask()
    {
        return Tasks[Random.Range(0, Tasks.Count)];
    }
}
