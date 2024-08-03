namespace PcRemoteControl
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage(new MainViewModel()));
        }
    }
}
