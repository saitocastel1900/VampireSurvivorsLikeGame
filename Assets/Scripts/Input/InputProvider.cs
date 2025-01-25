using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class InputProvider : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public IReactiveProperty<Vector2> MoveDirection => _moveDirection;
    private Vector2ReactiveProperty _moveDirection = new Vector2ReactiveProperty();

    /// <summary>
    /// 
    /// </summary>
    public IReactiveProperty<bool> IsRun => _isRun;
    private readonly BoolReactiveProperty _isRun = new BoolReactiveProperty();
    
    /// <summary>
    /// 
    /// </summary>
    public IObservable<Unit> OnAttack => _attackSubject;
    private ISubject<Unit> _attackSubject = new Subject<Unit>();
 
    /// <summary>
    /// 
    /// </summary>
    private GamePadInputs _inputAction;
    
    private void Awake()
    {
        _inputAction = new GamePadInputs();

        Observable.Merge(
                _inputAction.Player.Move.GetStartedObservable(),
            _inputAction.Player.Move.GetPerformedObservable(),
            _inputAction.Player.Move.GetCanceldObservable()
        )
            .Select(x => x.ReadValue<Vector2>())
            .Subscribe(x=>_moveDirection.Value = x)
            .AddTo(this.gameObject);
        
        Observable.Merge(
                _inputAction.Player.Run.GetStartedObservable().Select(_=>true),
                _inputAction.Player.Run.GetPerformedObservable().Select(_=>true),
                _inputAction.Player.Run.GetCanceldObservable().Select(_=>false)
            )
            .Subscribe(x=>_isRun.Value = x)
            .AddTo(this.gameObject);
        
        _inputAction.Player.Attack.GetPerformedObservable()
            .Subscribe(_ => _attackSubject.OnNext(Unit.Default))
            .AddTo(this.gameObject);
        
        _inputAction.Enable();
        
        this.gameObject.
            OnDestroyAsObservable()
            .Subscribe(_=>   _inputAction?.Dispose())
            .AddTo(this.gameObject);
    }

    private void OnDestroy()
    { 
        _inputAction?.Dispose();
    }
}
