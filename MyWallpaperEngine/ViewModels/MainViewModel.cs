using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using MyWallpaperEngine.Data;
using MyWallpaperEngine.Models;
using MyWallpaperEngine.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyWallpaperEngine.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Wallpaper> wallpapers;

        [ObservableProperty]
        private string statusMessage;

        // Instância do serviço de escaneamento de arquivos
        private readonly FileScannerService _scannerService;
        private readonly WallpaperChangerService _changerService;

        public MainViewModel()
        {
            Wallpapers = new ObservableCollection<Wallpaper>();
            _scannerService = new FileScannerService();
            _changerService = new WallpaperChangerService();
            StatusMessage = "Pronto para importar imagens!";

            CarregarDados();
        }

        public void CarregarDados()
        {

            try
            {
                using (var context = new AppDbContext())
                {
                    context.Database.EnsureCreated();

                    var listaDoBanco = context.Wallpapers.Where(w => w.Ativo).ToList();

                    Wallpapers.Clear();
                    foreach (var item in listaDoBanco)
                    {
                        Wallpapers.Add(item);
                    }

                    StatusMessage = $"Concluído! {Wallpapers.Count} wallpapers carregados na galeria";
                }
            }
            catch (System.Exception ex)
            {
                StatusMessage = $"Erro no carregamento: {ex.Message}";
            }
        }

        [RelayCommand]
        public void SelecionarPasta()
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Selecione a pasta de Wallpapers"
            };

            if (dialog.ShowDialog() == true)
            {
                string pastaEscolhida = dialog.FolderName;
                StatusMessage = $"Importando pasta: {pastaEscolhida}...";

                var novosWallpapers = _scannerService.EscanearPasta(pastaEscolhida);

                if (novosWallpapers.Count > 0)
                {
                    SalvarNoBanco(novosWallpapers);
                }
                else
                {
                    StatusMessage = "Nenhuma imagem válida encontrada nessa pasta.";
                    MessageBox.Show("Não encontrei imagens válidas (.jpg, .png) nessa pasta", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void SalvarNoBanco(System.Collections.Generic.List<Wallpaper> novos)
        {
            using (var context = new AppDbContext())
            {
                int adicionados = 0;

                foreach (var wall in novos)
                {
                    bool jaExiste = context.Wallpapers.Any(w => w.CaminhoCompleto == wall.CaminhoCompleto);

                    if (!jaExiste)
                    {
                        context.Wallpapers.Add(wall);
                        adicionados++;
                    }
                }

                context.SaveChanges();
                StatusMessage = $"Importação concluída! {adicionados} novos wallpapers adicionados.";
            }

            CarregarDados();
        }

        [RelayCommand]
        public void AplicarWallpaper(Wallpaper WallpaperSelecionado)
        {
            if (WallpaperSelecionado == null) return;

            StatusMessage = $"Colando: {WallpaperSelecionado.NomeExibicao}...";

            bool sucesso = _changerService.DefinirComoFundo(WallpaperSelecionado.CaminhoCompleto);

            if (sucesso)
            {
                StatusMessage = $"'{WallpaperSelecionado.NomeExibicao}' aplicado com sucesso!";
            }
            else
            {
                StatusMessage = "Falha ao aplicar wallpaper.";
                MessageBox.Show("Não foi possível encontrar o arquivo de imagem", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
