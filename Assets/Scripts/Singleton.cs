using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected virtual bool DestoryTargetGameObject => false;

    public static T I { get; private set; } = null;

    public static bool IsValid() => I != null;

    private void Awake()
    {
        if (I == null){
            I = this as T;
            I.Init();
            return;
        }
        if (DestoryTargetGameObject){
            Destroy(gameObject);
        } else {
            Destroy(this);
        }
    }

    protected virtual void Init() { }

    private void OnDestroy(){
        if (I == this){
            I = null;
        }
        OnRelease();
    }

    /// <summary>
    /// 派生クラス用のOnDestroy
    /// </summary>
    protected virtual void OnRelease() { }
}
