using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Instance;
    static Managers Instance { get { Init(); return s_Instance; } }

    InputManager _input = new InputManager();
    ResourceManager _resource = new ResourceManager();
    UIManager _ui = new UIManager();
    SceneManagerEX _scene = new SceneManagerEX();
    SoundManager _sound = new SoundManager();
    PoolManager _pool = new PoolManager();
    DataManager _data = new DataManager();
    public static InputManager Input { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static SceneManagerEX Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static DataManager Data { get { return Instance._data; } }
    void Start()
    {
        Init();
    }
    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {
        if(s_Instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject()
                { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<Managers>();
            s_Instance._data.Init();
            s_Instance._pool.Init();
            s_Instance._sound.Init();
        }
        
    }

    static public void Clear()
    {
        Input.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }
}
