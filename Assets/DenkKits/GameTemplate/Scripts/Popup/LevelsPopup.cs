using _GAME.Scripts.Popup;
using DenkKits.GameServices.Audio.Scripts;
using DenkKits.UIManager.Scripts.Base;
using DenkKits.UIManager.Scripts.UIButton;
using DenkKits.UIManager.Scripts.UIPopup;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME.Scripts.Popup
{
    public class LevelsPopup : UIPopup
    {
        [SerializeField] private DetailLevelItem _prefItemLevel;
        [SerializeField] private ScrollRect _scrollRect;
        private List<DetailLevelItem> _lstLevelsButton = new();
        private int numberLevel = 0;

        protected override void OnShowing()
        {
            base.OnShowing();

            if (numberLevel > 0)
            {
                _lstLevelsButton.Reverse();
                _lstLevelsButton.ForEach(x => SimplePool.Despawn(x.gameObject));
                CreateLevelList();
                return;
            }

            numberLevel = ShopModel.Instance.MapDataList.Levels.Count;
            SimplePool.Preload(_prefItemLevel.gameObject, ShopModel.Instance.MapDataList.Levels.Count);
            CreateLevelList(true);
        }

        private void CreateLevelList(bool isFirst = true)
        {
            _lstLevelsButton?.Clear();
            for (int i = 0; i < numberLevel; i++)
            {
                DetailLevelItem item = SimplePool.Spawn(_prefItemLevel.gameObject, Vector3.zero, Quaternion.identity).GetComponent<DetailLevelItem>();
                item.transform.SetParent(_scrollRect.content);
                item.transform.localScale = Vector3.one;
                item.Set(i + 1, BTN_OnClickedLevelPlay);
                _lstLevelsButton.Add(item);
            }
        }

        private void BTN_OnClickedLevelPlay(int level)
        {
            Debug.Log("LEVEL: " + level);
            this.Hide();
            var param = new PlayPopupParam
            {
                SelectedLevel = level
            };
            AudioManager.Instance.PlaySfx(AudioName.UI_Click);
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.PlayPopup, param);
        }
    }
}