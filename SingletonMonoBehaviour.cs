using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
    private static T instance;
    public static T Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<T>();
                if (instance == null) {
                    GameObject gameObject = new GameObject("Auto generated" + typeof(T));
                    instance = gameObject.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake() {
        OnAwake();
    }
    protected virtual void OnAwake() {

    }
}
