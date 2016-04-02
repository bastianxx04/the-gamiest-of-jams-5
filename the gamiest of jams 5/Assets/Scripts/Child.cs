using System.Linq;
using UnityEngine;

public class Child : MonoBehaviour
{
    private const float ScreamRadius = 3;
    private const float ScreamIncrease = 0.2F;
    private const float PanicReduct = 0.1F;

    [Range(0, 1)]
    public float panic;
    private bool modified;

    public Vector2 Position { get { return new Vector2(transform.position.x, transform.position.z); } }
    public float Panic
    {
        get { return panic; }
        set
        {
            panic = Mathf.Clamp(value, 0, 1);
            modified = true;
        }
    }

    void Update()
    {
        if (Panic == 1)
        {
            foreach(var child in Physics.OverlapSphere(transform.position, ScreamRadius).Select(col => col.GetComponent<Child>()).Where(chi => chi != null))
            {
                child.Panic += Time.deltaTime * ScreamIncrease;
            }
        }
    }

    void LateUpdate()
    {
        if (Panic < 1 && !modified)
        {
            Panic -= Time.deltaTime * PanicReduct;
        }

        modified = false;
    }
}
