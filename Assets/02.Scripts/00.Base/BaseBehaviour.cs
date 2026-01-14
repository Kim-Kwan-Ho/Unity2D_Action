using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


public class BaseBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        Initialize();
    }
    protected virtual void Initialize() { }

#if UNITY_EDITOR // 에디터에서만 작동하도록 설정
  
    protected virtual void OnButtonField() { } // 에디터 내에서 테스트 용도로 사용할 버튼

     
    #region ObjectFinder
    public enum EDataType 
    {
        prefab,
        asset,
        png

    }
    protected T FindObjectInAsset<T>() where T : UnityEngine.Object
    {
        // "t:TypeName" 필터로 타입 기반 검색
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null)
            {
                return asset;
            }
        }

        Debug.LogWarning($"No asset found of type {typeof(T).Name}.");
        return null;
    }
    protected T[] FindObjectsInAsset<T>() where T : UnityEngine.Object
    {
        // "t:TypeName" 필터로 타입 기반 검색
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");

        List<T> list = new List<T>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null)
            {
                list.Add(asset);
            }
        }

        return list.ToArray();
    }
    protected T FindObjectInAsset<T>(string name, EDataType type) where T : UnityEngine.Object
    {
        string typeName = Enum.GetName(typeof(EDataType), type);
        if (string.IsNullOrEmpty(typeName))
        {
            Debug.LogWarning($"Invalid EDataType: {type}");
            return null;
        }

        // name 과 확장자를 포함한 검색어
        string searchFilter = $"t:{typeof(T).Name} {name}";
        string[] guids = AssetDatabase.FindAssets(searchFilter);

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (!path.EndsWith($"{name}.{typeName}", System.StringComparison.OrdinalIgnoreCase))
                continue;

            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
            {
                Debug.LogWarning($"Asset at path {path} could not be loaded.");
                continue;
            }

            return asset;
        }

        Debug.LogWarning($"No asset found with name '{name}' and type '{typeName}'.");
        return null;
    }

    #endregion
    
    
    #region Validate

    protected void CheckNullValue(string objectName, UnityEngine.Object obj)
    {
        if (obj == null)
        {
            Debug.Log(objectName + " has null value");
        }
    }
    protected void CheckNullValue(string objectName, IEnumerable objs)
    {
        if (objs == null)
        {
            Debug.Log(objectName + "has null value");
            return;
        }
        foreach (var obj in objs)
        {
            if (obj == null)
            {
                Debug.Log(objectName + "has null value");
            }
        }
    }

    #endregion
    
    #region Binder
    protected virtual void OnBindField() { } // 바인딩 오브젝트를 사용할 버튼
    protected List<T> GetComponentsInChildrenExceptThis<T>() where T : Component
    {
        T[] components = GetComponentsInChildren<T>(true);
        List<T> list = new List<T>();
        foreach (T component in components)
        {
            if (component.gameObject.GetInstanceID() == this.gameObject.GetInstanceID())
            {
                continue;
            }
            else
            {
                list.Add(component);
            }
        }
        return list;
    }
    protected GameObject FindGameObjectInChildren(string name)
    {
        var objects = GetComponentsInChildren<Transform>(true);
        foreach (var obj in objects)
        {
            if (obj.gameObject.name.Equals(name))
                return obj.gameObject;
        }
        return null;
    }
    protected T FindGameObjectInChildren<T>(string name) where T : Component
    {
        T[] objects = GetComponentsInChildren<T>(true);
        foreach (var obj in objects)
        {
            if (obj.gameObject.name.Equals(name))
                return obj;
        }
        return null;
    }
    protected T[] GetComponentsInGameObject<T>(string name) where T : Component
    {
        GameObject gob = GameObject.Find(name);
        return gob.GetComponentsInChildren<T>(true);
    }

    protected T GetComponentInChildrenExceptThis<T>() where T : Component
    {
        T[] components = GetComponentsInChildren<T>(true);
        foreach (T component in components)
        {
            if (component.gameObject.GetInstanceID() == this.gameObject.GetInstanceID())
            {
                continue;
            }
            else
            {
                return component;
            }
        }

        return null;
    }
    #endregion
#endif 
}

#if UNITY_EDITOR
[CustomEditor(typeof(BaseBehaviour), true)]
[CanEditMultipleObjects]
public class BehaviourBaseEditor : Editor
{

    private MethodInfo _bindMethod = (typeof(BaseBehaviour)).GetMethod("OnBindField", BindingFlags.NonPublic | BindingFlags.Instance);
    private MethodInfo _buttonMethod = (typeof(BaseBehaviour)).GetMethod("OnButtonField", BindingFlags.NonPublic | BindingFlags.Instance);

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Active Button"))
        {
            _buttonMethod.Invoke(target, new object[] { });
            EditorUtility.SetDirty(target);
        }
        GUILayout.Space(50);
        if (GUILayout.Button("Bind Objects"))
        {
            _bindMethod.Invoke(target, new object[] { });
            EditorUtility.SetDirty(target);
        }
        GUILayout.Space(20);

        base.OnInspectorGUI();
    }

}
#endif