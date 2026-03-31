using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MyWallpaperEngine.Models
{
    public class Wallpaper
    {
        [Key]
        public int Id { get; set; }
        public string CaminhoCompleto { get; set; } = string.Empty;
        public string NomeExibicao { get; set; } = string.Empty;
        public string CaminhoThumb { get; set; } = string.Empty;
        public bool Favorito { get; set; } = false;
        public bool Ativo { get; set; } = true;
        public DateTime DataAdicao { get; set; } = DateTime.Now;
        public string Tags { get; set; } = string.Empty;
    }
}
