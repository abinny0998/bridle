using System;
using UnityEngine;

public class GroundColliderChecking : MonoBehaviour
{
    private BoxCollider boxCollider;
    private Action OnSinkGround = null;

    public void InitRegisterAction(Action OnSinkGround)
    {
        boxCollider ??= GetComponent<BoxCollider>();
        boxCollider.enabled = true;
        this.OnSinkGround = OnSinkGround;
    }    

    private void OnTriggerExit(Collider other)
    {
        if (other == null) return;
        boxCollider.enabled = false;
        if (other.gameObject.CompareTag("Player"))
        {
            OnSinkGround?.Invoke();
        }
    }
}
