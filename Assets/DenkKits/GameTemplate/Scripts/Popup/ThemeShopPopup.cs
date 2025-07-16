using DenkKits.GameServices.SaveData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DenkKits.UIManager.Scripts.UIPopup
{
    public class ThemeShopPopup : UIPopup
    {
        [SerializeField] private GameObject _prefItems;
        [SerializeField] private RectTransform _parent;
        private List<ThemeItem> _items = new();
        protected override void OnShowing()
        {
            base.OnShowing();

            if(_items.Count > 0)
            {
                RefreshShop();
                return;
            }

            foreach (var model in ShopModel.Instance.ThemeItems)
            {
                var go = Instantiate(_prefItems, _parent);
                go.transform.localScale = Vector3.one;
                var item = go.GetComponent<ThemeItem>();
                item.Set(model, ChangeList);
                _items.Add(item);
            }    
        }

        private void ChangeList(int index)
        {
            foreach (var it in ShopModel.Instance.ThemeItems)
                it.IsEquip = false;
            ShopModel.Instance.ThemeItems[SaveDataHandler.Instance.GetEquipShop(TypeShop.Theme)].IsEquip = false;
            ShopModel.Instance.ThemeItems[index].IsEquip = true;
            SaveDataHandler.Instance.SetEquipShop(TypeShop.Theme, index);
            RefreshShop();
        }    

        private void RefreshShop()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].RefreshButton(ShopModel.Instance.ThemeItems[i]);
            }
        }
    }
}