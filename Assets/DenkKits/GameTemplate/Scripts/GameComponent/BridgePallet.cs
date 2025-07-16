using DenkKits.GameServices.Audio.Scripts;
using UnityEngine;

public class BridgePallet : MonoBehaviour
{
    [SerializeField] private ColliderChecking _bridgePalletColliderChecking;
    [SerializeField] private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer.enabled = false;
        _bridgePalletColliderChecking.InitRegisterAction(BuildBrigde);
    }

    private void BuildBrigde()
    {
        AudioManager.Instance.PlaySfx(AudioName.Gameplay_BuildBridge);
        if (GameController.Instance.CountShowPallet < GameController.Instance.QuantityPallet)
        {
            GameController.Instance.CountShowPallet++;
            _meshRenderer.enabled = true;
        } else
        {
            GameController.Instance.GameEnd(false);
        }
    }
}
