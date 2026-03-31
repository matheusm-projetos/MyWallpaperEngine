using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.EntityFrameworkCore;
using MyWallpaperEngine.Models;

namespace MyWallpaperEngine.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Wallpaper> Wallpapers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=mywallpaper.db");
        }
    }
}
