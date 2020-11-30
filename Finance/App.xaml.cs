using Finance.View;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Finance
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            string androidAppSecret = "33093855-2100-4de2-b650-bbbd9257bd19";
            string iOSAppSecret = "3e7e7ff8-f5f9-49bc-805f-0aa3e9a637d3";
            AppCenter.Start($"android{androidAppSecret};ios={iOSAppSecret}", typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
