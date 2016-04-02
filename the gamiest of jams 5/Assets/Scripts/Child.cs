﻿using System.Linq;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Child : MonoBehaviour, IInteract
{
    private const float ScreamRadius = 3;
    private const float ScreamIncrease = 0.2F;
    private const float CollisionIncrease = 0.1F;
    private const float PanicReduct = 0.1F;

    [Range(0, 1)]
    public float panic;
    [Range(0, 3)]
    public int panicState;
    private bool modified;
    public SpriteRenderer interactSprite;
    public ChildTask task = new ChildTask();
    private NavMeshAgent navAgent;
    private Animator animator;
    private Transform graphics;
    private bool showInteract;
    private Teacher interacting;

    public bool CanInteract { get { return task.actionID == -1 && !interacting; } }

    public float Panic
    {
        get { return panic; }
        set
        {
            panic = Mathf.Clamp(value * Mathf.Pow(0.5F, PanicState), 0, 1);

            if (panic == 1 && PanicState < 3)
            {
                PanicState++;
                panic = 0;
            }

            modified = true;
        }
    }

    public int PanicState
    {
        get { return panicState; }
        set
        {
            panicState = Mathf.Clamp(value, 0, 3);
            task.Task = null;
            modified = true;
        }
    }

    public struct ChildTask
    {
        private Task task;
        public int actionID;
        public float timer;

        public Task Task
        {
            get { return task; }
            set
            {
                if (task != null && actionID != -1)
                    task.childen[actionID] = null;
                task = value;
                actionID = -1;
            }
        }

        public Transform Action { get { return Task.actions[actionID]; } }
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

        if (PanicState > 0)
        {
            foreach (var child in Physics.OverlapSphere(transform.position, ScreamRadius * Mathf.Pow(2, PanicState - 1)).Select(col => col.GetComponent<Child>()).Where(chi => chi != null))
            {
                var dist = child.transform.position - transform.position;

                if (Physics.RaycastAll(transform.position, dist, dist.magnitude).Count(hit => !hit.collider.GetComponent<Child>() && !hit.collider.GetComponent<Teacher>()) == 0)
                    child.Panic += Time.deltaTime * ScreamIncrease * Mathf.Pow(2, PanicState - 1);
            }
        }
        
        if (task.Task == null)
        {
            task.Task = Task.GetTask();
            navAgent.SetDestination(task.Task.transform.position);
        }
        else if (task.actionID == -1)
        {
            if ((task.Task.transform.position - transform.position).magnitude < 1)
            {
                if (PanicState == 1)
                {
                    task.Task = null;
                    return;
                }

                var action = -1;

                for (var i = 0; i < task.Task.actions.Length; i++)
                {
                    if (!task.Task.childen[i])
                    {
                        action = i;
                        break;
                    }
                }

                if (action == -1)
                {
                    task.Task = null;
                }
                else
                {
                    task.actionID = action;
                    task.timer = task.Task.time;
                    task.Task.childen[task.actionID] = this;
                    navAgent.SetDestination(task.Action.position);
                }
            }
            else if (navAgent.velocity.magnitude < 1)
            {
                Panic += Time.deltaTime * ScreamIncrease;
            }
        }
        else if ((task.Action.position - transform.position).sqrMagnitude < 1)
        {
            task.timer -= Time.deltaTime;

            if (task.timer <= 0)
            {
                task.Task = null;
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

        if (task.Task)
        {
            navAgent.SetDestination(task.Task.transform.position);
        }
    }
}
