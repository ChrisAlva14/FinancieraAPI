using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FinancieraAPI.Models;

public partial class FinancieraDbContext : DbContext
{
    public FinancieraDbContext()
    {
    }

    public FinancieraDbContext(DbContextOptions<FinancieraDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Empleo> Empleos { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Prestamo> Prestamos { get; set; }

    public virtual DbSet<Solicitud> Solicitudes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Clientes__71ABD087E14496D4");

            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Dui)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DUI");
            entity.Property(e => e.Nit)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NIT");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TelefonoCelular)
                .HasMaxLength(9)
                .IsUnicode(false);
            entity.Property(e => e.TelefonoFijo)
                .HasMaxLength(9)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Clientes_Usuarios");
        });

        modelBuilder.Entity<Empleo>(entity =>
        {
            entity.HasKey(e => e.EmpleoId).HasName("PK__Empleos__5C0587EA74E07C1D");

            entity.Property(e => e.Cargo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DireccionTrabajo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.LugarTrabajo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SueldoBase).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TelefonoTrabajo)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Cliente).WithMany(p => p.Empleos)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Empleos__Cliente__571DF1D5");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("PK__Pagos__F00B61386E843621");

            entity.Property(e => e.MontoPagado).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SaldoAcumulado).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Prestamo).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.PrestamoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pagos__PrestamoI__5441852A");
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasKey(e => e.PrestamoId).HasName("PK__Prestamo__AA58A0A0D8263DE1");

            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.MontoAprobado).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TasaInteres).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prestamos__Clien__5165187F");

            entity.HasOne(d => d.Solicitud).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.SolicitudId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prestamos__Solic__5070F446");
        });

        modelBuilder.Entity<Solicitud>(entity =>
        {
            entity.HasKey(e => e.SolicitudId).HasName("PK__Solicitu__85E95DC72E4B6F25");

            entity.Property(e => e.CantidadSolicitada).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DestinoPrestamo)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Solicitudes)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Solicitud__Clien__4D94879B");

            entity.HasOne(d => d.User).WithMany(p => p.Solicitudes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Solicitudes_Usuarios");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Usuarios__1788CCAC0142589D");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserRole)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
