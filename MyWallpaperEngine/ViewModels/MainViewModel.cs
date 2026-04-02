using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using MyWallpaperEngine.Data;
using MyWallpaperEngine.Models;
using MyWallpaperEngine.Services;
using System;
using System.Collections.ObjectModel;
using System.IO.Packaging;
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

        [ObservableProperty]
        private int tempoSelecionado = 5;

        [ObservableProperty]
        private string textoBotaoTimer = "Shuffle: Off";

        [ObservableProperty]
        private bool isTimerAtivo = false;

        public ObservableCollection<int> OpcoesTempo { get; } = new() { 1, 5, 10, 15, 30, 60 };


        // Instância do serviço de escaneamento de arquivos
        private readonly FileScannerService _scannerService;
        private readonly WallpaperChangerService _changerService;
        private readonly WallpaperTimerService _timerService;

        public MainViewModel()
        {
            Wallpapers = new ObservableCollection<Wallpaper>();

            _scannerService = new FileScannerService();

            _changerService = new WallpaperChangerService();
            
            _timerService = new WallpaperTimerService();
            _timerService.TempoEsgotado += OnTimerTempoEsgotado;

            StatusMessage = "Pronto para importar imagens!";

            CarregarDados();
        }

        //Quando o timer termina, ativa o evento de troca de wallpaper
        private void OnTimerTempoEsgotado(object? sender, EventArgs e)
        {
            if (Wallpapers.Count == 0) return;

            var random = new Random();
            int index = random.Next(Wallpapers.Count);

            var wallpaperSorteado = Wallpapers[index];
            AplicarWallpaper(wallpaperSorteado);
        }

        //Ativa e desativa o shuffle juntamente do timer
        [RelayCommand]
        public void ToggleTimer()
        {
            if (IsTimerAtivo)
            {
                _timerService.Parar();
                IsTimerAtivo = false;

                TextoBotaoTimer = "Shuffle: Off";
                StatusMessage = "Timer parado.";
            }
            else
            {
                if (Wallpapers.Count == 0)
                {
                    StatusMessage = "Adicione wallpapers para ligar o Shuffle!";
                    return;
                }

                _timerService.Iniciar(TempoSelecionado);
                IsTimerAtivo = true;
                TextoBotaoTimer = "Shuffle: On";
                StatusMessage = $"Piloto automático iniciado. Trocando a cada {TempoSelecionado} min.";
            }
        }

        //Carrega as imagens da memória para a listagem principal
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

        //Seleciona a pasta de que as imagens serão carregadas
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
                    System.Windows.MessageBox.Show("Não encontrei imagens válidas (.jpg, .png) nessa pasta", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        //Aplica o wallpaper selecionado na Área de Trabalho
        [RelayCommand]
        public void AplicarWallpaper(Wallpaper? WallpaperSelecionado)
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
                System.Windows.MessageBox.Show("Não foi possível encontrar o arquivo de imagem", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
