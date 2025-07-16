using DenkKits.GameServices.Audio.Scripts;
using DG.Tweening;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private GroundColliderChecking groundColliderChecking;
    [SerializeField] private MeshRenderer meshRenderer;

    public MeshRenderer MeshRenderer { get => meshRenderer; set => meshRenderer = value; }

    private void Start()
    {
        FloatOnGround();
        groundColliderChecking.InitRegisterAction(SinkGround);
    }

    private void SinkGround()
    {
        DOTween.Sequence()
            .Insert(0.0f, this.gameObject.transform.DOMoveY(2.0f, 0.2f))
            .Append(this.gameObject.transform.DOMoveY(-1f, 0.8f))
            .OnComplete(() =>
            {
                this.gameObject.SetActive(false);
            });
    }

    public void FloatOnGround()
    {
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, -1.0f, this.gameObject.transform.position.z);
        this.gameObject.SetActive(true);
        DOTween.Sequence()
            .Insert(0.0f, this.gameObject.transform.DOMoveY(2.0f, 0.8f))
            .Append(this.gameObject.transform.DOMoveY(0, 0.2f))
            .OnComplete(() =>
            {
                AudioManager.Instance.PlaySfx(AudioName.Gameplay_Ground);
            });
    }    
}
