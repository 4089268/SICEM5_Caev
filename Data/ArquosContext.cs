using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Sicem_Blazor.Models.Entities.Arquos;

namespace Sicem_Blazor.Data
{
    public partial class ArquosContext : DbContext
    {
        private readonly string _cadenaConexion;
        public ArquosContext(string cadConexion )
        {
            this._cadenaConexion = cadConexion;
        }

        public ArquosContext(DbContextOptions<ArquosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CatAnomalias> CatAnomaliases { get; set; }
        public virtual DbSet<CatClasesUsuario> CatClasesUsuarios { get; set; }
        public virtual DbSet<CatColonia> CatColonias { get; set; }
        public virtual DbSet<CatConcepto> CatConceptos { get; set; }
        public virtual DbSet<CatEstatus> CatEstatuses { get; set; }
        public virtual DbSet<CatGiro> CatGiros { get; set; }
        public virtual DbSet<CatNivelesSocioEconomico> CatNivelesSocioEconomicos { get; set; }
        public virtual DbSet<CatServicio> CatServicios { get; set; }
        public virtual DbSet<CatTarifa> CatTarifas { get; set; }
        public virtual DbSet<CatTiposCalculo> CatTiposCalculos { get; set; }
        public virtual DbSet<CatTiposUsuario> CatTiposUsuarios { get; set; }
        public virtual DbSet<VwCatPadron> VwCatPadrons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_cadenaConexion);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatAnomalias>(entity =>
            {
                entity.HasKey(e => e.IdAnomalia)
                    .HasName("PK_Fac_Cat_Anomalias");

                entity.ToTable("Cat_Anomalias", "Facturacion");

                entity.Property(e => e.IdAnomalia)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_anomalia");

                entity.Property(e => e.CveWang)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("cve_wang");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.EnCampo)
                    .HasColumnName("en_campo")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FuncionaMedidor)
                    .IsRequired()
                    .HasColumnName("funciona_medidor")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdGenera1)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_genera1");

                entity.Property(e => e.IdGenera2)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_genera2");

                entity.Property(e => e.IdSystema)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("id_systema");

