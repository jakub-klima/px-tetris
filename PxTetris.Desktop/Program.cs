using PxTetris.Core;
using System;

namespace PxTetris.Desktop
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new TetrisGame();
            game.Run();
        }
    }
}
