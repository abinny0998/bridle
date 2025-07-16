using UnityEngine;

public class Pallet : MonoBehaviour
{
    [SerializeField] private ColliderChecking _palletColliderChecking;

    private void Start()
    {
        _palletColliderChecking.InitRegisterAction(Collected);
    }

    private void Collected()
    {
        GameController.Instance.CollectPallet();
        this.gameObject.SetActive(false);
    }
}
