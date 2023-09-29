using Microsoft.AspNetCore.Components;

namespace Blax;
public class ObserverBase : ComponentBase, IDisposable
{
    private Action? _stateChangedAction;

    [Parameter] public ObservableState? State { get; set; }

    protected override void OnInitialized()
    {
        if (State == null)
        {
            throw new InvalidOperationException("State parameter is null. You must provide a State instance.");
        }

        _stateChangedAction = () => { InvokeAsync(StateHasChanged); };
        State.Subscribe(_stateChangedAction);
    }

    public void Dispose()
    {
        if (_stateChangedAction != null)
        {
            State?.Unsubscribe(_stateChangedAction);
        }
    }
}
