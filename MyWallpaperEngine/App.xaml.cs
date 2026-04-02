using System.Configuration;
using System.Data;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Threading;

namespace MyWallpaperEngine
{
    public partial class App : System.Windows.Application
    {
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show(
                $"Atenção!! Ocorreu um erro na execução do programa.\n\n Detalhe técnico: {e.Exception.Message}",
                "MyWallpaperEngine - Erro Inesperado",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            e.Handled = true;
        }
    }

}
