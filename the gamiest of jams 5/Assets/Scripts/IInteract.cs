using UnityEngine;
using System.Collections;

public interface IInteract
{
    bool CanInteract { get; }

    void ShowInteract();
    void Interact();
}
