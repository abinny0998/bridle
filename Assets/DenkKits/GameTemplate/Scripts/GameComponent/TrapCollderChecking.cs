using System;
using UnityEngine;

public class TrapCollderChecking : MonoBehaviour
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

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other == null) return;
    //    boxCollider.enabled = false;
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        OnCallBack?.Invoke();
    //    }
    //}
}
