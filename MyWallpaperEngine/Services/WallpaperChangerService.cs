using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace MyWallpaperEngine.Services
{
    internal class WallpaperChangerService
    {
        private const int SPI_SETDESKWALLPAPER = 0x0014;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public bool DefinirComoFundo(string CaminhoImagem)
        {
            if (!File.Exists(CaminhoImagem))
                return false;

            int resultado = SystemParametersInfo(
                SPI_SETDESKWALLPAPER,
                0,
                CaminhoImagem,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

            return resultado != 0;

        }
    }
}
