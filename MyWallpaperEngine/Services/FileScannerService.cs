using MyWallpaperEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyWallpaperEngine.Services
{
    public class FileScannerService
    {
        //Lista de extensões aceitas
        private readonly string[] _extensoesPermitidas = { ".jpg", ".jpeg", ".png", ".bmp", ".webp" };

        public List<Wallpaper> EscanearPasta(string caminhoPasta)
        {
            var listaEncontrada = new List<Wallpaper>();

            //Verifica se a pasta realmente existe
            if (!Directory.Exists(caminhoPasta))
            {
                return listaEncontrada;
            }

            try
            {
                //Pega todos os arquivos da pasta
                var arquivos = Directory.GetFiles(caminhoPasta);

                foreach (var arquivoPath in arquivos)
                {
                    //Pega a extensão do aquivo em minúsculo
                    var extensao = Path.GetExtension(arquivoPath).ToLower();

                    if (_extensoesPermitidas.Contains(extensao))
                    {
                        //Cria o objeto Wallpaper sem salvar no banco
                        var wall = new Wallpaper
                        {
                            CaminhoCompleto = arquivoPath,
                            NomeExibicao = Path.GetFileNameWithoutExtension(arquivoPath),
                            DataAdicao = DateTime.Now,
                            Ativo = true,
                            Favorito = false
                        };

                        listaEncontrada.Add(wall);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao escanear: {ex.Message}");
            }

            return listaEncontrada;
        }

    }
}
