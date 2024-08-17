using UniRx;
using UnityEngine;

public interface IPlayerComponent
{
    void SetSubject(Subject<object> subject);
}


public class Player : MonoBehaviour
{
    public Subject<object> PlayerSubject { get; private set; }

    private void Awake()
    {
        // 初始化 Subject
        PlayerSubject = new Subject<object>();

        // 将 Subject 传递给子对象或子组件
        foreach (var component in GetComponentsInChildren<IPlayerComponent>())
        {
            component.SetSubject(PlayerSubject);
        }
    }

    private void OnDestroy()
    {
        // 销毁时释放 Subject，防止内存泄漏
        PlayerSubject.OnCompleted();
        PlayerSubject.Dispose();
    }
}
