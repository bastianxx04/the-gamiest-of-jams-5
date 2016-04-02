using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshObstacle))]
public class Door : MonoBehaviour, IInteract
{
    public SpriteRenderer interactSprite;
    private NavMeshObstacle obstacle;
    private new Collider collider;

    public SpriteRenderer InteractSprite { get { return interactSprite; } }
    public bool CanInteract { get { return Physics.CheckBox(transform.position, Vector3.one * 0.5F); } }

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
