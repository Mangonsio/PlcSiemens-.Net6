using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConectionWithS71200.Contexto;

public partial class PlcReportsContext : DbContext
{
    public PlcReportsContext()
    {
    }

    public PlcReportsContext(DbContextOptions<PlcReportsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Lectura> Lecturas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=bd-prueba-estudiantil.database.windows.net;Database=PLC_Reports;User Id=Mango;Password=Mamut2017@;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lectura>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Lecturas__3214EC274BDF3728");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Fecha).HasMaxLength(1);
            entity.Property(e => e.Ia1)
                .HasMaxLength(1)
                .HasColumnName("IA_1");
            entity.Property(e => e.Out1).HasMaxLength(1);
            entity.Property(e => e.Out2).HasMaxLength(1);
            entity.Property(e => e.Out3).HasMaxLength(1);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
