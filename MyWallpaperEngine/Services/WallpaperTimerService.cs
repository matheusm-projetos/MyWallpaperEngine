using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;

namespace MyWallpaperEngine.Services
{
    public class WallpaperTimerService
    {
        private readonly DispatcherTimer _timer;

        public event EventHandler? TempoEsgotado;

        public WallpaperTimerService()
        {
            _timer = new DispatcherTimer();

            _timer.Tick += (sender, e) => TempoEsgotado?.Invoke(this, EventArgs.Empty);
        }

        public void Iniciar(int minutos)
        {
            _timer.Interval = TimeSpan.FromMinutes(minutos);
            _timer.Start();
        }

        public void Parar()
        {
            _timer.Stop();
        }

        public bool EstaAtivo => _timer.IsEnabled;
    }
}
