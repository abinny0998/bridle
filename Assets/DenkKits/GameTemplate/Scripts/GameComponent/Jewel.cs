using UnityEngine;

public class Jewel : MonoBehaviour
{
    [SerializeField] private ColliderChecking collderChecking;
    [SerializeField] private ParticleSystem _vfx;

    private void Start()
    {
        collderChecking.InitRegisterAction(OnHide);
    }

    private void OnHide()
    {
        GameController.Instance.GameEnd(true);
        if (_vfx != null)
            _vfx.SetActive(true);
    }
}
