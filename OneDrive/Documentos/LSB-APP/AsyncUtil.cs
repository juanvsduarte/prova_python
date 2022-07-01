using LSB.App.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LSB.App
{
    public class AsyncUtil : IDisposable
    {
        private WaitWindow _window;
        private Thread trd;


        public void Start(string title)
        {
            _window = new WaitWindow() { Text = title, Visible = true };
            ThreadStart threadStart = new ThreadStart(() =>
            {
                _window.Invoke(new Action(() => { _window.Show(); }));
                
            });
            trd = new Thread(threadStart);
            trd.Start();
        }

        public void Dispose()
        {
            _window.Invoke(new Action(() => { _window.Close(); }));
            trd.Abort();
        }
    }
}
