using PcRemoteControl.Animations;

namespace PcRemoteControl.Triggers
{
    class LoadingDeviceBackgroundTrigger : TriggerAction<VisualElement>
    {
        protected override async void Invoke(VisualElement sender)
        {
            await sender.MoveGradientStop((brush) => sender.Background = brush);
        }
    }
}
