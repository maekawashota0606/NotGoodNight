using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectFinder : EditorWindow
{
#region 変数領域

    private static Dictionary<string, List<AssetData>> results         = new Dictionary<string, List<AssetData>>();
    private static Dictionary<string, bool>            foldOuts        = new Dictionary<string, bool>();
    private        Vector2                             ScrollPosition  = Vector2.zero;
    //private static string                              _searchFileName = "";
    private const  float                               WINDOW_W        = 500f;
    private const  float                               WINDOW_H        = 500f;

#endregion

    [MenuItem("Tools/Find/Script to Find object(prefab, scene)", false)]
    private static void ObjectFindWindow()
    {
        var window = GetWindow<ObjectFinder>();
        window.titleContent.text = "プレハブ探索";
        window.maxSize           = window.minSize =
                                       new Vector2(WINDOW_W, WINDOW_H);
        results.Clear();
        foldOuts.Clear();
    }

    private bool IsTargetFile(string path, string guid)
    {
        var pathHeader = 
            Application.dataPath.Replace("Assets", "");
        var filePath   = pathHeader + path;
        using (var str = new StreamReader(filePath))
        {
            var fileText = str.ReadToEnd();
            return (0 <= fileText.IndexOf(guid, StringComparison.Ordinal));
        }
    }

    private void SearchObject()
    {
        results.Clear();
        foldOuts.Clear();
        
        // 検索対象のディレクトリ
        var targetsPrefab   = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets", }).Select(AssetData.CreateByGuid).ToList();
        var targetsScene    = AssetDatabase.FindAssets("t:Scene",  new string[] { "Assets", }).Select(AssetData.CreateByGuid).ToList();
        var selectPrefabs   = new List<AssetData>();
        var selecteds       = 
            Selection.objects.Select(AssetData.CreateByObject).ToList();
        var selectAssetData = selecteds[0];

        var ext = Path.GetExtension(selectAssetData.Path);
        if(!ext.Equals(".cs"))
            return;
        // 全てのアセットの中から検索
        foreach (var target in targetsPrefab)
        {
            if (IsTargetFile(target.Path, selectAssetData.Guid))
            {
                results.AddSafety(selectAssetData.Name, new List<AssetData>());
                results[selectAssetData.Name].Add(target);
                selectPrefabs.Add(target);
            }
        }

        foreach (var selectPrefab in selectPrefabs)
        {
            foreach (var target in targetsScene)
            {
                if (IsTargetFile(target.Path, selectPrefab.Guid))
                {
                    if(!results[selectAssetData.Name].Contains(target))
                        results[selectAssetData.Name].Add(target);
                }
            }
        }
        selectPrefabs.Clear();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("対象オブジェクトを探しますか？");
        if (GUILayout.Button("探す"))
        {
            SearchObject();
        }
        GUILayout.EndHorizontal();
        this.ScrollPosition = GUILayout.BeginScrollView(this.ScrollPosition);
        foreach (var referent in results.Keys)
        {
            foldOuts.AddSafety(referent, true);
            if (foldOuts[referent] == EditorGUILayout.Foldout(foldOuts[referent], referent))
            {
                foreach (var target in results[referent])
                {
                    var iconSize = EditorGUIUtility.GetIconSize();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(target.Name);
                    EditorGUIUtility.SetIconSize(Vector2.one * 16);
                    if (GUILayout.Button("開く"))
                    {
                        var obj = target.ToObject();
                        Selection.objects = new[] { obj };
                    }
                    GUILayout.EndHorizontal();

                    EditorGUIUtility.SetIconSize(iconSize);
                }
            }
        }
        GUILayout.EndScrollView();

    }
}

/// <summary>
/// ユニティで使用するデータファイルの情報を格納するクラス
/// </summary>
public class AssetData
{
    public string Name { get; }
    public string Path { get; }
    public string Guid { get; }
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="name"></param>
    /// <param name="path"></param>
    /// <param name="guid"></param>
    public AssetData(string name, string path, string guid)
    {
        this.Name = name;
        this.Path = path;
        this.Guid = guid;
    }
    
    public static AssetData CreateByObject(Object obj)
    {
        var path = AssetDatabase.GetAssetPath(obj);
        var guid = AssetDatabase.AssetPathToGUID(path);
        var name = obj.name;
        return new AssetData(name, path, guid);
    }

    public static AssetData CreateByPath(string path)
    {
        var guid = AssetDatabase.AssetPathToGUID(path);
        var name = System.IO.Path.GetFileName(path);
        return new AssetData(name, path, guid);
    }
    
    public static AssetData CreateByGuid(string guid)
    {
        var path = AssetDatabase.GUIDToAssetPath(guid);
        var name = System.IO.Path.GetFileName(path);
        return new AssetData(name, path, guid);
    }

    public Object ToObject()
    {
        return AssetDatabase.LoadAssetAtPath<Object>(this.Path);
    }
    
    public override bool Equals(object obj)
    {
        var other = obj as AssetData;
        Debug.Assert(other != null);
        return this.Guid == other.Guid;
    }
    
    public override int GetHashCode()
    {
        return this.Guid.GetHashCode();
    }
}

public static class DictionaryExtension
{
    public static void AddSafety<K,V>(this IDictionary<K,V> self, K key, V value)
    {
        if (!self.ContainsKey(key))
        {
            self.Add(key, value);
        }
    }
}
