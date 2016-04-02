using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshObstacle))]
public class Door : MonoBehaviour, IInteract
{
    public SpriteRenderer interactSprite;
    private NavMeshObstacle obstacle;
    private new Collider collider;

    public SpriteRenderer InteractSprite { get { return interactSprite; } }
    public bool CanInteract { get { Debug.Log("Interact" + Physics.OverlapBox(transform.position, Vector3.one * 0.4F).Length); return Physics.OverlapBox(transform.position, Vector3.one * 0.4F).Length == 1; } }

    void Awake()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        collider = GetComponent<Collider>();
    }

    public void Interact()
    {
        collider.enabled = obstacle.enabled = !obstacle.enabled;
    }
}
