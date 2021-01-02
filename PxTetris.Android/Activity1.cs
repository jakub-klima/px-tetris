using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;

namespace PxTetris.Android
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.FullUser, // ve stare impl. bylo: ScreenOrientation.Portrait
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize // ve stare impl. bylo jeste: | ConfigChanges.ScreenLayout
    )]
    public class Activity1 : AndroidGameActivity, View.IOnSystemUiVisibilityChangeListener
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var game = new AndroidTetrisGame();
            SetContentView(game.Services.GetService<View>());
            game.Run();

            Window.DecorView.SetOnSystemUiVisibilityChangeListener(this);
            HideSystemUI();
        }

        public void OnSystemUiVisibilityChange(StatusBarVisibility visibility)
        {
            HideSystemUI();
        }

        private void HideSystemUI()
        {
            var flags = SystemUiFlags.HideNavigation | SystemUiFlags.ImmersiveSticky | SystemUiFlags.Fullscreen;
            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)flags;
        }
    }
}
