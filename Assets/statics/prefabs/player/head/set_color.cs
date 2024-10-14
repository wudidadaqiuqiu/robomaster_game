using InterfaceDef;
using UniRx;
using UnityEngine;
using System.Linq;
namespace PlayerNameSpace {
public class setColor : MonoBehaviour, IPlayerComponent {
    private Subject<object> playerSubject;

    public void SetSubject(Subject<object> subject)
    {
        playerSubject = subject;

        playerSubject.Where(x => x is StructDef.TeamInfo.TeamInfo).Subscribe(_ => {
            Debug.Log("haed Rceeived TeamInfo message");
            // 在这里处理接收到的 TeamInfo 消息
        }
        ).AddTo(this);
    }
}
}