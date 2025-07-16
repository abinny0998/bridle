using System.Collections.Generic;
using _GAME.Scripts;
using DenkKits.GameServices.SaveData;
using DenkKits.UIManager.Scripts.UIPopup;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DenkKits.GameTemplate.Scripts.Popup
{
    public class GameModePopup : UIPopup
    {
        [SerializeField] private List<GameObject> modeDetails;
        protected override void OnInit()
        {
            base.OnInit();
        }

        protected override void OnShowing()
        {
            base.OnShowing();
            CloseModeDetail();
        }

        public void ShowModeDetail(int mode)
        {
            modeDetails[mode].SetActive(true);
        }
        public void CloseModeDetail()
        {
            foreach (var xDetail in modeDetails)
            {
                xDetail.SetActive(false);
            }
        }
        public void LoadGame(int mode)
        {
            SaveDataHandler.Instance.saveData.gameModeChoose = mode;
            Hide();
            UIManager.Scripts.Base.UIManager.Instance.ShowTransition(() => { SceneManager.LoadScene(GameConstants.SceneGame); });
        }
    }
}