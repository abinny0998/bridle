using _GAME.Scripts.Views;
using DenkKits.GameServices.SaveData;
using DenkKits.UIManager.Scripts.Base;
using DenkKits.UIManager.Scripts.UIView;
using UnityEngine;

namespace _GAME.Scripts.Controllers
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private SkinnedMeshRenderer _player;

        private void OnEnable()
        {
            LoadCurrentSkin();
            ChangeTheme();
            SaveDataHandler.Instance.OnChangeThemeEvent = ChangeTheme;
            UIManager.Instance.ViewManager.GetViewByName<ShopView>(UIViewName.ShopCharacterView).OnChangeSkinCharacter = ChangeCharacter;
        }

        private void Awake()
        {
            UIManager.Instance.ViewManager.ShowView(UIViewName.MainView);
            UIManager.Instance.HideTransition(() => {  });
        }

        private void ChangeTheme()
        {
            int indexEquip = SaveDataHandler.Instance.GetEquipShop(TypeShop.Theme);
            _meshRenderer.material = ShopModel.Instance.ThemeItems[indexEquip].Material;
        }    

        private void ChangeCharacter(Material material)
        {
            _player.material = material;
        }

        private void LoadCurrentSkin()
        {
            int indexEquip = SaveDataHandler.Instance.GetEquipShop(TypeShop.Skin);
            ChangeCharacter(ShopModel.Instance.SkinItems[indexEquip].Material);
        }
    }
}