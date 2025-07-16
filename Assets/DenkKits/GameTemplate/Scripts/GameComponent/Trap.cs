using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private ColliderChecking trapCollderChecking;
    [SerializeField] private ParticleSystem _vfx;

    private void Start()
    {
        trapCollderChecking.InitRegisterAction(OnHide);
    }

    private void OnHide()
    {
        GameController.Instance.GameEnd(false);
        if (_vfx != null)
            _vfx.SetActive(true);
    }
}
