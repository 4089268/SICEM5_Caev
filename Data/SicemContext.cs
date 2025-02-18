using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sicem_Blazor.Models
{
    public partial class SicemContext : DbContext
    {
        public SicemContext()
        {
        }

        public SicemContext(DbContextOptions<SicemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BoletinMensaje> BoletinMensajes { get; set; }
        public virtual DbSet<CatAuxiliare> CatAuxiliares { get; set; }
        public virtual DbSet<CatContable> CatContables { get; set; }
        public virtual DbSet<CatContable1> CatContables1 { get; set; }
        public virtual DbSet<CatDispositivo> CatDispositivos { get; set; }
        public virtual DbSet<CatFirma> CatFirmas { get; set; }
        public virtual DbSet<CatImagenesTemplate> CatImagenesTemplates { get; set; }
        public virtual DbSet<CatLocalidade> CatLocalidades { get; set; }
        public virtual DbSet<CatMessagesTemplate> CatMessagesTemplates { get; set; }
        public virtual DbSet<CatOpcione> CatOpciones { get; set; }
        public virtual DbSet<CatSistema> CatSistemas { get; set; }
        public virtual DbSet<CatTipoOficina> CatTipoOficinas { get; set; }
        public virtual DbSet<CatTipoUsuario> CatTipoUsuarios { get; set; }
        public virtual DbSet<CatTiposPoliza> CatTiposPolizas { get; set; }
        public virtual DbSet<CfgConcepto> CfgConceptos { get; set; }
        public virtual DbSet<CfgParametro> CfgParametros { get; set; }
        public virtual DbSet<CfgParametro1> CfgParametros1 { get; set; }
        public virtual DbSet<CfgPoliza> CfgPolizas { get; set; }
        public virtual DbSet<CfgTarifa> CfgTarifas { get; set; }
        public virtual DbSet<Destinatario> Destinatarios { get; set; }
        public virtual DbSet<DetModsOficina> DetModsOficinas { get; set; }
        public virtual DbSet<FacElectronica> FacElectronicas { get; set; }
        public virtual DbSet<HistTarifa> HistTarifas { get; set; }
        public virtual DbSet<Imagene> Imagenes { get; set; }
        public virtual DbSet<LocalizacionRutum> LocalizacionRuta { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<ModsOficina> ModsOficinas { get; set; }
        public virtual DbSet<OprActualizacion> OprActualizacions { get; set; }
        public virtual DbSet<OprBoletin> OprBoletins { get; set; }
        public virtual DbSet<OprDetPoliza> OprDetPolizas { get; set; }
        public virtual DbSet<OprOpcione> OprOpciones { get; set; }
        public virtual DbSet<OprPoliza> OprPolizas { get; set; }
        public virtual DbSet<OprPolizasCerrada> OprPolizasCerradas { get; set; }
        public virtual DbSet<OprSesione> OprSesiones { get; set; }
        public virtual DbSet<Ruta> Rutas { get; set; }
        public virtual DbSet<StoDetTarifa> StoDetTarifas { get; set; }
        public virtual DbSet<StoTarifa> StoTarifas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=ConnectionStrings:SICEM");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoletinMensaje>(entity =>
            {
                entity.ToTable("BoletinMensaje", "Boletin");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.BoletinId).HasColumnName("boletinId");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deletedAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EsArchivo)
                    .HasColumnName("esArchivo")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FileName)
                    .IsUnicode(false)
                    .HasColumnName("fileName");

                entity.Property(e => e.FileSize).HasColumnName("fileSize");

                entity.Property(e => e.Mensaje)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("mensaje");

                entity.Property(e => e.MimmeType)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("mimmeType");

                entity.HasOne(d => d.Boletin)
                    .WithMany(p => p.BoletinMensajes)
                    .HasForeignKey(d => d.BoletinId)
                    .HasConstraintName("FK_BoletinMensaje_Boletin");
            });

            modelBuilder.Entity<CatAuxiliare>(entity =>
            {
                entity.ToTable("Cat_Auxiliares", "Polizas");

                entity.HasIndex(e => e.Auxiliar, "NonClusteredIndex-20181123-122117");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(5, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Auxiliar)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("auxiliar");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.EsOrganismo).HasColumnName("es_organismo");
            });

            modelBuilder.Entity<CatContable>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Cat_Contable");

                entity.Property(e => e.Concepto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("concepto");

                entity.Property(e => e.CtaContable)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("cta_contable")
                    .HasComputedColumnSql("(replace((str([mayor],(4))+str([cuenta],(2)))+str([subcuenta],(2)),' ','0'))", false);

                entity.Property(e => e.Cuenta)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("cuenta");

                entity.Property(e => e.IdContable)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("id_contable")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdCuenta)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_cuenta");

                entity.Property(e => e.Mayor)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("mayor");

                entity.Property(e => e.Subcuenta)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("subcuenta");
            });

            modelBuilder.Entity<CatContable1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Cat_Contable", "Polizas");

                entity.Property(e => e.Concepto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("concepto");

                entity.Property(e => e.CtaContable)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("cta_contable")
                    .HasComputedColumnSql("(replace((str([mayor],(4))+str([cuenta],(2)))+str([subcuenta],(2)),' ','0'))", false);

                entity.Property(e => e.Cuenta)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("cuenta");

                entity.Property(e => e.IdContable)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("id_contable")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdCuenta)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_cuenta");

                entity.Property(e => e.Mayor)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("mayor");

                entity.Property(e => e.Subcuenta)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("subcuenta");
            });

            modelBuilder.Entity<CatDispositivo>(entity =>
            {
                entity.ToTable("cat_dispositivos", "moviles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Caduca)
                    .HasColumnType("date")
                    .HasColumnName("caduca");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.IdUsuario)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_usuario");

                entity.Property(e => e.Imei)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("imei");

                entity.Property(e => e.Inactivo).HasColumnName("inactivo");

                entity.Property(e => e.Oficina).HasColumnName("oficina");
            });

            modelBuilder.Entity<CatFirma>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Cat_Firmas", "Polizas");

                entity.Property(e => e.Activo).HasColumnName("activo");

                entity.Property(e => e.IdOficina).HasColumnName("id_oficina");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Numero)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("numero");

                entity.Property(e => e.Op)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("op");

                entity.Property(e => e.Puesto)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("puesto");
            });

            modelBuilder.Entity<CatImagenesTemplate>(entity =>
            {
                entity.ToTable("Cat_ImagenesTemplate", "Notificacion");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DeletedAt).HasColumnType("smalldatetime");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatLocalidade>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Cat_Localidades", "Sigma");

                entity.Property(e => e.CveLoc)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("cve_loc")
                    .IsFixedLength();

                entity.Property(e => e.CveLocAnt)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("cve_loc_ant")
                    .IsFixedLength();

                entity.Property(e => e.CveOrg)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("cve_org")
                    .IsFixedLength();

                entity.Property(e => e.Enlace)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("enlace");

                entity.Property(e => e.IdOficina).HasColumnName("id_oficina");

                entity.Property(e => e.Manzana)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("manzana");

                entity.Property(e => e.No).HasColumnName("no");

                entity.Property(e => e.NoRural)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("no_rural");

                entity.Property(e => e.Oficina)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("oficina");

                entity.Property(e => e.Programa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("programa");

                entity.Property(e => e.Sb)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("sb");

                entity.Property(e => e.Sector)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("sector");

                entity.Property(e => e.Zona)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("zona");
            });

            modelBuilder.Entity<CatMessagesTemplate>(entity =>
            {
                entity.ToTable("Cat_MessagesTemplate", "Notificacion");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DeletedAt).HasColumnType("smalldatetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("smalldatetime");

                entity.Property(e => e.ImagePropertiesJson).IsUnicode(false);

                entity.Property(e => e.Mensaje)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UltimaModificacion).HasColumnType("smalldatetime");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.CatMessagesTemplates)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_Cat_MessagesTemplate_ImageId");
            });

            modelBuilder.Entity<CatOpcione>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Cat_Opciones");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.IdOpcion)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("id_opcion");
            });

            modelBuilder.Entity<CatSistema>(entity =>
            {
                entity.ToTable("Cat_Sistemas", "Polizas");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(5, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Auxiliar)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("auxiliar");

                entity.Property(e => e.CvePoliza)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("cve_poliza");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.IdTipoOficina).HasColumnName("id_tipoOficina");

                entity.Property(e => e.Oficina).HasColumnName("oficina");

                entity.Property(e => e.Sb)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("sb");

                entity.Property(e => e.Sector)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("sector");
            });

            modelBuilder.Entity<CatTipoOficina>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Cat_TipoOficina", "Polizas");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Valor)
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatTipoUsuario>(entity =>
            {
                entity.ToTable("cat_tipoUsuario", "Fact");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.FactorDrenaje)
                    .HasColumnType("numeric(4, 4)")
                    .HasColumnName("factor_drenaje");

                entity.Property(e => e.FactorTratmto)
                    .HasColumnType("numeric(4, 4)")
                    .HasColumnName("factor_tratmto");

                entity.Property(e => e.ImpAguaFijo)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("imp_agua_fijo");

                entity.Property(e => e.ImpDrenajeFijo)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("imp_drenaje_fijo");

                entity.Property(e => e.ImpSaneamientoFijo)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("imp_saneamiento_fijo");

                entity.Property(e => e.MetrosBase).HasColumnName("metros_base");

                entity.Property(e => e.TasaIva)
                    .HasColumnType("numeric(4, 4)")
                    .HasColumnName("tasa_iva");

                entity.Property(e => e.UltimaModif)
                    .HasColumnType("datetime")
                    .HasColumnName("ultima_modif");
            });

            modelBuilder.Entity<CatTiposPoliza>(entity =>
            {
                entity.ToTable("Cat_TiposPolizas", "Polizas");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(5, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");
            });

            modelBuilder.Entity<CfgConcepto>(entity =>
            {
                entity.HasKey(e => e.IdConcepto)
                    .HasName("PK_Config_Conceptos");

                entity.ToTable("cfg_conceptos", "Config");

                entity.Property(e => e.IdConcepto)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_concepto");

                entity.Property(e => e.ClaveFija).HasColumnName("clave_fija");

                entity.Property(e => e.ClaveProdServ)
                    .HasMaxLength(254)
                    .IsUnicode(false)
                    .HasColumnName("clave_prodServ");

                entity.Property(e => e.ClaveUnidad)
                    .HasMaxLength(254)
                    .IsUnicode(false)
                    .HasColumnName("clave_unidad");

                entity.Property(e => e.CostoEstatico).HasColumnName("costo_estatico");

                entity.Property(e => e.Credito).HasColumnName("credito");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.IdEstatus)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_estatus");

                entity.Property(e => e.IdSystema)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("id_systema");

                entity.Property(e => e.IdTipoConcepto)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_tipoConcepto");

                entity.Property(e => e.IdTrabajo)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_trabajo");

                entity.Property(e => e.Importe)
                    .HasColumnType("money")
                    .HasColumnName("importe");

                entity.Property(e => e.Inactivo).HasColumnName("inactivo");

                entity.Property(e => e.Mostrar).HasColumnName("mostrar");

                entity.Property(e => e.SolicitarCantidad).HasColumnName("solicitar_cantidad");

                entity.Property(e => e.SolicitarMetros).HasColumnName("solicitar_metros");

                entity.Property(e => e.SolicitarNombre).HasColumnName("solicitar_nombre");

                entity.Property(e => e.TipoIva)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("tipo_iva")
                    .IsFixedLength();
            });

            modelBuilder.Entity<CfgParametro>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("cfg_parametros", "Config");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.FechaInsert)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_insert");

                entity.Property(e => e.FechaUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_update");

                entity.Property(e => e.Parametro)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("parametro");

                entity.Property(e => e.PosiblesValores)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("posibles_valores");

                entity.Property(e => e.Valor)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("valor");
            });

            modelBuilder.Entity<CfgParametro1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Cfg_Parametros", "Polizas");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.FechaInsert)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_insert");

                entity.Property(e => e.FechaUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_update");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Parametro)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("parametro");

                entity.Property(e => e.PosiblesValores)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("posibles_valores");

                entity.Property(e => e.Valor)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("valor");
            });

            modelBuilder.Entity<CfgPoliza>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Cfg_Poliza", "Polizas");

                entity.Property(e => e.AuxEsp)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("aux_esp");

                entity.Property(e => e.C1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("c1");

                entity.Property(e => e.C2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("c2");

                entity.Property(e => e.C3)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("c3");

                entity.Property(e => e.C4)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("c4");

                entity.Property(e => e.C5)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("c5");

                entity.Property(e => e.C6)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("c6");

                entity.Property(e => e.IdConcepto)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("id_concepto");

                entity.Property(e => e.IdOficina).HasColumnName("id_oficina");
            });

            modelBuilder.Entity<CfgTarifa>(entity =>
            {
                entity.HasKey(e => e.IdTarifa)
                    .HasName("PK_Config_Tarifas");

                entity.ToTable("cfg_tarifas", "Config");

                entity.Property(e => e.IdTarifa)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_tarifa");

                entity.Property(e => e.AdicionalM3)
                    .HasColumnType("money")
                    .HasColumnName("adicional_m3");

                entity.Property(e => e.Costo)
                    .HasColumnType("money")
                    .HasColumnName("costo");

                entity.Property(e => e.CostoBase)
                    .HasColumnType("money")
                    .HasColumnName("costo_base");

                entity.Property(e => e.CuotaBase)
                    .HasColumnType("money")
                    .HasColumnName("cuota_base");

                entity.Property(e => e.Desde)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("desde");

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
                    .HasColumnName("id_poblacion");

                entity.Property(e => e.IdTipoUsuario)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_TipoUsuario");

                entity.Property(e => e.Inactivo).HasColumnName("inactivo");
            });

            modelBuilder.Entity<Destinatario>(entity =>
            {
                entity.ToTable("Destinatario", "Boletin");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.BoletinId).HasColumnName("boletinId");

                entity.Property(e => e.EnvioMetadata)
                    .IsUnicode(false)
                    .HasColumnName("envioMetadata");

                entity.Property(e => e.Error).HasColumnName("error");

                entity.Property(e => e.FechaEnvio).HasColumnName("fechaEnvio");

                entity.Property(e => e.Lada)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("lada");

                entity.Property(e => e.Resultado)
                    .IsUnicode(false)
                    .HasColumnName("resultado");

                entity.Property(e => e.Telefono).HasColumnName("telefono");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("titulo");

                entity.HasOne(d => d.Boletin)
                    .WithMany(p => p.Destinatarios)
                    .HasForeignKey(d => d.BoletinId)
                    .HasConstraintName("FK_Destinatario_Boletin");
            });

            modelBuilder.Entity<DetModsOficina>(entity =>
            {
                entity.ToTable("Det_ModsOficina");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.IdModif).HasColumnName("id_modif");

                entity.Property(e => e.ValorAnt)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("valor_ant");

                entity.Property(e => e.ValorNuevo)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("valor_nuevo");

                entity.HasOne(d => d.IdModifNavigation)
                    .WithMany(p => p.DetModsOficinas)
                    .HasForeignKey(d => d.IdModif)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModificacionesOficina_ID");
            });

            modelBuilder.Entity<FacElectronica>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Fac_Electronica");

                entity.Property(e => e.Folio)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("folio");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Oficina)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("oficina");
            });

            modelBuilder.Entity<HistTarifa>(entity =>
            {
                entity.ToTable("hist_Tarifas", "Fact");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdicionalM3)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("adicional_m3");

                entity.Property(e => e.Af).HasColumnName("af");

                entity.Property(e => e.Costo)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("costo");

                entity.Property(e => e.CostoBase)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("costo_base");

                entity.Property(e => e.CuotaBase)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("cuota_base");

                entity.Property(e => e.Dese).HasColumnName("dese");

                entity.Property(e => e.Hasta).HasColumnName("hasta");

                entity.Property(e => e.IdEdito)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("id_edito");

                entity.Property(e => e.IdOficina).HasColumnName("id_oficina");

                entity.Property(e => e.IdTarifa).HasColumnName("id_tarifa");

                entity.Property(e => e.IdTipoUsuario).HasColumnName("id_tipoUsuario");

                entity.Property(e => e.Mf).HasColumnName("mf");

                entity.Property(e => e.UltimAct)
                    .HasColumnType("datetime")
                    .HasColumnName("ultim_act");

                entity.HasOne(d => d.IdOficinaNavigation)
                    .WithMany(p => p.HistTarifas)
                    .HasForeignKey(d => d.IdOficina)
                    .HasConstraintName("FK__hist_Tari__id_of__7C6F7215");

                entity.HasOne(d => d.IdTipoUsuarioNavigation)
                    .WithMany(p => p.HistTarifas)
                    .HasForeignKey(d => d.IdTipoUsuario)
                    .HasConstraintName("FK__hist_Tari__id_ti__7B7B4DDC");
            });

            modelBuilder.Entity<Imagene>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Fecha)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("fecha");

                entity.Property(e => e.FechaInsercion)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_insercion")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Imagen)
                    .HasColumnType("image")
                    .HasColumnName("imagen");

                entity.Property(e => e.Oficina).HasColumnName("oficina");

                entity.Property(e => e.Opcion)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("opcion");
            });

            modelBuilder.Entity<LocalizacionRutum>(entity =>
            {
                entity.ToTable("LocalizacionRuta", "Global");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Latitud)
                    .HasColumnType("numeric(18, 15)")
                    .HasColumnName("latitud");

                entity.Property(e => e.Longitud)
                    .HasColumnType("numeric(18, 15)")
                    .HasColumnName("longitud");

                entity.Property(e => e.RutaId).HasColumnName("rutaId");

                entity.HasOne(d => d.Ruta)
                    .WithMany(p => p.LocalizacionRuta)
                    .HasForeignKey(d => d.RutaId)
                    .HasConstraintName("FK_LocalizacionRuta");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("locations", "moviles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Accuracy)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("accuracy");

                entity.Property(e => e.Altitude)
                    .HasColumnType("numeric(14, 6)")
                    .HasColumnName("altitude");

                entity.Property(e => e.Aplicado).HasColumnName("aplicado");

                entity.Property(e => e.Bearing)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("bearing");

                entity.Property(e => e.Dateadded)
                    .HasColumnType("datetime")
                    .HasColumnName("dateadded")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FechaInsert)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_insert");

                entity.Property(e => e.FechaUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_update");

                entity.Property(e => e.IdMovil)
                    .HasColumnType("numeric(12, 0)")
                    .HasColumnName("id_movil");

                entity.Property(e => e.Imei)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("imei");

                entity.Property(e => e.Inactivo).HasColumnName("inactivo");

                entity.Property(e => e.Latitude)
                    .HasColumnType("numeric(14, 6)")
                    .HasColumnName("latitude");

                entity.Property(e => e.Longitude)
                    .HasColumnType("numeric(14, 6)")
                    .HasColumnName("longitude");

                entity.Property(e => e.Provider)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("provider");

                entity.Property(e => e.Speed)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("speed");
            });

            modelBuilder.Entity<ModsOficina>(entity =>
            {
                entity.ToTable("ModsOficina");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.IdOficina).HasColumnName("id_oficina");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Tabla)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("tabla");

                entity.HasOne(d => d.IdOficinaNavigation)
                    .WithMany(p => p.ModsOficinas)
                    .HasForeignKey(d => d.IdOficina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Oficina_ID");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.ModsOficinas)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_ID");
            });

            modelBuilder.Entity<OprActualizacion>(entity =>
            {
                entity.ToTable("Opr_Actualizacion", "Ubitoma");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AnomaliaPredio)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("anomaliaPredio");

                entity.Property(e => e.Estatus)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("estatus");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.Giro)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("giro");

                entity.Property(e => e.IdActualizacionAnt).HasColumnName("id_actualizacionAnt");

                entity.Property(e => e.IdAnomaliaPredio).HasColumnName("id_anomaliaPredio");

                entity.Property(e => e.IdCuenta).HasColumnName("id_cuenta");

                entity.Property(e => e.IdEstatus).HasColumnName("id_estatus");

                entity.Property(e => e.IdGiro).HasColumnName("id_giro");

                entity.Property(e => e.IdOficina).HasColumnName("id_oficina");

                entity.Property(e => e.IdPadron).HasColumnName("id_padron");

                entity.Property(e => e.IdSituacion).HasColumnName("id_situacion");

                entity.Property(e => e.IdTarifa).HasColumnName("id_tarifa");

                entity.Property(e => e.Latitud)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("latitud");

                entity.Property(e => e.Longitud)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("longitud");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("observaciones");

                entity.Property(e => e.Situacion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("situacion");

                entity.Property(e => e.Tarifa)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("tarifa");
            });

            modelBuilder.Entity<OprBoletin>(entity =>
            {
                entity.ToTable("OprBoletin", "Boletin");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("titulo");
            });

            modelBuilder.Entity<OprDetPoliza>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Opr_DetPolizas", "Polizas");

                entity.Property(e => e.Abonos)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("abonos")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Auxiliar)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("auxiliar");

                entity.Property(e => e.Cargos)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("cargos")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Concepto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("concepto");

                entity.Property(e => e.IdAuxiliar)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("id_auxiliar");

                entity.Property(e => e.IdCuenta)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_cuenta")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IdPoliza)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("id_poliza");

                entity.Property(e => e.Recargado).HasColumnName("recargado");
            });

            modelBuilder.Entity<OprOpcione>(entity =>
            {
                entity.ToTable("opr_opciones");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdOpcion).HasColumnName("id_opcion");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            });

            modelBuilder.Entity<OprPoliza>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Opr_Polizas", "Polizas");

                entity.Property(e => e.Año)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("año");

                entity.Property(e => e.Cheque)
                    .HasMaxLength(18)
                    .IsUnicode(false)
                    .HasColumnName("cheque");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FechaInsert)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_insert")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Firma1)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("firma1");

                entity.Property(e => e.IdGenero)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("id_genero")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdOficina).HasColumnName("id_oficina");

                entity.Property(e => e.IdPoliza)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_poliza");

                entity.Property(e => e.IdSistema).HasColumnName("id_Sistema");

                entity.Property(e => e.InActivo).HasColumnName("inActivo");

                entity.Property(e => e.Mes)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("mes");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("observacion")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Poliza)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("poliza");

                entity.Property(e => e.Tipo)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("tipo");
            });

            modelBuilder.Entity<OprPolizasCerrada>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Opr_PolizasCerradas", "Polizas");

                entity.Property(e => e.Año)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("año");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.IdOficina).HasColumnName("id_oficina");

                entity.Property(e => e.Mes)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("mes");
            });

            modelBuilder.Entity<OprSesione>(entity =>
            {
                entity.ToTable("Opr_Sesiones", "Global");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Caducidad)
                    .HasColumnType("datetime")
                    .HasColumnName("caducidad");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Inicio)
                    .HasColumnType("datetime")
                    .HasColumnName("inicio");

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ip_address");

                entity.Property(e => e.MacAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("mac_address");
            });

            modelBuilder.Entity<Ruta>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Alias)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("alias");

                entity.Property(e => e.Alterno).HasColumnName("alterno");

                entity.Property(e => e.BaseDatos)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Base_Datos");

                entity.Property(e => e.Contraseña)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Desconectado)
                    .HasColumnName("desconectado")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IdZona).HasColumnName("id_zona");

                entity.Property(e => e.Inactivo).HasDefaultValueSql("((0))");

                entity.Property(e => e.Oficina)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("oficina");

                entity.Property(e => e.Ruta1)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ruta");

                entity.Property(e => e.Servidor)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServidorA)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Servidor_A");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StoDetTarifa>(entity =>
            {
                entity.ToTable("sto_detTarifa", "Fact");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdicionalM3)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("adicional_m3");

                entity.Property(e => e.Costo)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("costo");

                entity.Property(e => e.CostoBase)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("costo_base");

                entity.Property(e => e.CuotaBase)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("cuota_base");

                entity.Property(e => e.Desde).HasColumnName("desde");

                entity.Property(e => e.Hasta).HasColumnName("hasta");

                entity.Property(e => e.IdStoTarifa).HasColumnName("id_stoTarifa");

                entity.Property(e => e.IdTarifa).HasColumnName("id_tarifa");

                entity.Property(e => e.IdTipoUsuario).HasColumnName("id_tipoUsuario");

                entity.HasOne(d => d.IdStoTarifaNavigation)
                    .WithMany(p => p.StoDetTarifas)
                    .HasForeignKey(d => d.IdStoTarifa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_stoTarifaID");
            });

            modelBuilder.Entity<StoTarifa>(entity =>
            {
                entity.ToTable("sto_tarifa", "Fact");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creacion");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_modificacion");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.Usuario1, "IX_Usuarios")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Administrador)
                    .HasColumnName("administrador")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CfgOfi)
                    .HasColumnName("cfg_ofi")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CfgOpc)
                    .HasColumnName("cfg_opc")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Contraseña)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("contraseña");

                entity.Property(e => e.Inactivo)
                    .HasColumnName("inactivo")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Oficinas)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("oficinas");

                entity.Property(e => e.Opciones)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("opciones");

                entity.Property(e => e.UltimaModif).HasColumnType("datetime");

                entity.Property(e => e.Usuario1)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("usuario");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
