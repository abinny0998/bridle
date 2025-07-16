using _GAME.Scripts.Popup;
using _GAME.Scripts.Views;
using DenkKits.GameServices.Audio.Scripts;
using DenkKits.GameServices.SaveData;
using DenkKits.UIManager.Scripts.Base;
using DenkKits.UIManager.Scripts.UIPopup;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public int QuantityPallet { get => _quantityPallet; set => _quantityPallet = value; }
    public int TargetPallet { get => _targetPallet; set => _targetPallet = value; }
    public int CountShowPallet { get => _countShowPallet; set => _countShowPallet = value; }

    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private GenerateMapInLevel generateMapInLevel;
    [SerializeField] private Player player;
    [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;
    [SerializeField] private MeshRenderer _meshRenderer;

    private int _quantityPallet = 0;
    private int _targetPallet = 0;
    private int _countShowPallet = 0;

    private void Start()
    {
        _quantityPallet = 0;
        _currentLevel = SaveDataHandler.Instance.CurrentLevel;
        UIManager.Instance.ViewManager.GetViewByName<GameView>(DenkKits.UIManager.Scripts.UIView.UIViewName.GameView).ShowQuantityPallet(_quantityPallet);
        Instance = this;
        OnReadyGame();
        ChangeTheme();
        LoadCurrentSkin();
    }

    private void LoadCurrentSkin()
    {
        int indexEquip = SaveDataHandler.Instance.GetEquipShop(TypeShop.Skin);
        playerMeshRenderer.material = ShopModel.Instance.SkinItems[indexEquip].Material;
    }

    private void ChangeTheme()
    {
        int indexEquip = SaveDataHandler.Instance.GetEquipShop(TypeShop.Theme);
        _meshRenderer.material = ShopModel.Instance.ThemeItems[indexEquip].Material;
    }

    private void OnReadyGame()
    {
        player.SetActive(false);
        generateMapInLevel.CreateMap(_currentLevel);
    }

    public void SetPositionPlayer(Vector3 position)
    {
        player.transform.localPosition = position;
        player.SetActive(true);
    }

    public void MovePlayer(Vector2 swipeVector)
    {
        if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
        {
            if (swipeVector.x > 0)
            {
                player.ChangeStatePlayer(StatePlayer.MoveRight);
            }
            else
            {
                player.ChangeStatePlayer(StatePlayer.MoveLeft);
            }
        }
        else
        {
            if (swipeVector.y > 0)
            {
                player.ChangeStatePlayer(StatePlayer.MoveForward);
            }
            else
            {
                player.ChangeStatePlayer(StatePlayer.MoveBackward);
            }
        }
    }

    public async void GameEnd(bool isWin)
    {
        if (isWin)
        {
            AudioManager.Instance.PlaySfx(AudioName.Gameplay_WinGame);
            player.GameEnd(isWin);
        }
        else
        {
            AudioManager.Instance.PlaySfx(AudioName.Gameplay_LoseGame);
            player.GameEnd(isWin);
        }

        await Task.Delay(1000);

        var param = new CompletedPopupParam
        {
            CompletedLevel = _currentLevel,
            IsWin = isWin,
        };
        UIManager.Instance.PopupManager.ShowPopup(UIPopupName.CompletedPopup, param);
    }

    public void CollectPallet()
    {
        _quantityPallet++;
        AudioManager.Instance.PlaySfx(AudioName.Gameplay_CollectPallet);
        UIManager.Instance.ViewManager.GetViewByName<GameView>(DenkKits.UIManager.Scripts.UIView.UIViewName.GameView).ShowQuantityPallet(_quantityPallet);
    }
}
