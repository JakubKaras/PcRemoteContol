namespace PcRemoteControl;

public partial class AddOrEditDevicePage : ContentPage
{
	private AddOrEditDeviceViewModel _viewModel;

    public AddOrEditDevicePage(AddOrEditDeviceViewModel viewModel)
	{
		InitializeComponent();

		_viewModel = viewModel;
		BindingContext = viewModel;
	}

	private async void SaveBtn_Clicked(object sender, EventArgs e)
	{
        await Navigation.PopAsync();
    }
}