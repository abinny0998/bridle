using System;
using System.Collections.Generic;
using System.Linq;
using Imba.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DenkKits.GameServices.SaveData
{
    public class SaveDataHandler : ManualSingletonMono<SaveDataHandler>
    {
        public SaveData saveData;
        public Action OnChangeThemeEvent;
        public Action OnChangeTerrainEvent;
        public Action OnChangeSkinEvent;

        #region Private Variables

        private const string SaveKey = "UserDataKey";
        private const string CurrentLevelKey = "CurrentLevelKey";
        private const string HighestLevelKey = "HighestLevelKey";
        private const string CurrentJewelKey = "CurrentJewelKey";
        private const string EquipThemeKey   = "EquipThemeKey";
        private const string EquipTerrainKey = "EquipTerrainKey";
        private const string EquipSkinKey    = "EquipSkinKey";
        private const string OwnedSkinKey    = "OwnedSkinKey";
        private bool _requestSave;

        #endregion

        public int UserHighScore
        {
            get => saveData.userHighScore;
            set => saveData.userHighScore = value;
        }
        public int UserHighScoreTime
        {
            get => saveData.userHighScoreTime;
            set => saveData.userHighScoreTime = value;
        }
        #region DAILY REWARD

        [Button]
        public void ResetLastTimeEarnedDailyReward()
        {
            saveData.lastTimeEarnedDailyReward = DateTime.Now.ToString();
        }

        public void AddDayDailyRewardClaimed(int day)
        {
            if (!saveData.dailyRewardClaimedList.Contains(day))
            {
                saveData.dailyRewardClaimedList.Add(day);
            }
        }

        public void ResetDailyRewardList()
        {
            saveData.dailyRewardClaimedList.Clear();
        }

        public bool IsDayDailyRewardClaimed(int day)
        {
            if (saveData.dailyRewardClaimedList.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < saveData.dailyRewardClaimedList.Count; i++)
            {
                if (saveData.dailyRewardClaimedList[i] == day)
                {
                    return true;
                }
            }

            return false;
        }

        public DateTime GetLastTimeEarnedDailyReward()
        {
            return DateTime.Parse(saveData.lastTimeEarnedDailyReward);
        }

        #endregion

        #region Unity Methods

        public override void Awake()
        {
            base.Awake();
            OnLoadData();
        }

        private void OnDisable()
        {
            OnSaveData();
        }

        private void OnApplicationPause(bool pause)
        {
            OnSaveData();
        }

        private void OnApplicationQuit()
        {
            OnSaveData();
        }

        private void Update()
        {
            if (_requestSave)
            {
                _requestSave = false;
                OnSaveData();
            }
        }

        #endregion

        #region Private Methods

        private string RandomName()
        {
            int random = Random.Range(0, 10000);
            return "Player" + random;
        }

        private void OnSaveData()
        {
            // SaveDataByES3();
            SaveDataByPlayerPrefs();
        }

        private void OnLoadData()
        {
            // LoadDataByES3();
            LoadDataByPlayerPrefs();
        }

        private void LoadDataByPlayerPrefs()
        {
            var json = PlayerPrefs.GetString(SaveKey, "");
            if (!PlayerPrefs.HasKey(SaveKey))
            {
                saveData = new SaveData();
            }
            else
            {
                var loadUserData = JsonUtility.FromJson<SaveData>(json);
                saveData = loadUserData;
            }
        }

        private void SaveDataByPlayerPrefs()
        {
            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        #endregion

        #region Public Method

        public void RequestSave()
        {
            _requestSave = true;
            Debug.Log("Requesting save data");
        }

        public int CurrentLevel
        {
            get => PlayerPrefs.GetInt(CurrentLevelKey, 1);
            internal set => PlayerPrefs.SetInt(CurrentLevelKey, value);
        }

        public int HighestLevel
        {
            get => PlayerPrefs.GetInt(HighestLevelKey, 1);
            internal set 
            {
                PlayerPrefs.SetInt(HighestLevelKey, value);
            }
        }

        public int CurrentJewel
        {
            get => PlayerPrefs.GetInt(CurrentJewelKey, 0);
            internal set => PlayerPrefs.SetInt(CurrentJewelKey, value);
        }

        public int GetEquipShop(TypeShop typeShop)
        {
            int indexEquip = 0;
            switch (typeShop)
            {
                case TypeShop.Theme:
                    indexEquip = PlayerPrefs.GetInt(EquipThemeKey, 0);
                    break;
                case TypeShop.Terrain:
                    indexEquip = PlayerPrefs.GetInt(EquipTerrainKey, 0);
                    break;
                case TypeShop.Skin:
                    indexEquip = PlayerPrefs.GetInt(EquipSkinKey, 0);
                    break;
            }
            return indexEquip;
        }

        public void SetEquipShop(TypeShop typeShop, int indexEquip = 0)
        {
            if (GetEquipShop(typeShop) == indexEquip) return;
            switch (typeShop)
            {
                case TypeShop.Theme:
                    PlayerPrefs.SetInt(EquipThemeKey, indexEquip);
                    OnChangeThemeEvent?.Invoke();
                    break;
                case TypeShop.Terrain:
                    PlayerPrefs.SetInt(EquipTerrainKey, indexEquip);
                    OnChangeTerrainEvent?.Invoke();
                    break;
                case TypeShop.Skin:
                    PlayerPrefs.SetInt(EquipSkinKey, indexEquip);
                    OnChangeSkinEvent?.Invoke();
                    break;
            }
        }

        public void AddNewId(int id)
        {
            List<int> list = LoadIntList();
            list.Add(id);
            SaveIntList(list);
        }

        public void SaveIntList(List<int> list)
        {
            string joined = string.Join(',', list);
            PlayerPrefs.SetString(OwnedSkinKey, joined);
            PlayerPrefs.Save();
        }

        public List<int> LoadIntList()
        {
            if (!PlayerPrefs.HasKey(OwnedSkinKey))
                return new List<int> { 0 };

            string data = PlayerPrefs.GetString(OwnedSkinKey);
            if (string.IsNullOrEmpty(data))
                return new List<int> { 0 };

            return data.Split(',').Select(int.Parse).ToList();
        }

        #endregion
    }
}