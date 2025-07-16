using DenkKits.GameServices.SaveData;
using Imba.Utils;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public enum TypeShop
{
    Theme = 0,
    Terrain = 1,
    Skin = 2,
}

[Serializable]
public class ShopThemeModel
{
    public int Id;
    public int LevelReach;
    public Sprite Preview;
    public Material Material;
    public bool IsLock;
    public bool IsEquip;
}

[Serializable]
public class ShopCharacterModel
{
    public int Id;
    public int Price;
    public string Name;
    public Material Material;
    public bool IsLock;
    public bool IsEquip;
}

public class MapData
{
    [JsonProperty("rows")]
    public int Rows { get; set; }

    [JsonProperty("cols")]
    public int Cols { get; set; }

    [JsonProperty("map")]
    public List<int> Map { get; set; }
}

public class MapDataList
{
    [JsonProperty("levels")]
    public List<MapData> Levels { get; set; }
}

public class ShopModel : ManualSingletonMono<ShopModel>
{
    [SerializeField] private List<ShopThemeModel> _themeItems = new List<ShopThemeModel>();
    [SerializeField] private List<ShopCharacterModel> _skinItems = new List<ShopCharacterModel>();

    public List<ShopThemeModel> ThemeItems { get => _themeItems; set => _themeItems = value; }
    public List<ShopCharacterModel> SkinItems { get => _skinItems; set => _skinItems = value; }
    public MapDataList MapDataList { get => mapDataList; set => mapDataList = value; }

    [SerializeField] private TextAsset mapJsonFile;
    [SerializeField] private MapDataList mapDataList;


    void Start()
    {
        LoadThemes();
        LoadSkins();
        LoadLevels();
    }

    private void LoadLevels()
    {
        if (mapJsonFile != null)
        {
            mapDataList = JsonConvert.DeserializeObject<MapDataList>(mapJsonFile.text);
        }
    }    

    private void LoadThemes()
    {
        int indexEquip = SaveDataHandler.Instance.GetEquipShop(TypeShop.Theme);
        for (int i = 0; i < _themeItems.Count; i++)
        {
            _themeItems[i].IsEquip = (i == indexEquip);
            _themeItems[i].IsLock = (_themeItems[i].LevelReach >= SaveDataHandler.Instance.HighestLevel);
        }
    }

    private void LoadSkins()
    {
        int indexEquip = SaveDataHandler.Instance.GetEquipShop(TypeShop.Skin);
        List<int> ownedSkin = SaveDataHandler.Instance.LoadIntList();
        for (int i = 0; i < _skinItems.Count; i++)
        {
            _skinItems[i].IsEquip = (i == indexEquip);
            _skinItems[i].IsLock = ownedSkin.FindIndex(x => x == i) < 0;
        }
    }
}
