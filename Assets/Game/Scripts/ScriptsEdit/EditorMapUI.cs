using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class EditorMapUI : MonoBehaviour
{
    public int cols = 0;
    public int rows = 0;
    public GridLayoutGroup.Constraint type = GridLayoutGroup.Constraint.FixedColumnCount;
    public EditGroundToggle _prefToggle;
    public List<Toggle> _listToggle;
    public GridLayoutGroup parentGrid;
    public List<EditGroundToggle> listGroundUI = new();
    public int _level = 1;

    private void Start()
    {
        //CreateBlockUI();
    }

    [Button("Create Grid UI")]
    public void CreateBlockUI()
    {
        listGroundUI?.Clear();

        //int childCount = total;
        //int constraintCount = parentGrid.constraintCount;

        //int rows = 0;
        //int cols = 0;

        //if (type == GridLayoutGroup.Constraint.FixedColumnCount)
        //{
        //    cols = constraintCount;
        //    rows = Mathf.CeilToInt((float)childCount / cols);
        //}
        //else if (type == GridLayoutGroup.Constraint.FixedRowCount)
        //{
        //    rows = constraintCount;
        //    cols = Mathf.CeilToInt((float)childCount / rows);
        //}

        //Debug.Log($"Grid: {rows} x {cols}");

        int[,] arr = new int[rows, cols];

        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                var tg = Instantiate(_prefToggle, parentGrid.transform);
                tg.SetPositionInArray(new Vector2(i, j), ChangeType);
                listGroundUI.Add(tg);
            }
        }
    }

    public void ChangeType(EditGroundToggle edit)
    {
        for (int i = 0; i < _listToggle.Count; i++)
        {
            if (_listToggle[i].isOn)
            {
                edit.ChangeMapType((MapType)i, _listToggle[i].GetComponent<Image>().color);
            }    
        }
    }

    [Button("Refresh List")]
    public void RefreshList()
    {
        foreach (var item in listGroundUI)
        {
            item.Refresh();
        }
    }

    [Button("Destroy Item Grid UI")]
    public void DestroyItemGridUI()
    {
        parentGrid.constraint = type;
        if (type == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            parentGrid.constraintCount = cols;
        }
        else if (type == GridLayoutGroup.Constraint.FixedRowCount)
        {
            parentGrid.constraintCount = rows;
        }

        listGroundUI?.Clear();
        foreach (Transform child in parentGrid.transform)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }
    }

    [Button("Save JSON")]
    public void SaveJson()
    {
        List<int> list = new List<int>();

        foreach (var item in listGroundUI)
        {
            list.Add((int)item.MapType);
        }

        //int childCount = parentGrid.transform.childCount;
        //int constraintCount = parentGrid.constraintCount;

        //int rows = 0;
        //int cols = 0;

        //if (parentGrid.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        //{
        //    cols = constraintCount;
        //    rows = Mathf.CeilToInt((float)childCount / cols);
        //}
        //else if (parentGrid.constraint == GridLayoutGroup.Constraint.FixedRowCount)
        //{
        //    rows = constraintCount;
        //    cols = Mathf.CeilToInt((float)childCount / rows);
        //}

        //Debug.Log($"Grid: {rows} x {cols}");

        //string json = JsonUtility.ToJson(list);
        //string path = Path.Combine(Application.persistentDataPath, $"{rows}x{cols}_custom_map_{_level}.json"); //row x column

        string path = $"Assets/Resources/puzzles/custom_map_{_level}.json";
        //string json = JsonConvert.SerializeObject(worldMapModels, Formatting.Indented);

        //MapData wrapper = new MapData
        //{ 
        //    rows = this.rows, 
        //    cols = this.cols,
        //    map = list
        //};
        //string json = JsonUtility.ToJson(wrapper);
        //Debug.Log(json);

        //Debug.Log($"Name file: {rows}x{cols}_custom_map_{_level}.json ___\n {path}");
        //string directory = Path.GetDirectoryName(path);
        //if (!Directory.Exists(directory))
        //{
        //    Directory.CreateDirectory(directory);
        //}

        //File.WriteAllText(path, json);
        //Debug.Log("Save file success.");
    }
}