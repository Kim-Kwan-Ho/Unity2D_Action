using UnityEngine;

public abstract class SingletonBehaviour<T> : BaseBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance { get { return _instance; } }

    protected override void Awake()
    {
        if (_instance == null)
        {
            base.Awake();
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public abstract void Init();

}
