using DenkKits.GameServices.Audio.Scripts;
using DenkKits.GameServices.SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

//[Serializable]
//public class IntListWrapper
//{
//    public int rows;
//    public int cols;
//    public List<int> map;
//}

public class GenerateMapInLevel : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private MapData _currentMapData;
    [SerializeField] private Transform _parentMap;
    [SerializeField] private Transform _gameSpace;

    [Space(2.0f)]
    [Header("PREFABS")]
    [SerializeField] private GameObject _prefGround;
    [SerializeField] private GameObject _prefPallet;
    [SerializeField] private GameObject _prefBridge;
    [SerializeField] private GameObject _prefBomb;
    [SerializeField] private GameObject _prefSaw;
    [SerializeField] private GameObject _prefSpikes;
    [SerializeField] private GameObject _prefJewel;

    private List<Vector3Int> _lstPositionGrounds = new();

    [SerializeField] private CinemachineCamera camMap;
    [SerializeField] private CinemachineFollow followMap;
    [SerializeField] private CinemachineCamera camPlayer;

    public void CreateMap(int level)
    {
        camMap.Priority = 100;
        camPlayer.Priority = 50;
        LoadJsonMap(level);
        GenerateGroundMap();
    }

    private void LoadJsonMap(int level)
    {

        _currentMapData = ShopModel.Instance.MapDataList.Levels[level - 1];
        followMap.FollowOffset = new Vector3(_currentMapData.Cols, 35, _currentMapData.Rows);

        //string path = $"Assets/Resources/puzzles/custom_map_{level}.json";

        //if (File.Exists(path))
        //{
        //    string json = File.ReadAllText(path);
        //    _currentMapData = JsonUtility.FromJson<IntListWrapper>(json);
        //}
        //else
        //{
        //    Debug.LogError("File not found: " + path);
        //}
    }

    private void GenerateGroundMap()
    {
        MeshRenderer renderer = _prefGround.GetComponent<Ground>().MeshRenderer;
        Bounds bounds = renderer.bounds;
        int index = -1;
        for (int i = 0; i < _currentMapData.Rows; i++)
        {
            for (int j = 0; j < _currentMapData.Cols; j++)
            //for (int j = _currentMapData.cols - 1; j >= 0; j--)
            {
                index++;

                float z = (int)(i * 2);
                float x = (j * 2);
                //float z = (int)(i * bounds.size.z);
                //float x = (j * bounds.size.x);
                Vector3Int cellPos = new Vector3Int((int)x, 0, (int)z);
                MapType type = (MapType)_currentMapData.Map[index];
                if (type == MapType.None) continue;
                if (type != MapType.None && type != MapType.Bridge)
                {
                    //Create ground
                    _lstPositionGrounds.Add(cellPos);
                    //var go = Instantiate(_prefGround, _parentMap);
                    //go.transform.localPosition = cellPos;
                }
            }
        }

        StartCoroutine(CreateGround());
    }

    private void GenerateItemsMap()
    {
        AudioManager.Instance.PlaySfx(AudioName.Gameplay_ShowItemsMap);
        camPlayer.Priority = 200;
        MeshRenderer renderer = _prefGround.GetComponent<Ground>().MeshRenderer;
        Bounds bounds = renderer.bounds;
        int index = -1;
        for (int i = 0; i < _currentMapData.Rows; i++)
        {
            for (int j = 0; j < _currentMapData.Cols; j++)
            {
                index++;
                //float z = (int)(i * bounds.size.z);
                //float x = (j * bounds.size.x);
                float z = (int)(i * 2);
                float x = (j * 2);
                Vector3Int cellPos = new Vector3Int((int)x, 0, (int)z);

                MapType type = (MapType)_currentMapData.Map[index];
                if (type != MapType.None && type != MapType.Ground)
                {
                    GameObject pref = null;
                    switch (type)
                    {
                        case MapType.First:
                            _gameController.SetPositionPlayer(cellPos); 
                            continue;
                        case MapType.Pallet:
                            pref = _prefPallet; break;
                        case MapType.Bridge:
                            GameController.Instance.TargetPallet++;
                            pref = _prefBridge; break;
                        case MapType.Bomb:
                            pref = _prefBomb; break;
                        case MapType.Saw:
                            pref = _prefSaw; break;
                        case MapType.Spikes:
                            pref = _prefSpikes; break;
                        case MapType.Jewel:
                            pref = _prefJewel; break;
                        default: continue;
                    }
                    var go = Instantiate(pref, _parentMap);
                    go.transform.localPosition = cellPos;
                }
            }
        }
    }

    private IEnumerator CreateGround()
    {
        foreach (var cellPos in _lstPositionGrounds)
        {
            var go = Instantiate(_prefGround, _parentMap);
            go.transform.localPosition = cellPos;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1.0f);
        GenerateItemsMap();   
    }
}
