using System;
using UnityEngine;

public class PalletColliderChecking : MonoBehaviour
{
    private BoxCollider boxCollider;
    private Action OnCollectPallet = null;

    public void InitRegisterAction(Action OnCollectPallet)
    {
        boxCollider ??= GetComponent<BoxCollider>();
        boxCollider.enabled = true;
        this.OnCollectPallet = OnCollectPallet;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        boxCollider.enabled = false;
        if (other.gameObject.CompareTag("Player"))
        {
            OnCollectPallet?.Invoke();
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other == null) return;
    //    boxCollider.enabled = false;
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        OnCollectPallet?.Invoke();
    //    }
    //}
}
