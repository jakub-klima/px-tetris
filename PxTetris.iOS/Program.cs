using Foundation;
using UIKit;

namespace PxTetris.iOS
{
    [Register("AppDelegate")]
    class Program : UIApplicationDelegate
    {
        private static iOSTetrisGame game;

        internal static void RunGame()
        {
            game = new iOSTetrisGame();
            game.Run();
        }

        static void Main(string[] args)
        {
            UIApplication.Main(args, null, "AppDelegate");
        }

        public override void FinishedLaunching(UIApplication app)
        {
            RunGame();
        }
    }
}
