using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class Teacher : MonoBehaviour
{
    public string axisHorizontal = "Horizontal";
    public string axisVertical = "Vertical";
    public string buttonInteract = "Interact";
    public float moveSpeed = 5;

    public Child child;

    private new Rigidbody rigidbody;
    private Animator animator;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void OnMouseUpAsButton()
    {
        if (CamControl.Instance.teacher == null)
            CamControl.Instance.teacher = this;
    }

    void Update()
    {
        if (CamControl.Instance.teacher != this) return;

        if (child)
        {
            if (Input.GetButtonDown(buttonInteract))
                child.Release();

            return;
        }

        var dir = animator.GetInteger("Direction") * 0.5F * Mathf.PI;
        var forward = new Vector3(Mathf.Sin(dir), 0, Mathf.Cos(dir));
        float sqrDist = 0;
        IInteract interact = null;

        foreach (var col in Physics.OverlapSphere(transform.position - forward * 0.8F, 0.4F, -1, QueryTriggerInteraction.Collide))
        {
            var i = col.GetComponent<IInteract>();

            if (i == null || !i.CanInteract) continue;

            var colSqrtDist = (col.transform.InverseTransformPoint(col.ClosestPointOnBounds(transform.position)) - transform.position).sqrMagnitude;

            if (interact == null || sqrDist > colSqrtDist)
            {
                interact = i;
                sqrDist = colSqrtDist;
            }
        }

        if (interact != null)
        {
            interact.ShowInteract();

            if (Input.GetButtonDown(buttonInteract))
                interact.Interact(this);
        }
    }

    void FixedUpdate()
    {
        var moveVector = CamControl.Instance.teacher == this ? new Vector2(Input.GetAxisRaw(axisHorizontal), Input.GetAxisRaw(axisVertical)) : Vector2.zero;
        animator.SetBool("Walking", moveVector != Vector2.zero);

        if (moveVector == Vector2.zero)
        {
            rigidbody.velocity = Vector3.zero;
            return;
        }

        moveVector.Normalize();

        animator.SetInteger("Direction", Mathf.FloorToInt(4.5F + Mathf.Atan2(-moveVector.x, -moveVector.y) * 2 / Mathf.PI) % 4);
        rigidbody.velocity = new Vector3(moveVector.x, 0, moveVector.y) * moveSpeed;
    }
}
