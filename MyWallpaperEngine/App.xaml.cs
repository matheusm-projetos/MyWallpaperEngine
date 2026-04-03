using System.Windows;
using System.Windows.Threading;
using Serilog;

namespace MyWallpaperEngine
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Configura arquivo de log ao iniciar o aplicativo
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()

                //Configura o log para ser salvo em arquivos diários na pasta "Logs" dentro do diretório do aplicativo
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("===============================================");
            Log.Information("MyWallpaperEngine iniciado.");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("MyWallpaperEngine encerrado.");
            Log.CloseAndFlush();
            base.OnExit(e);
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Fatal(e.Exception, "Erro Crítico");

            System.Windows.MessageBox.Show(
                $"Atenção!! Ocorreu um erro na execução do programa.\n\n Detalhe técnico: {e.Exception.Message}",
                "MyWallpaperEngine - Erro Inesperado",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            e.Handled = true;
        }
    }

}
