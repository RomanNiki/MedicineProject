using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


#nullable disable

namespace PredpriyatieProject
{
    public partial class MedicineContext : DbContext
    {
        public MedicineContext()
        {
            if (Database.CanConnect()==false)
            {
                
            }
        }

        public MedicineContext(DbContextOptions<MedicineContext> options)
            : base(options)
        { 
        }
        
        public virtual DbSet<Rof> Rofs { get; set; }
        public virtual DbSet<Бригады> Бригадыs { get; set; }
        public virtual DbSet<ДокументыВещи> ДокументыВещиs { get; set; }
        public virtual DbSet<ЕдиницыИзмерения> ЕдиницыИзмеренияs { get; set; }
        public virtual DbSet<НаименованиеЛекарственныхСредств> НаименованиеЛекарственныхСредствs { get; set; }
        public virtual DbSet<ПриходРасход> ПриходРасходs { get; set; }
        public virtual DbSet<Приходвсего> Приходвсеs { get; set; }
        public virtual DbSet<Склады> Складыs { get; set; }
        public virtual DbSet<ТипДокументации> ТипДокументацииs { get; set; }
        public virtual DbSet<УходВсе> УходВсеs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
    
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Medicine;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Rof>(entity =>
            {
                entity.ToTable("ROFs");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.МаксимальноеКолВо)
                    .HasColumnName("Максимальное кол-во")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.МинимальноеКолВо)
                    .HasColumnName("Минимальное кол-во")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.ЦенностьNavigation)
                    .WithMany(p => p.Rofs)
                    .HasForeignKey(d => d.Ценность)
                    .HasConstraintName("ROFs$Наименование лекарственных средствROFs");
            });

            modelBuilder.Entity<Бригады>(entity =>
            {
                entity.ToTable("Бригады");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Название).HasMaxLength(255);
            });

            modelBuilder.Entity<ДокументыВещи>(entity =>
            {
                entity.ToTable("ДокументыВещи");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Количество).HasDefaultValueSql("((0))");

                entity.Property(e => e.НаименованиеЦенности).HasColumnName("Наименование ценности");

                entity.HasOne(d => d.ДокументNavigation)
                    .WithMany(p => p.ДокументыВещиs)
                    .HasForeignKey(d => d.Документ)
                    .HasConstraintName("ДокументыВещи$Приход\\РасходДокументыВещи");

                entity.HasOne(d => d.НаименованиеЦенностиNavigation)
                    .WithMany(p => p.ДокументыВещиs)
                    .HasForeignKey(d => d.НаименованиеЦенности)
                    .HasConstraintName("ДокументыВещи$Наименование лекарственных средствДокументыВещи");
            });

            modelBuilder.Entity<ЕдиницыИзмерения>(entity =>
            {
                entity.HasKey(e => e.Код)
                    .HasName("Единицы измерения$PrimaryKey");

                entity.ToTable("Единицы измерения");

                entity.Property(e => e.ЕдИзмерения)
                    .HasMaxLength(255)
                    .HasColumnName("Ед измерения");
            });

            modelBuilder.Entity<НаименованиеЛекарственныхСредств>(entity =>
            {
                entity.ToTable("Наименование лекарственных средств");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ЕдиницаИзмерения).HasColumnName("Единица измерения");

                entity.Property(e => e.Код).HasDefaultValueSql("((0))");

                entity.Property(e => e.НаименованиеЦенностей)
                    .HasMaxLength(255)
                    .HasColumnName("Наименование ценностей");

                entity.HasOne(d => d.ЕдиницаИзмеренияNavigation)
                    .WithMany(p => p.НаименованиеЛекарственныхСредствs)
                    .HasForeignKey(d => d.ЕдиницаИзмерения)
                    .HasConstraintName("Наименование лекарственных средств$Единицы измерения'Наим");
            });

            modelBuilder.Entity<ПриходРасход>(entity =>
            {
                entity.ToTable("Приход\\Расход");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Дата).HasPrecision(0);

                entity.Property(e => e.Название).HasMaxLength(255);

                entity.Property(e => e.ТипДокумента).HasColumnName("Тип документа");

                entity.HasOne(d => d.ПодразделениеNavigation)
                    .WithMany(p => p.ПриходРасходs)
                    .HasForeignKey(d => d.Подразделение)
                    .HasConstraintName("Приход\\Расход$БригадыПриход\\Расход");

                entity.HasOne(d => d.СкладNavigation)
                    .WithMany(p => p.ПриходРасходs)
                    .HasForeignKey(d => d.Склад)
                    .HasConstraintName("Приход\\Расход$СкладыПриход\\Расход");

                entity.HasOne(d => d.ТипДокументаNavigation)
                    .WithMany(p => p.ПриходРасходs)
                    .HasForeignKey(d => d.ТипДокумента)
                    .HasConstraintName("Приход\\Расход$Названия документовПриход\\Расход");
            });

            modelBuilder.Entity<Приходвсего>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Приходвсего");

                entity.Property(e => e.SumКоличество).HasColumnName("Количество");

                entity.Property(e => e.НаименованиеЦенностей)
                    .HasMaxLength(255)
                    .HasColumnName("Наименование ценностей");
            });

            modelBuilder.Entity<Склады>(entity =>
            {
                entity.ToTable("Склады");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Название).HasMaxLength(255);
            });

            modelBuilder.Entity<ТипДокументации>(entity =>
            {
                entity.ToTable("Тип документации");

                entity.Property(e => e.Название).HasMaxLength(255);
            });

            modelBuilder.Entity<УходВсе>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("УходВсего");

                entity.Property(e => e.SumКоличество).HasColumnName("Количество");

                entity.Property(e => e.НаименованиеЦенностей)
                    .HasMaxLength(255)
                    .HasColumnName("Наименование ценностей");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
