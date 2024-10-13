namespace PcRemoteControl;

public partial class AddOrEditDevicePage : ContentPage
{
    public AddOrEditDevicePage(AddOrEditDeviceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void SaveBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}