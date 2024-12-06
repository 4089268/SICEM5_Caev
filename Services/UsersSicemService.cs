using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Data;
using Sicem_Blazor.Models;
using Sicem_Blazor.Models.Entities.Arquos;

namespace Sicem_Blazor.Services
{
    public class UsersSicemService
    {

        public readonly IConfiguration appSettings;
        private readonly SicemContext sicemContext;
        private readonly SessionService sessionService;
        private readonly ILogger<UsersSicemService> logger;

        public IUsuario Usuario { get; set;}
        public string IdSession {get;set;} = "";

        const string secret = "*SICEM*";

        public UsersSicemService(SicemContext context, IConfiguration c, SessionService s, ILogger<UsersSicemService> logger) {
            this.sicemContext = context;
            appSettings = c;
            sessionService = s;
            this.logger = logger;
        }

        public IEnumerable<Usuario> ObtenerListadoUsuarios() {
            return this.sicemContext.Usuarios.ToList();
        }

        public Usuario GenerarUsuarioNuevo( Usuario user, string pass, IEnumerable<int> opciones, IEnumerable<int> oficinas ) {
            var _cadConexion = appSettings.GetConnectionString("SICEM");
            int userID = 0;
            try {
                using(var xConnecton = new SqlConnection(_cadConexion)) {
                    xConnecton.Open();

                    //*** Generar opciones xml
                    var xmlCadOciones = "";
                    if(opciones.Any()){
                        xmlCadOciones = GenerarOpcionesOficinasXml(opciones);
                    }

                    // * prepare the sqlCommand
                    var sqlCommand = new SqlCommand( "[dbo].[Usp_Usuarios]", xConnecton);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Alias", "NUEVO-USUARIO");
                    sqlCommand.Parameters.AddWithValue("@Nombre", user.Nombre.ToUpper());
                    sqlCommand.Parameters.AddWithValue("@Usuario", user.Usuario1);
                    sqlCommand.Parameters.AddWithValue("@Password", pass);
                    sqlCommand.Parameters.AddWithValue("@Secret", secret);
                    sqlCommand.Parameters.AddWithValue("@IsAdmin", user.Administrador ?? false);
                    sqlCommand.Parameters.AddWithValue("@CanConfOfi", user.CfgOfi ?? false);
                    sqlCommand.Parameters.AddWithValue("@Oficinas", string.Join(";", oficinas) );
                    sqlCommand.Parameters.AddWithValue("@OpcionesXml", xmlCadOciones);

                    // * execute the command
                    using(SqlDataReader xReader = sqlCommand.ExecuteReader()) {
                        if(xReader.Read()) {
                            userID = int.Parse(xReader["id"].ToString());
                        }else{
                            throw new Exception("No response from the procedure after make the user.");
                        }
                    }

                    this.logger.LogInformation("New user created with id '{userId}'", userID);

                    // * attempt to retrive the new user
                    sicemContext.ChangeTracker.Clear();
                    return this.sicemContext.Usuarios.FirstOrDefault( u => u.Id == userID);
                }
            } catch(Exception err) {
                this.logger.LogError(err, "Fail at attempting to make a new user: {message}", err.Message);
                return null;
            }
        }

        public Usuario ActualizarUsuario( Usuario user, string pass, IEnumerable<int> opciones, IEnumerable<int> oficinas ) {
            var _cadConexion = appSettings.GetConnectionString("SICEM");
            int userID = 0;
            try {
                using(var xConnecton = new SqlConnection(_cadConexion)) {
                    xConnecton.Open();

                    //*** Generar opciones xml
                    var xmlCadOciones = "";
                    if(opciones.Any()){
                        xmlCadOciones = GenerarOpcionesOficinasXml(opciones);
                    }

                    // * prepare the sqlCommand

                    var sqlCommand = new SqlCommand( "[dbo].[Usp_Usuarios]", xConnecton);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Alias", "ACTUALIZAR-USUARIO");
                    sqlCommand.Parameters.AddWithValue("@IdUsuario", user.Id);
                    sqlCommand.Parameters.AddWithValue("@Nombre", user.Nombre.ToUpper());
                    sqlCommand.Parameters.AddWithValue("@Usuario", user.Usuario1);
                    sqlCommand.Parameters.AddWithValue("@Password", pass);
                    sqlCommand.Parameters.AddWithValue("@Secret", secret);
                    sqlCommand.Parameters.AddWithValue("@IsAdmin", user.Administrador ?? false);
                    sqlCommand.Parameters.AddWithValue("@CanConfOfi", user.CfgOfi ?? false);
                    sqlCommand.Parameters.AddWithValue("@Oficinas", string.Join(";", oficinas) );
                    sqlCommand.Parameters.AddWithValue("@OpcionesXml", xmlCadOciones);
                    
                    // * execute the command
                    using(SqlDataReader xReader = sqlCommand.ExecuteReader()) {
                        if(xReader.Read()) {
                            userID = int.Parse(xReader["id"].ToString());
                        }else{
                            throw new Exception("No response from the procedure after update the user.");
                        }
                    }

                    this.logger.LogInformation("The user id '{userId}' was updated", userID);

                    // * attempt to retrive the new user
                    sicemContext.ChangeTracker.Clear();
                    return this.sicemContext.Usuarios.FirstOrDefault( u => u.Id == userID);
                }
            } catch(Exception err) {
                this.logger.LogError(err, "Fail at attempting to make a new user: {message}", err.Message);
                return null;
            }
        }

        public bool ExisteUsuario(string usuario) {
            int r = 0;
            r = sicemContext.Usuarios.Where(item => item.Usuario1.ToLower() == usuario.ToLower()).Count();
            return (r > 0);
        }

        public void ChangeUserStatus(int userID){
            var usuario = sicemContext.Usuarios.Where(item => item.Id == userID).FirstOrDefault();
            if( usuario == null){
                throw new KeyNotFoundException("User not found");
            }
            usuario.Inactivo = !(usuario.Inactivo??false);
            sicemContext.Usuarios.Update(usuario);
            sicemContext.SaveChanges();

        }

        /// <summary>
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="oficinas"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        private void ModificarCadOficinasUsuario(int userID, IEnumerable<int> oficinas) {

            var cadOficinas = String.Join(";", oficinas);

            var usuario = sicemContext.Usuarios.Where(item => item.Id == userID).FirstOrDefault();
            if( usuario == null){
                throw new KeyNotFoundException("User not found");
            }

            usuario.Oficinas = cadOficinas;
            sicemContext.Usuarios.Update(usuario);
            sicemContext.SaveChanges();
        }


        #region Funciones Estaticas
        public static string GenerarOpcionesOficinasXml(IEnumerable<int> idOficinas){
            var _xmlBuilder = new System.Text.StringBuilder();
            _xmlBuilder.Append("<Opciones>");
            foreach(int idOfi in idOficinas) {
                _xmlBuilder.Append($"<Opcion id=\"{idOfi}\" />");
            }
            _xmlBuilder.Append("</Opciones>");
            return _xmlBuilder.ToString();
        }
        #endregion
    }

}