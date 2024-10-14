using UniRx;

namespace InterfaceDef {
    public interface IPlayerComponent
    {
        void SetSubject(Subject<object> subject);
    }
}