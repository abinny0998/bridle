using DenkKits.GameServices.Audio.Scripts;
using DenkKits.GameServices.SaveData;
using DenkKits.UIManager.Scripts.Base;
using DenkKits.UIManager.Scripts.UIPopup;
using DenkKits.UIManager.Scripts.UIView;
using Sirenix.Utilities;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _GAME.Scripts.Popup
{
    public class CompletedPopupParam
    {
        public int CompletedLevel;
        public bool IsWin;
    }
    public class CompletedPopup : UIPopup
    {
        [SerializeField] private GameObject _bNext;
        [SerializeField] private GameObject _fireWork;
        [SerializeField] private GameObject[] _rewards;
        [SerializeField] private TMP_Text _tLevelNumber;
        protected override void OnShowing()
        {
            base.OnShowing();

            int numberLevel = ShopModel.Instance.MapDataList.Levels.Count;

            var param = (CompletedPopupParam)Parameter;
            if (param != null)
            {
                _tLevelNumber.text = $"{param.CompletedLevel}";
                _bNext.SetActive(param.IsWin && param.CompletedLevel < numberLevel);

                _rewards[0].SetActive(false);
                _rewards[1].SetActive(SaveDataHandler.Instance.HighestLevel > param.CompletedLevel && param.IsWin);

                if (SaveDataHandler.Instance.HighestLevel == param.CompletedLevel && param.IsWin)
                {
                    _rewards[0].SetActive(true);
                    SaveDataHandler.Instance.HighestLevel = param.CompletedLevel + 1;
                    SaveDataHandler.Instance.CurrentJewel += 100;
                }
                _fireWork.SetActive(param.IsWin);
            }
        }

        public void BTN_Next()
        {
            this.Hide(true);
            SaveDataHandler.Instance.CurrentLevel += 1;
            UIManager.Instance.ViewManager.ShowView(UIViewName.GameView);
            SceneManager.LoadScene("Game");
        }   
        
        public void BTN_Home()
        {
            this.Hide(true);
            SceneManager.LoadScene("Main");
        }   
        
        public void BTN_Replay()
        {
            this.Hide(true);
            UIManager.Instance.ViewManager.ShowView(UIViewName.GameView);
            SceneManager.LoadScene("Game");
        }    
    }
}