using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshObstacle))]
public class Door : MonoBehaviour
{
    private NavMeshObstacle obstacle;

    void Awake()
    {
        obstacle = GetComponent<NavMeshObstacle>();
    }

    public void OpenCloseDoor()
    {
        obstacle.enabled = !obstacle.enabled;
    }
}
