using MendMissionControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MendMissionControl.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<EquipamentoEspacial> EquipamentosEspaciais { get; set; }
    public DbSet<Satelite> Satelites { get; set; }
    public DbSet<NaveLimpezaOrbital> NavesLimpezaOrbital { get; set; }
    public DbSet<DetritoOrbital> DetritosOrbitais { get; set; }
    public DbSet<MissaoRemocao> MissoesRemocao { get; set; }
    public DbSet<Telemetria> Telemetrias { get; set; }
    public DbSet<MendCredit> MendCredits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EquipamentoEspacial>()
            .HasDiscriminator<string>("TipoEquipamento")
            .HasValue<NaveLimpezaOrbital>("NAVE_LIMPEZA_ORBITAL")
            .HasValue<Satelite>("SATELITE");

        modelBuilder.Entity<MendCredit>()
            .Property(c => c.ValorCredito)
            .HasPrecision(18, 2);

        modelBuilder.Entity<DetritoOrbital>()
            .Property(d => d.Removido)
            .HasColumnType("NUMBER(1)")
            .HasConversion<int>();

        modelBuilder.Entity<Satelite>()
            .Property(s => s.EstaAtivo)
            .HasColumnType("NUMBER(1)")
            .HasConversion<int>();

        modelBuilder.Entity<NaveLimpezaOrbital>()
            .Property(n => n.PossuiLaserAblacao)
            .HasColumnType("NUMBER(1)")
            .HasConversion<int>();

        modelBuilder.Entity<NaveLimpezaOrbital>()
            .Property(n => n.PossuiGarrasCaptura)
            .HasColumnType("NUMBER(1)")
            .HasConversion<int>();
    }
}
