using System.Linq;
using UnityEngine;

[RequireComponent(typeof(NavMeshObstacle), typeof(Animator))]
public class Door : MonoBehaviour, IInteract
{
    public SpriteRenderer interactSprite;
    private NavMeshObstacle obstacle;
    private new Collider collider;
    private Animator animator;
    private bool showInteract;

    public bool CanInteract { get { return Physics.OverlapBox(transform.position, Vector3.one * 0.4F).Count(col => col.gameObject != gameObject) == 0; } }

    void Awake()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        collider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
    }

    void LateUpdate()
    {
        interactSprite.enabled = showInteract;
        showInteract = false;
    }

    public void ShowInteract()
    {
        showInteract = true;
    }

    public void Interact(Teacher teacher)
    {
        animator.SetBool("Open", collider.enabled);
        collider.enabled = obstacle.enabled = !obstacle.enabled;
    }
}
