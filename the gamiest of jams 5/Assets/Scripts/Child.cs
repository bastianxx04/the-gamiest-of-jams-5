using System.Linq;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Child : MonoBehaviour, IInteract
{
    private const float ScreamRadius = 3;
    private const float ScreamIncrease = 0.2F;
    private const float PanicReduct = 0.1F;

    [Range(0, 1)]
    public float panic;
    private bool modified;
    public SpriteRenderer interactSprite;
    public ChildTask task;
    private NavMeshAgent navAgent;
    private Animator animator;
    private Transform graphics;
    private bool showInteract;
    private Teacher interacting;

    public bool CanInteract { get { return !interacting; } }

    public float Panic
    {
        get { return panic; }
        set
        {
            panic = Mathf.Clamp(value, 0, 1);
            modified = true;
        }
    }

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        graphics = transform.GetChild(0);
    }

    void Update()
    {
        if (interacting)
        {
            navAgent.SetDestination(interacting.transform.position);
        }

        graphics.rotation = Quaternion.Euler(90, 0, 0);
        animator.SetInteger("Direction", Mathf.FloorToInt(2.5F + transform.rotation.eulerAngles.y / 90) % 4);
        animator.SetBool("Walking", navAgent.velocity.sqrMagnitude > 0);

        if (Panic == 1)
        {
            foreach (var child in Physics.OverlapSphere(transform.position, ScreamRadius).Select(col => col.GetComponent<Child>()).Where(chi => chi != null))
            {
                child.Panic += Time.deltaTime * ScreamIncrease;
            }
        }
        else
        {
            if (task == null)
            {
                task = ChildTask.GetTask();
                navAgent.SetDestination(task.transform.position);
            }
        }
    }

    void LateUpdate()
    {
        if (Panic < 1 && !modified)
        {
            Panic -= Time.deltaTime * PanicReduct;
        }

        interactSprite.enabled = showInteract;
        showInteract = false;
        modified = false;
    }

    public void ShowInteract()
    {
        showInteract = true;
    }

    public void Interact(Teacher teacher)
    {
        teacher.child = this;
        interacting = teacher;
        navAgent.stoppingDistance = 0.7F;
    }

    public void Release()
    {
        interacting.child = null;
        interacting = null;
        navAgent.stoppingDistance = 0F;
    }
}
