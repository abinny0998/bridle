using DenkKits.GameServices.Audio.Scripts;
using DenkKits.GameServices.SaveData;
using DenkKits.UIManager.Scripts.Base;
using DenkKits.UIManager.Scripts.UIView;
using Sirenix.Utilities;
using System;
using TMPro;
using UnityEngine;

namespace _GAME.Scripts.Views
{
    public class ShopView : UIView
    {
        [SerializeField] private TMP_Text _tNameSkin;
        [SerializeField] private TMP_Text _tPrice;
        [SerializeField] private TMP_Text _tCoin;
        [SerializeField] private GameObject[] _btns = new GameObject[3];
        private int _indexSkin = 0;
        public Action<Material> OnChangeSkinCharacter = null;
        private ShopCharacterModel _currentModel = null;

        protected override void OnShowing()
        {
            base.OnShowing();
            _indexSkin = SaveDataHandler.Instance.GetEquipShop(TypeShop.Skin);
            _currentModel = ShopModel.Instance.SkinItems[_indexSkin];
            _tCoin.text = SaveDataHandler.Instance.CurrentJewel.ToString();
            ChangeSkin();
        }

        public void BTN_Previous()
        {
            AudioManager.Instance.PlaySfx(AudioName.UI_Click);
            _indexSkin--;
            ChangeSkin();
        }

        public void BTN_Next()
        {
            AudioManager.Instance.PlaySfx(AudioName.UI_Click);
            _indexSkin++;
            ChangeSkin();
        }

        public void BTN_HideView()
        {
            this.Hide();
            int indexEquip = SaveDataHandler.Instance.GetEquipShop(TypeShop.Skin);
            _currentModel = null;
            OnChangeSkinCharacter?.Invoke(ShopModel.Instance.SkinItems[indexEquip].Material);
            UIManager.Instance.ViewManager.ShowView(UIViewName.MainView);
        }    

        public void ChangeSkin()
        {
            _indexSkin = Mathf.Clamp(_indexSkin, 0, ShopModel.Instance.SkinItems.Count - 1);
            _currentModel = ShopModel.Instance.SkinItems[_indexSkin];
            _tNameSkin.text = _currentModel.Name;

            _btns.ForEach(x => x.SetActive(false));
            if (_currentModel.IsEquip)
                _btns[0].SetActive(true);
            else if (_currentModel.IsLock)
            {
                _btns[^1].SetActive(true);
                _tPrice.text = _currentModel.Price.ToString();
            }
            else
                _btns[1].SetActive(true);

            Material material = _currentModel.Material;
            OnChangeSkinCharacter?.Invoke(material);
        }

        public void BTN_Buy()
        {
            AudioManager.Instance.PlaySfx(AudioName.UI_Click);
            if (_currentModel.Price <= SaveDataHandler.Instance.CurrentJewel)
            {
                SaveDataHandler.Instance.CurrentJewel -= _currentModel.Price;
                SaveDataHandler.Instance.AddNewId(_currentModel.Id);
                ShopModel.Instance.SkinItems[SaveDataHandler.Instance.GetEquipShop(TypeShop.Skin)].IsEquip = false;
                SaveDataHandler.Instance.SetEquipShop(TypeShop.Skin, _currentModel.Id);
                ShopModel.Instance.SkinItems[_currentModel.Id].IsEquip = true;
                ShopModel.Instance.SkinItems[_currentModel.Id].IsLock = false;
                _tCoin.text = SaveDataHandler.Instance.CurrentJewel.ToString();
            }
            ChangeSkin();
        }

        public void BTN_Equip()
        {
            AudioManager.Instance.PlaySfx(AudioName.UI_Click);
            ShopModel.Instance.SkinItems[SaveDataHandler.Instance.GetEquipShop(TypeShop.Skin)].IsEquip = false;
            SaveDataHandler.Instance.SetEquipShop(TypeShop.Skin, _currentModel.Id);
            ShopModel.Instance.SkinItems[_currentModel.Id].IsEquip = true;
            ChangeSkin();
        }
    }
}