                entity.Property(e => e.IdTipoanomalia)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_tipoanomalia")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IdTipocalculo)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_tipocalculo");

                entity.Property(e => e.Inactivo).HasColumnName("inactivo");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("observaciones");

                entity.HasOne(d => d.IdTipocalculoNavigation)
                    .WithMany(p => p.CatAnomaliases)
                    .HasForeignKey(d => d.IdTipocalculo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cat_Anomalias_Cat_TiposCalculo");
            });

            modelBuilder.Entity<CatClasesUsuario>(entity =>
            {
                entity.HasKey(e => e.IdClaseUsuario)
                    .HasName("PK_Cat_Clasesusuario");

                entity.ToTable("Cat_ClasesUsuario", "Padron");

                entity.HasIndex(e => e.Descripcion, "IX_Cat_ClasesUsuario_Desc")
                    .IsUnique();

                entity.Property(e => e.IdClaseUsuario)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_ClaseUsuario");

                entity.Property(e => e.Condicionar).HasColumnName("condicionar");

                entity.Property(e => e.CveDescuento)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("cve_descuento")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Desctoxanticipo)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("desctoxanticipo")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Inactivo).HasColumnName("inactivo");

                entity.Property(e => e.Porcentaje)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("porcentaje");
            });

            modelBuilder.Entity<CatColonia>(entity =>
            {
                entity.HasKey(e => e.IdColonia);

                entity.ToTable("Cat_Colonias", "Padron");

                entity.Property(e => e.IdColonia)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_colonia");

                entity.Property(e => e.Anticipo)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("anticipo");

                entity.Property(e => e.CalidadServicio)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("calidad_servicio")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Horario)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("horario")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdMaterialcalle)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_materialcalle");

                entity.Property(e => e.IdPoblacion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_poblacion")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IdRegion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_region")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IdServicio)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_servicio");

                entity.Property(e => e.IdTipousuario)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_tipousuario");

                entity.Property(e => e.Inactivo)
                    .HasColumnName("inactivo")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ManzanaFin1)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("manzana_fin1");

                entity.Property(e => e.ManzanaFin2)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("manzana_fin2");

                entity.Property(e => e.ManzanaFin3)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("manzana_fin3");

                entity.Property(e => e.ManzanaIni1)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("manzana_ini1");

                entity.Property(e => e.ManzanaIni2)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("manzana_ini2");

                entity.Property(e => e.ManzanaIni3)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("manzana_ini3");

                entity.Property(e => e.Plazos)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("plazos");

                entity.Property(e => e.Politicas)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("politicas")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Sb)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("sb");

                entity.Property(e => e.Sector)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("sector");

                entity.Property(e => e.Suministro)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("suministro")
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.IdServicioNavigation)
                    .WithMany(p => p.CatColonia)
                    .HasForeignKey(d => d.IdServicio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cat_Colonias_Cat_Servicios");

                entity.HasOne(d => d.IdTipousuarioNavigation)
                    .WithMany(p => p.CatColonia)
                    .HasForeignKey(d => d.IdTipousuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cat_Colonias_Cat_TiposUsuario");
            });

            modelBuilder.Entity<CatConcepto>(entity =>
            {
                entity.HasKey(e => e.IdConcepto)
                    .HasName("PK_Pad_Cat_OtrosServ");

                entity.ToTable("Cat_Conceptos", "Padron");

                entity.HasIndex(e => new { e.Descripcion, e.IdSystema }, "IX_Cat_Conceptos_Desc")
                    .IsUnique();

                entity.Property(e => e.IdConcepto)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_concepto");

                entity.Property(e => e.Clave)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("clave")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ClaveFija).HasColumnName("clave_fija");

                entity.Property(e => e.ClaveRezago)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("clave_rezago")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CostoEstatico)
                    .HasColumnName("costo_estatico")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Credito).HasColumnName("credito");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.IdEstatus)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_estatus");

                entity.Property(e => e.IdSystema)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("id_systema");

                entity.Property(e => e.IdTipoconcepto)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_tipoconcepto");

                entity.Property(e => e.Importe)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("importe");

                entity.Property(e => e.Inactivo).HasColumnName("inactivo");

                entity.Property(e => e.Mostrar).HasColumnName("mostrar");

                entity.Property(e => e.Ordinal)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ordinal");

                entity.Property(e => e.RezOrdinal)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("rez_ordinal");

                entity.Property(e => e.TasaIva)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("tasa_iva");

                entity.Property(e => e.TipoIva)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("tipo_iva")
                    .IsFixedLength();
            });

            modelBuilder.Entity<CatEstatus>(entity =>
            {
                entity.HasKey(e => e.IdEstatus);

                entity.ToTable("Cat_Estatus", "Global");

                entity.HasIndex(e => new { e.Descripcion, e.Tabla }, "IX_Cat_ST_Desc_Tab")
                    .IsUnique();

                entity.Property(e => e.IdEstatus)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_estatus");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Tabla)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tabla");
            });

            modelBuilder.Entity<CatGiro>(entity =>
            {
                entity.HasKey(e => e.IdGiro);

                entity.ToTable("Cat_Giros", "Padron");

                entity.HasIndex(e => e.Descripcion, "IX_Cat_Giros_Desc")
                    .IsUnique();

                entity.Property(e => e.IdGiro)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_giro");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Inactivo).HasColumnName("inactivo");
            });

            modelBuilder.Entity<CatNivelesSocioEconomico>(entity =>
            {
                entity.HasKey(e => e.IdNivelsocial)
                    .HasName("PK_Padron.Cat_NivelesSocioEconomicos");

                entity.ToTable("Cat_NivelesSocioEconomicos", "Padron");

                entity.Property(e => e.IdNivelsocial)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_nivelsocial");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Inactivo).HasColumnName("inactivo");
            });

            modelBuilder.Entity<CatServicio>(entity =>
            {
                entity.HasKey(e => e.IdServicio);

                entity.ToTable("Cat_Servicios", "Facturacion");

                entity.HasIndex(e => e.Descripcion, "IX_Cat_Servicios")
                    .IsUnique();

                entity.Property(e => e.IdServicio)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_Servicio");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Inactivo).HasColumnName("inactivo");
            });

            modelBuilder.Entity<CatTarifa>(entity =>
            {
                entity.HasKey(e => e.IdTarifa);

                entity.ToTable("Cat_Tarifas", "Facturacion");

                entity.HasIndex(e => new { e.IdPoblacion, e.IdTipoUsuario }, "IX_Cat_Tarifas_IdPob_Usuario");

                entity.HasIndex(e => e.IdTipoUsuario, "idx_facturacion");

                entity.Property(e => e.IdTarifa)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_tarifa");

                entity.Property(e => e.Costo)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("costo");

                entity.Property(e => e.Desde)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("desde");

                entity.Property(e => e.FactorDrenaje)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("factor_drenaje");

                entity.Property(e => e.FactorReferencia)
                    .HasColumnType("numeric(7, 6)")
                    .HasColumnName("factor_referencia");

                entity.Property(e => e.FactorTratamto)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("factor_tratamto");

                entity.Property(e => e.FechaEdito)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_edito");

                entity.Property(e => e.Hasta)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("hasta");

                entity.Property(e => e.IdEdito)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("id_edito");

                entity.Property(e => e.IdPoblacion)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("id_poblacion")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IdTipoUsuario)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_TipoUsuario");

                entity.Property(e => e.Inactivo).HasColumnName("inactivo");

                entity.Property(e => e.M3Base)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("m3_base");

                entity.Property(e => e.TasaIva)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("tasa_iva");

                entity.HasOne(d => d.IdTipoUsuarioNavigation)
                    .WithMany(p => p.CatTarifas)
                    .HasForeignKey(d => d.IdTipoUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cat_Tarifas_Cat_TiposUsuario");
            });

            modelBuilder.Entity<CatTiposCalculo>(entity =>
            {
                entity.HasKey(e => e.IdCalculo)
                    .HasName("PK_Fac_Cat_Tiposcalculo");

                entity.ToTable("Cat_TiposCalculo", "Facturacion");

                entity.HasIndex(e => e.Descripcion, "IX_Cat_TipoCalc_Desc")
                    .IsUnique();

                entity.Property(e => e.IdCalculo)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_calculo");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Inactivo).HasColumnName("inactivo");
            });

            modelBuilder.Entity<CatTiposUsuario>(entity =>
            {
                entity.HasKey(e => e.IdTipousuario)
                    .HasName("PK_Pad_Cat_Tiposusuario");

                entity.ToTable("Cat_TiposUsuario", "Padron");

                entity.HasIndex(e => e.Descripcion, "IX_Cat_TiposUsuario_Desc")
                    .IsUnique();

                entity.Property(e => e.IdTipousuario)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_tipousuario");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("descripcion");

                entity.Property(e => e.FactorDrenaje)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("factor_drenaje")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FactorTratamto)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("factor_tratamto")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ImpAguaFijo)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("imp_agua_fijo")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ImpDrenajeFijo)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("imp_drenaje_fijo")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ImpSaneamientoFijo)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("imp_saneamiento_fijo");

                entity.Property(e => e.Inactivo).HasColumnName("inactivo");

                entity.Property(e => e.MetrosBase)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("metros_base")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TasaIva)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("tasa_iva")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Tipo)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("tipo")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.VecesPromAlto)
                    .HasColumnType("decimal(10, 4)")
                    .HasColumnName("veces_prom_alto")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.VecesPromMenor)
                    .HasColumnType("decimal(10, 4)")
                    .HasColumnName("veces_prom_menor")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<VwCatPadron>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Cat_Padron", "Padron");

                entity.Property(e => e.Af)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("af");

                entity.Property(e => e.AltaFactura)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("_alta_factura");

                entity.Property(e => e.AnomaliaAct)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_anomalia_act");

                entity.Property(e => e.AnomaliaAnt)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_anomalia_ant");

                entity.Property(e => e.Anual).HasColumnName("anual");

                entity.Property(e => e.AreaConstruida)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("area_construida");

                entity.Property(e => e.AreaJardin)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("area_jardin");

                entity.Property(e => e.AreaLote)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("area_lote");

                entity.Property(e => e.Calculo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_calculo");

                entity.Property(e => e.CalculoAct)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_calculo_act");

                entity.Property(e => e.CalculoAnt)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_calculo_ant");

                entity.Property(e => e.CalleLat1)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("calle_lat1");

                entity.Property(e => e.CalleLat11)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_calle_lat1");

                entity.Property(e => e.CalleLat2)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("calle_lat2");

                entity.Property(e => e.CalleLat21)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_calle_lat2");

                entity.Property(e => e.CallePpal)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("calle_ppal");

                entity.Property(e => e.CallePpal1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_calle_ppal");

                entity.Property(e => e.Ciudad)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ciudad");

                entity.Property(e => e.Claseusuario)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_claseusuario");

                entity.Property(e => e.CodigoPostal)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("codigo_postal");

                entity.Property(e => e.Colonia)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("colonia");

                entity.Property(e => e.Colonia1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("_colonia");

                entity.Property(e => e.ConsumoAct)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("consumo_act");

                entity.Property(e => e.ConsumoAnt)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("consumo_ant");

                entity.Property(e => e.ConsumoFijo)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("consumo_fijo");

                entity.Property(e => e.ConsumoForzado)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("consumo_forzado");

                entity.Property(e => e.ConsumoRealAct)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("consumo_real_act");

                entity.Property(e => e.ConsumoRealAnt)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("consumo_real_ant");

                entity.Property(e => e.Curp)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("curp");

                entity.Property(e => e.Desviacion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("desviacion");

                entity.Property(e => e.Diametro)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_diametro");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(356)
                    .IsUnicode(false)
                    .HasColumnName("direccion");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.EsAltoconsumidor).HasColumnName("es_altoconsumidor");

                entity.Property(e => e.EsDraef).HasColumnName("es_draef");

                entity.Property(e => e.EsFiscal).HasColumnName("es_fiscal");

                entity.Property(e => e.EsMacromedidor).HasColumnName("es_macromedidor");

                entity.Property(e => e.Estado)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.Estatus)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_estatus");

                entity.Property(e => e.FechaAlta)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("_fecha_alta");

                entity.Property(e => e.FechaFacturaAct)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("_fecha_factura_act");

                entity.Property(e => e.FechaFacturaAnt)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("_fecha_factura_ant");

                entity.Property(e => e.FechaInsert)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("_fecha_insert");

                entity.Property(e => e.FechaLecturaAct)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("_fecha_lectura_act");

                entity.Property(e => e.FechaLecturaAnt)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("_fecha_lectura_ant");

                entity.Property(e => e.FechaVencimiento)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("_fecha_vencimiento");

                entity.Property(e => e.FechaVencimientoAct)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("_fecha_vencimiento_act");

                entity.Property(e => e.Fraccion)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("fraccion");

                entity.Property(e => e.Frente)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("frente");

                entity.Property(e => e.Giro)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("_giro");

                entity.Property(e => e.Hidrocircuito)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_hidrocircuito");

                entity.Property(e => e.IdAnomaliaAct)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_anomalia_act");

                entity.Property(e => e.IdClaseusuario)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_claseusuario");

                entity.Property(e => e.IdColonia)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_colonia");

                entity.Property(e => e.IdCuenta)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_cuenta");

                entity.Property(e => e.IdEstatus)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_estatus");

                entity.Property(e => e.IdGiro)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_giro");

                entity.Property(e => e.IdLocalidad)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_localidad");

                entity.Property(e => e.IdMedidor)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("id_medidor");

                entity.Property(e => e.IdPadron)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_padron");

                entity.Property(e => e.IdServicio)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_servicio");

                entity.Property(e => e.IdSituacion)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_situacion");

                entity.Property(e => e.IdTarifa)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_tarifa");

                entity.Property(e => e.IdTarifafija)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_tarifafija");

                entity.Property(e => e.IdTipocalculo)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_tipocalculo");

                entity.Property(e => e.IdTipodraef)
                    .HasColumnType("numeric(1, 1)")
                    .HasColumnName("id_tipodraef");

                entity.Property(e => e.ImporteFijo)
                    .HasColumnType("money")
                    .HasColumnName("importe_fijo");

                entity.Property(e => e.ImporteFijoDren)
                    .HasColumnType("money")
                    .HasColumnName("importe_fijo_dren");

                entity.Property(e => e.ImporteFijoSane)
                    .HasColumnType("money")
                    .HasColumnName("importe_fijo_sane");

                entity.Property(e => e.Iva)
                    .HasColumnType("money")
                    .HasColumnName("iva");

                entity.Property(e => e.LastIdAbono)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("last_idAbono");

                entity.Property(e => e.LastIdVenta)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("last_idVenta");

                entity.Property(e => e.Latitud)
                    .HasColumnType("decimal(10, 8)")
                    .HasColumnName("latitud");

                entity.Property(e => e.LecturaAct)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("lectura_act");

                entity.Property(e => e.LecturaAnt)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("lectura_ant");

                entity.Property(e => e.Localizacion)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("_localizacion");

                entity.Property(e => e.Longitud)
                    .HasColumnType("decimal(10, 8)")
                    .HasColumnName("longitud");

                entity.Property(e => e.Lote)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("lote");

                entity.Property(e => e.LpsPagados)
                    .HasColumnType("decimal(8, 4)")
                    .HasColumnName("lps_pagados");

                entity.Property(e => e.Manzana)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("manzana");

                entity.Property(e => e.MaterialBanqueta)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_material_banqueta");

                entity.Property(e => e.MaterialCalle)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_material_calle");

                entity.Property(e => e.MaterialToma)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_material_toma");

                entity.Property(e => e.Materialmedidor)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_materialmedidor");

                entity.Property(e => e.MesAdeudoAct)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("mes_adeudo_act");

                entity.Property(e => e.MesAdeudoAnt)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("mes_adeudo_ant");

                entity.Property(e => e.MesFacturado)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("_MesFacturado");

                entity.Property(e => e.Mf)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("mf");

                entity.Property(e => e.Nivel)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("nivel");

                entity.Property(e => e.Nivelsocial)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_nivelsocial");

                entity.Property(e => e.NomComercial)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("nom_comercial");

                entity.Property(e => e.NomPropietario)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("nom_propietario");

                entity.Property(e => e.NumExt)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("num_ext");

                entity.Property(e => e.NumInt)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("num_int");

                entity.Property(e => e.PaginaInternet)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("pagina_internet");

                entity.Property(e => e.Poblacion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_poblacion");

                entity.Property(e => e.PorDescto)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("por_descto");

                entity.Property(e => e.Prefacturado).HasColumnName("prefacturado");

                entity.Property(e => e.PromedioAct)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("promedio_act");

                entity.Property(e => e.PromedioAnt)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("promedio_ant");

                entity.Property(e => e.RazonSocial)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("razon_social");

                entity.Property(e => e.Recibo)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("recibo");

                entity.Property(e => e.ReciboMail).HasColumnName("recibo_mail");

                entity.Property(e => e.Rfc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("rfc");

                entity.Property(e => e.Ruta)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ruta");

                entity.Property(e => e.SalidasHidraulicas)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("salidas_hidraulicas");

                entity.Property(e => e.Sb)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("sb");

                entity.Property(e => e.Sector)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("sector");

                entity.Property(e => e.Servicio)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("_servicio");

                entity.Property(e => e.Situacion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_situacion");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("money")
                    .HasColumnName("subtotal");

                entity.Property(e => e.Tarifafija)
                    .HasMaxLength(61)
                    .HasColumnName("_tarifafija");

                entity.Property(e => e.Telefono1)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("telefono1");

                entity.Property(e => e.Telefono2)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("telefono2");

                entity.Property(e => e.Telefono3)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("telefono3");

                entity.Property(e => e.TienePozo).HasColumnName("tiene_pozo");

                entity.Property(e => e.Tipodraef)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("_tipodraef");

                entity.Property(e => e.Tipofactible)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_tipofactible");

                entity.Property(e => e.Tipogrupo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_tipogrupo");

                entity.Property(e => e.Tipoinstalacion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_tipoinstalacion");

                entity.Property(e => e.Tipotoma)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_tipotoma");

                entity.Property(e => e.Tipotuberia)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_tipotuberia");

                entity.Property(e => e.Tipousuario)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_tipousuario");

                entity.Property(e => e.Toma)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("toma");

                entity.Property(e => e.Total)
                    .HasColumnType("money")
                    .HasColumnName("total");

                entity.Property(e => e.Ubicacionmedidor)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_ubicacionmedidor");

                entity.Property(e => e.Visitar).HasColumnName("visitar");

                entity.Property(e => e.Viviendas)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("viviendas");

                entity.Property(e => e.Zona)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("_zona");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
