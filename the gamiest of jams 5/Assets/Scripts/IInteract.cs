using UnityEngine;
using System.Collections;

public interface IInteract
{
    SpriteRenderer InteractSprite { get; }
    bool CanInteract { get; }

    void Interact();
}
