namespace DynamicDataLibrary.ViewModel
{
    using System;
    using System.Diagnostics;
    using System.Windows;

    public class VMTimer
    {
        private Stopwatch stopwatch = new Stopwatch();
        private string name = string.Empty;

        public VMTimer(string name)
        {
            this.stopwatch.Start();
            this.name = name;
        }

        public void End()
        {
            this.stopwatch.Stop();
            TimeSpan ts = this.stopwatch.Elapsed;
            string str = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            MessageBox.Show(this.name + "/" + str);
        }
    }
}
