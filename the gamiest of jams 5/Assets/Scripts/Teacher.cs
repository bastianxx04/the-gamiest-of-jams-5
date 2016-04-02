using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Teacher : MonoBehaviour
{
    public string axisHorizontal = "Horizontal";
    public string axisVertical = "Vertical";
    public string buttonInteract = "Interact";
    public float moveSpeed = 5;

    private new Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float sqrDist = 0;
        IInteract interact = null;

        foreach (var col in Physics.OverlapSphere(transform.position - transform.forward * 0.8F, 0.4F, -1, QueryTriggerInteraction.Collide))
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
                interact.Interact();
        }
    }

    void FixedUpdate()
    {
        var moveVector = new Vector2(Input.GetAxis(axisHorizontal), Input.GetAxis(axisVertical));

        if (moveVector == Vector2.zero)
        {
            rigidbody.velocity = Vector3.zero;
            return;
        }

        moveVector.Normalize();
        var direction = Mathf.Atan2(-moveVector.x, -moveVector.y) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, direction, 0);
        rigidbody.velocity = new Vector3(moveVector.x, 0, moveVector.y) * moveSpeed;
    }
}
