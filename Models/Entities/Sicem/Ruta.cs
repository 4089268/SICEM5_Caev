using System;
using System.Collections.Generic;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Models
{
    public partial class Ruta: IEnlace
    {
        public int Id { get; set; }
        public string Oficina { get; set; }
        public string Ruta1 { get; set; }
        public bool? Inactivo { get; set; }
        public string Servidor { get; set; }
        public string BaseDatos { get; set; }
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public short? IdZona { get; set; }
        public bool? Desconectado { get; set; }
        public bool? Alterno { get; set; }
        public string ServidorA { get; set; }
        public string Alias { get; set; }

        public virtual ICollection<HistTarifa> HistTarifas { get; set; }
        public virtual ICollection<ModsOficina> ModsOficinas { get; set; }

        public string StringConection {
            get {
                var _connectionBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder() {
                    DataSource = Alterno == true?this.ServidorA:this.Servidor,
                    InitialCatalog = this.BaseDatos,
                    UserID = this.Usuario,
                    Password = this.Contraseña,
                    ApplicationName = "SICEM_CAEV",
                    ConnectTimeout = ( 60 * 20)
                };
                return _connectionBuilder.ConnectionString;
            }
        }

        // *** IMPLEMENTACIONES 
        public string Nombre => Oficina;
        public string GetConnectionString() => StringConection;
    }
}
