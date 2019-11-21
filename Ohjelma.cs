#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Program
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using ( var game = new Attack_of_8_Bit() )
                game.Run();
        }
    }

    internal class Attack_of_8_Bit : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        internal void Run()
        {
            throw new NotImplementedException();
        }
    }
#endif
}
