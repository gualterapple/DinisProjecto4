using System;
using DinisProjecto4.ViewModels;
using DinisProjecto4.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DinisProjecto4
{
    public partial class App : Application
    {
        public App()
        {
            Main = new MainViewModel();
            InitializeComponent();
            var page = new LoginPage();
            NavigationPage.SetHasNavigationBar(page, false);
            MainPage = new NavigationPage(page);
        }
        public MainViewModel Main
        {
            get;
            set;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
