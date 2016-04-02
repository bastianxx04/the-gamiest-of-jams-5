using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Teacher : MonoBehaviour
{
    public string axisHorizontal = "Horizontal";
    public string axisVertical = "Vertical";
    public float moveSpeed = 5;

    private new Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
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
