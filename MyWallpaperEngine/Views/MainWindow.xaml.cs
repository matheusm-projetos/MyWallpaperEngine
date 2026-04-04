using System;
using System.Windows;
using System.Windows.Input;
using WinForms = System.Windows.Forms;

namespace MyWallpaperEngine.Views
{
    
    public partial class MainWindow : Window
    {

        private WinForms.NotifyIcon? _notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
            ConfigurarModoFantasma();
        }

        //Permite arrastar a janela por clicar na falsa borda
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        //botão minimizar customizado
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            FecharAplicativo();
        }

        private void ConfigurarModoFantasma()
        {
            _notifyIcon = new WinForms.NotifyIcon();

            //"Herda" o ícone do aplicativo para a bandeja do sistema
            _notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            _notifyIcon.Text = "MyWallpaperEngine";
            _notifyIcon.Visible = true;

            _notifyIcon.DoubleClick += (s, args) => RestaurarJanela();

            //Menu de clique direito na bandeja o sistema
            var menu = new WinForms.ContextMenuStrip();
            menu.Items.Add("Abrir", null, (s, args) => RestaurarJanela());
            menu.Items.Add("Sair", null, (s, args) => FecharAplicativo());

            _notifyIcon.ContextMenuStrip = menu;
        }

        //Monitora o estado da janela
        protected override void OnStateChanged(EventArgs e)
        {
            //Se a janela for minimizada, esconde a janela principal
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
            }
            base.OnStateChanged(e);
        }

        //Traz a janela de volta para o primeiro plano
        private void RestaurarJanela()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private void FecharAplicativo()
        {
            _notifyIcon?.Dispose();
            System.Windows.Application.Current.Shutdown();
        }
    }
}