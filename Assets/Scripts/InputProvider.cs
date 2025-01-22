using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputProvider : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public ReactiveProperty<Vector2> MoveInputValue =  new ReactiveProperty<Vector2>();
    public BoolReactiveProperty IsRun = new BoolReactiveProperty();
 
    /// <summary>
    /// 
    /// </summary>
    private GamePadInputs _gameInputs;
    
    private void Awake()
    {
        // Actionスクリプトのインスタンス生成
        _gameInputs = new GamePadInputs();

        Observable
            .FromEvent<InputAction.CallbackContext>(
                h => _gameInputs.Player.Move.started += h,
                h => _gameInputs.Player.Move.started -= h
            )
            .Select(x => x.ReadValue<Vector2>())
            .Subscribe(x=>MoveInputValue.Value = x)
            .AddTo(this.gameObject);
        
        Observable
            .FromEvent<InputAction.CallbackContext>(
                h => _gameInputs.Player.Move.performed += h,
                h => _gameInputs.Player.Move.performed -= h
            )
            .Select(x => x.ReadValue<Vector2>())
            .Subscribe(x=>MoveInputValue.Value = x)
            .AddTo(this.gameObject);
        
        Observable
            .FromEvent<InputAction.CallbackContext>(
                h => _gameInputs.Player.Move.canceled += h,
                h => _gameInputs.Player.Move.canceled -= h
            )
            .Select(x => x.ReadValue<Vector2>())
            .Subscribe(x=>MoveInputValue.Value = x)
            .AddTo(this.gameObject);
        
        Observable
            .FromEvent<InputAction.CallbackContext>(
                h => _gameInputs.Player.Run.started += h,
                h => _gameInputs.Player.Run.started -= h
            )
            .Subscribe(x=>IsRun.Value = true)
            .AddTo(this.gameObject);
        
        Observable
            .FromEvent<InputAction.CallbackContext>(
                h => _gameInputs.Player.Run.canceled += h,
                h => _gameInputs.Player.Run.canceled -= h
            )
            .Subscribe(x=>IsRun.Value = false)
            .AddTo(this.gameObject);
          
        _gameInputs.Enable();
    }

    private void OnDestroy()
    { 
        _gameInputs?.Dispose();
    }
}
