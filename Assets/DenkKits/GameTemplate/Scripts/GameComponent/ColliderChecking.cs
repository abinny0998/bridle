using System;
using UnityEngine;

public class ColliderChecking : MonoBehaviour
{
    private BoxCollider boxCollider;
    private Action OnCallBack = null;

    public void InitRegisterAction(Action OnCallBack)
    {
        boxCollider ??= GetComponent<BoxCollider>();
        boxCollider.enabled = true;
        this.OnCallBack = OnCallBack;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        boxCollider.enabled = false;
        if (other.gameObject.CompareTag("Player"))
        {
            OnCallBack?.Invoke();
        }
    }
}
