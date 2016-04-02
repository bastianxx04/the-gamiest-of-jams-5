using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshObstacle))]
public class Door : MonoBehaviour
{
    private NavMeshObstacle obstacle;
    private new Collider collider;

    void Awake()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        collider = GetComponent<Collider>();
    }

    public void OpenCloseDoor()
    {
        collider.enabled = obstacle.enabled = !obstacle.enabled;
    }
}
