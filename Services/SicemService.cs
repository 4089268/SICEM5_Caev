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
using ServiceReference;

namespace Sicem_Blazor.Services {
    public class SicemService {

        public readonly IConfiguration appSettings;
        private readonly SicemContext sicemContext;
        private readonly SessionService sessionService;
        private readonly ILogger<SicemService> logger;

        public IUsuario Usuario { get; set;}
        public string IdSession {get;set;} = "";

        const string secret = "*SICEM*";

        public SicemService(SicemContext context, IConfiguration c, SessionService s, ILogger<SicemService> logger) {
            this.sicemContext = context;
            appSettings = c;
            sessionService = s;
            this.logger = logger;
        }


        // ****** Funciones Sesiones ******
        public string Logearse(string usuario, string pass, string ipAddress = "") {
            var _cadConexion = appSettings.GetConnectionString("SICEM");

            try {
                
                //*** Validar usando base de datos SICEM
                var _usuario = new Usuario();
                using(var xConnecton = new SqlConnection(_cadConexion)) {
                    xConnecton.Open();
                    var xCommand = new SqlCommand();
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"Exec [api].[logearse] @alias = 'LOGEARSE', @user = '{usuario}', @password = '{pass}', @secret = '{secret}' "; // @ipAddress = '{ipAddress}';
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        if(xReader.Read()) {
                            _usuario.Id = int.Parse(xReader["id"].ToString());
                        }else{
                            _usuario.Id = -1;
                        }
                    }
                    xConnecton.Close();
                }

                //*** Validar usando datos oficina
                var _usuarioExt = new UsuarioExterno();
                if(_usuario.Id < 0){
                    var _enlace = ObtenerEnlaces().Where( item => item.Oficina.ToLower() == usuario.ToLower() ).FirstOrDefault();
                    if(_enlace != null){
                        Console.WriteLine(">>"+_enlace.GetConnectionString());
                        using( var sqlConnection = new SqlConnection(_enlace.GetConnectionString())){
                            sqlConnection.Open();
                            var _query = $"Select id_usuario, nombre, usuario From [Global].[Sys_Usuarios] Where IsNull(inactivo,0) = 0 and id_jerarquia = 1 and usuario = '{pass}'";
                            Console.WriteLine($"oficina: {_enlace.Nombre} query:"+_query);
                            var _command = new SqlCommand(_query, sqlConnection);
                            using( var reader = _command.ExecuteReader()){
                                if(reader.Read()){
                                    _usuarioExt.Id = reader["id_usuario"].ToString();
                                    _usuarioExt.Nombre = reader["nombre"].ToString();
                                    _usuarioExt.Usuario = reader["usuario"].ToString();
                                    _usuarioExt.SetEnlaces( new IEnlace[]{_enlace});
                                }else{
                                    return "El usuario y/o contraseña son incorrectas,";
                                }
                            }
                            sqlConnection.Close();
                        }
                    }else{
                        return "El usuario y/o contraseña son incorrectas";
                    }
                }


                //*** Cargar usuario sicem
                if(_usuario.Id >= 0 ){
                    _usuario = sicemContext.Usuarios.Where(item => item.Id == _usuario.Id).FirstOrDefault();
                    if(_usuario != null){
                        //**** Cargar Oficinas del usuario
                        var idEnlaces = _usuario.Oficinas.Split(";").Select( item => ConvertUtils.ParseInteger(item,-1)).ToList<int>();
                        var _tmpEnlaces = ObtenerEnlaces().Where(item => idEnlaces.Contains(item.Id)).ToList();
                        _usuario.SetEnlaces( _tmpEnlaces);

                        //**** Cargar Opciones disponibles
                        var _catOpciones = ObtenerListaOpcionesDelUsuario(_usuario.Id).ToList<IOpcionSistema>();
                        _usuario.SetOpciones(_catOpciones);
                        

                        this.Usuario = _usuario;
                        
                    }else{
                        return "El usuario y/o contraseña son incorrectas.";
                    }
                }else{
                    this.Usuario = _usuarioExt;
                }
                this.IdSession = sessionService.IniciarSesion(Usuario, ipAddress);
                return null;
                
            }catch(Exception err) {
                Console.WriteLine($">> Error al logearse: \n\t{err.Message}\n{err.StackTrace}");
                return "Error al conectarse con el servidor, inténtelo mas tarde.";
            }


        }
        public bool CargarUsuarioToken(string token){
            IdSession = token;
            var _tmpUsuario = sessionService.ObtenerUsuario(token);
            
            if( _tmpUsuario == null){
                CerrarSesion();
                return false;
            }

            this.Usuario = _tmpUsuario;
            this.IdSession = token;
            return true;
        }
        public void CerrarSesion(){
            this.Usuario = null;
            sessionService.FinalizarSesion(IdSession);
        }


        // ****** Funciones Usuario ******
        public IEnumerable<IEnlace> ObtenerOficinasDelUsuario(int idUsuario = 0) {
            if(idUsuario == 0) {
                return Usuario.Enlaces.ToArray();
            }else{
                var _usuario = sicemContext.Usuarios.Where(item => item.Id == idUsuario).FirstOrDefault();
                if(_usuario == null){
                    return new IEnlace[]{};
                }else{
                    var idEnlaces = _usuario.Oficinas.Split(";").Select( item => ConvertUtils.ParseInteger(item,-1)).ToList<int>();
                    return ObtenerEnlaces().Where(item => idEnlaces.Contains(item.Id)).ToList();
                }
            }
        }
        public async Task<bool> GuardarCambiosUsuario() {
            int res = 0;
            var _idUsuario = ConvertUtils.ParseInteger(this.Usuario.Id);
            try
            {
                var _usuario = sicemContext.Usuarios.Where( item => item.Id == _idUsuario).FirstOrDefault() ?? throw new KeyNotFoundException("The user is not found on the system");

                var newCadOffices = this.Usuario.GetCadEnlaces();
                _usuario.Oficinas = newCadOffices;
                res = await sicemContext.SaveChangesAsync();
                this.logger.LogInformation("The user {user} has changed the cad of offices to [{newCadOffices}]", this.Usuario, newCadOffices );
                return res > 0;
            }catch(Exception err)
            {
                this.logger.LogError(err, "Fail at attempt to update the offices of the user: {message}", err.Message);
                return false;
            }
        }
        public List<CatOpcione> ObtenerListaOpcionesDelUsuario(int idUsuario = 0) {
            var _listaOpciones = new List<CatOpcione>();
            var _idsOpciones = new List<int>();
            if(idUsuario > 0)
            {
                _idsOpciones = sicemContext.OprOpciones.Where(item => item.IdUsuario == idUsuario).Select(item => item.IdOpcion).ToList<int>();
            }
            else
            {
                var _idUsuario = ConvertUtils.ParseInteger(Usuario.Id);
                _idsOpciones = sicemContext.OprOpciones.Where(item => item.IdUsuario == _idUsuario).Select(item => item.IdOpcion).ToList<int>();
            }
            _listaOpciones = sicemContext.CatOpciones.Where(item => _idsOpciones.Contains((int)item.IdOpcion)).ToList();
            return _listaOpciones;
        }
        public List<CatOpcione> ObtenerCatalogoOpciones() {
            List<CatOpcione> _result = new List<CatOpcione>();
            _result = sicemContext.CatOpciones.ToList();
            return _result;
        }
        public Usuario ObtenerUsuarioToken(string token, string ipAddress){
            throw new NotImplementedException();
            // try{
            //     Usuario tmpUsuario = null;
            //     var session = sicemContext.OprSesiones.Where(item => item.Id.ToString() == token && item.IpAddress == ipAddress && item.Caducidad > DateTime.Now ).FirstOrDefault();
            //     if(session != null){
            //         tmpUsuario = sicemContext.Usuarios.Where(item => item.Id == session.IdUsuario).FirstOrDefault();
            //     }
            //     return tmpUsuario;
            // }catch(Exception err){
            //     Console.WriteLine($">>> Error al obtener el usuario por token \n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
            //     return null;
            // }
        }
        public void CerrarSesionDb(string token, string ipAddress){
            throw new NotImplementedException();
            // try{
            //     var session = sicemContext.OprSesiones.Where(item => item.Id.ToString() == token && item.IpAddress == ipAddress ).FirstOrDefault();
            //     session.Caducidad = DateTime.Now.AddMinutes(-1);
            //     sicemContext.OprSesiones.Update(session);
            //     sicemContext.SaveChanges();
            // }catch(Exception err){
            //     Console.WriteLine($">>> Error al tratar de cerrar la sesion del usuario token \n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");                
            // }
        }


        //****** Midelware ******
        public bool ComprobarOficinaUsuario(int idOficina, out IEnlace ruta){
            var listaRutas = ObtenerOficinasDelUsuario();
            if(listaRutas.Count() <= 0) {
                ruta = null;
                return false;
            }
            if(listaRutas.Select(item => item.Id).ToArray().Contains(idOficina)){
                ruta = listaRutas.Where(item => item.Id == idOficina).FirstOrDefault();
                return true;
            }
            else {
                ruta = null;
                return false;
            }
        }


        // ****** Funciones Generales ******
        public Ruta[] ObtenerEnlaces(int id_oficina = 0) {
            Ruta[] rutas = null;
            try {
                
                rutas = sicemContext.Rutas.ToArray();
                if(id_oficina > 0) {
                    rutas = rutas.Where(item => item.Id == id_oficina).ToArray();
                }
            } catch(Exception err) {
                logger.LogError(0, err, "Error al obtener enlace  de la oficina:{Oficina}", id_oficina);
                return new Ruta[]{};
            }
            return rutas;
        }
        public ICollection<Sicem_Colonia> ObtenerCatalogoColonias(int id_oficina) {
            var oficina = ObtenerEnlaces(id_oficina).FirstOrDefault();
            if(oficina == null){
                return null;
            }
            var _result = new List<Sicem_Colonia>();
            using( var context = new ArquosContext(oficina.StringConection)){
                _result = context.CatColonias.Select( item => Sicem_Colonia.FromEntity(item) ).ToList();
            }
            return _result;
        }
        public Sicem_Sucursal[] ObtenerCatalogoSucursales(int id_oficina) {
            var tmpList = new List<Sicem_Sucursal>();
            var xEnlace = ObtenerEnlaces(id_oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                var xCommand = new SqlCommand();
                xCommand.Connection = xConnecton;
                xCommand.CommandText = string.Format("Select id_sucursal, descripcion, sb, inactivo From [Global].[Cat_Sucursales]");
                using(var xReader = xCommand.ExecuteReader()) {
                    while(xReader.Read()) {
                        int tmpInt = 0;
                        var newItem = new Sicem_Sucursal {
                            Id_Sucursal = int.TryParse(xReader["id_sucursal"].ToString(), out tmpInt) ? tmpInt : 0,
                            Descripcion = xReader["descripcion"].ToString(),
                            Sb = int.TryParse(xReader["sb"].ToString(), out tmpInt) ? tmpInt : 0,
                            Inactivo = int.TryParse(xReader["inactivo"].ToString(), out tmpInt) ? tmpInt : 0
                        };
                        tmpList.Add(newItem);
                    }
                }
            }
            return tmpList.ToArray();
        }   
        public Sicem_Localidad[] ObtenerCatalogoLocalidades(int id_oficina){
            var tmpList = new List<Sicem_Localidad>();
            var _query = "Select id_poblacion, descripcion, id_sucursal, sb, sectores, inactivo From[Padron].[Cat_Poblaciones]";
            var _ruta = ObtenerEnlaces(id_oficina).FirstOrDefault();
            if(_ruta != null) {
                using(var xConeccion = new SqlConnection(_ruta.StringConection)){
                    xConeccion.Open();
                    using(var xCommand = new SqlCommand(_query, xConeccion)){
                        using(SqlDataReader xReader = xCommand.ExecuteReader()){
                            while(xReader.Read()){
                                int tmpId = 0;
                                var newItem = new Sicem_Localidad() {
                                    Id_Poblacion = int.TryParse(xReader["id_poblacion"].ToString(), out tmpId)?tmpId:0,
                                    Descripcion = xReader["descripcion"].ToString(),
                                    Id_sucursal = int.TryParse(xReader["id_sucursal"].ToString(), out tmpId) ? tmpId : 0,
                                    Sb = int.TryParse(xReader["sb"].ToString(), out tmpId) ? tmpId : 0,
                                    Sector =  int.TryParse(xReader["sectores"].ToString(), out tmpId) ? tmpId : 0,
                                    Inactivo = int.TryParse(xReader["inactivo"].ToString(), out tmpId) ? tmpId : 0,
                                };
                                tmpList.Add(newItem);
                            }
                        }
                    }
                    xConeccion.Close();
                }
            }
            return tmpList.ToArray();
        }
        public DatosUsuario ObtenerDatosUsuarios(int id_oficina,int id_padron) {
            var _response = new DatosUsuario();

            var _query = "Select id_padron, id_cuenta, razon_social, rfc, direccion, colonia, ciudad, estado, telefono1, _giro as giro,  _estatus as estatus, " +
                $" _tarifafija as tarifa, _servicio as servicio, id_medidor as medidor, mes_adeudo_act as meses_adeudo From[Padron].[vw_cat_padron] where id_padron = {id_padron}";

            var _enlace = ObtenerEnlaces(id_oficina).FirstOrDefault();
            using(var conexion = new SqlConnection(_enlace.StringConection)) {
                conexion.Open();
                using(var command = new SqlCommand(_query, conexion)){
                    using(SqlDataReader reader = command.ExecuteReader()) {
                        if(reader.Read()) {
                            _response.Id_Padron = int.TryParse(reader["id_padron"].ToString(), out int tmpP) ? tmpP : 0;
                            _response.Id_Cuenta = int.TryParse(reader["id_cuenta"].ToString(), out int tmpC) ? tmpC : 0;
                            _response.Razon_Social = reader["razon_social"].ToString();
                            _response.RFC = reader["rfc"].ToString();
                            _response.Direccion = reader["direccion"].ToString();
                            _response.Colonia = reader["colonia"].ToString();
                            _response.Ciudad = reader["ciudad"].ToString();
                            _response.Estado = reader["estado"].ToString();
                            _response.Telefono1 = reader["telefono1"].ToString();
                            _response.Giro = reader["giro"].ToString();
                            _response.Estatus = reader["estatus"].ToString();
                            _response.Tarifa = reader["tarifa"].ToString();
                            _response.Servicios = reader["servicio"].ToString();
                            _response.Medidor = reader["medidor"].ToString();
                            _response.Meses_Adeudo = int.TryParse(reader["meses_adeudo"].ToString(), out int tmpM) ? tmpM : 0;
                        }
                    }
                }
                conexion.Close();
            }
            return _response;
        }
        public string ObtenerDireccionApi(){
            return appSettings.GetValue<string>("AppSettings:Direccion_Api");
        }
        
        public Dictionary<int, string> CatalogoTarifas(int id_oficina) {
            var tmpCatalogo = new Dictionary<int, string>();
            var xEnlace = ObtenerEnlaces(id_oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                var xCommand = new SqlCommand();
                xCommand.Connection = xConnecton;
                xCommand.CommandText = string.Format("Select id_tipousuario as id, descripcion From Padron.Cat_TiposUsuario Order By descripcion");
                using(var xReader = xCommand.ExecuteReader()) {
                    while(xReader.Read()) {
                        try {
                            tmpCatalogo.Add(int.Parse(xReader["id"].ToString()), xReader["descripcion"].ToString());
                        }
                        catch(Exception) { }
                    }
                }
            }
            return tmpCatalogo;
        }
        public Dictionary<int,string> ObtenerCatalogoEstatus(int id_oficina, string filtro = ""){
            var result = new Dictionary<int,string>();

            var oficina = ObtenerEnlaces(id_oficina).FirstOrDefault();
            if(oficina == null){
                return null;
            }
            var _datos = new List<Sicem_Blazor.Models.Entities.Arquos.CatEstatus>();
            try{
                using(var context = new ArquosContext(oficina.StringConection)){
                    if(filtro.Trim().Length > 1){
                        _datos = context.CatEstatuses.Where(item => item.IdEstatus == 0 || item.Tabla == filtro).ToList();
                    }else{
                        _datos = context.CatEstatuses.ToList();
                    }
                }
                _datos.ForEach(item => result.Add((int)item.IdEstatus, item.Descripcion.ToUpper() ));
                return result;
            }catch(Exception err){
                Console.WriteLine($">> Error al obtener el catalogo de esatus de la oficina {oficina.Oficina}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
                return null;
            }
        }
        public Dictionary<int,string> ObtenerCatalogoTipoCalculo(int id_oficina){
            var result = new Dictionary<int,string>();

            var oficina = ObtenerEnlaces(id_oficina).FirstOrDefault();
            if(oficina == null){
                return null;
            }
            var _datos = new List<CatTiposCalculo>();
            try{
                using(var context = new ArquosContext(oficina.StringConection)){
                    _datos = context.CatTiposCalculos.ToList();
                }
                _datos.ForEach(item => result.Add((int)item.IdCalculo, item.Descripcion.ToUpper() ));
                return result;
            }catch(Exception err){
                Console.WriteLine($">> Error al obtener el catalogo tipoCalculo {oficina.Oficina}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
                return null;
            }
        }
        public Dictionary<int,string> ObtenerCatalogoServicios(int id_oficina){
            var result = new Dictionary<int,string>();

            var oficina = ObtenerEnlaces(id_oficina).FirstOrDefault();
            if(oficina == null){
                return null;
            }
            var _datos = new List<CatServicio>();
            try{
                using(var context = new ArquosContext(oficina.StringConection)){
                    _datos = context.CatServicios.ToList();
                }
                _datos.ForEach(item => result.Add((int)item.IdServicio, item.Descripcion.ToUpper() ));
                return result;
            }catch(Exception err){
                Console.WriteLine($">> Error al obtener el catalogo CatServicio de la oficina {oficina.Oficina}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
                return null;
            }
        }
        public Dictionary<int,string> ObtenerCatalogoTarifas(int id_oficina){
            var result = new Dictionary<int,string>();

            var oficina = ObtenerEnlaces(id_oficina).FirstOrDefault();
            if(oficina == null){
                return null;
            }
            var _datos = new List<CatTiposUsuario>();
            try{
                using(var context = new ArquosContext(oficina.StringConection)){
                    _datos = context.CatTiposUsuarios.ToList();
                }
                _datos.ForEach(item => result.Add((int)item.IdTipousuario, item.Descripcion.ToUpper() ));
                return result;
            }catch(Exception err){
                Console.WriteLine($">> Error al obtener el catalogo CatTiposUsuario de la oficina {oficina.Oficina}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
                return null;
            }
        }
        public Dictionary<int,string> ObtenerCatalogoAnomalias(int id_oficina){
            var result = new Dictionary<int,string>();

            var oficina = ObtenerEnlaces(id_oficina).FirstOrDefault();
            if(oficina == null){
                return null;
            }
            var _datos = new List<CatAnomalias>();
            try{
                using(var context = new ArquosContext(oficina.StringConection)){
                    _datos = context.CatAnomaliases.ToList();
                }
                _datos.ForEach(item => result.Add((int)item.IdAnomalia, item.Descripcion.ToUpper() ));
                return result;
            }catch(Exception err){
                Console.WriteLine($">> Error al obtener el catalogo CatAnomalias de la oficina {oficina.Oficina}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
                return null;
            }
        }
        public Dictionary<int,string> ObtenerCatalogoGiros(int id_oficina){
            var result = new Dictionary<int,string>();

            var oficina = ObtenerEnlaces(id_oficina).FirstOrDefault();
            if(oficina == null){
                return null;
            }
            var _datos = new List<CatGiro>();
            try{
                using(var context = new ArquosContext(oficina.StringConection)){
                    _datos = context.CatGiros.ToList();
                }
                _datos.ForEach(item => result.Add((int)item.IdGiro, item.Descripcion.ToUpper() ));
                return result;
            }catch(Exception err){
                Console.WriteLine($">> Error al obtener el catalogo CatGiro de la oficina {oficina.Oficina}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
                return null;
            }
        }
        public Dictionary<int,string> ObtenerCatalogoClaseUsuario(int id_oficina){
            var result = new Dictionary<int,string>();

            var oficina = ObtenerEnlaces(id_oficina).FirstOrDefault();
            if(oficina == null){
                return null;
            }
            var _datos = new List<CatClasesUsuario>();
            try{
                using(var context = new ArquosContext(oficina.StringConection)){
                    _datos = context.CatClasesUsuarios.ToList();
                }
                _datos.ForEach(item => result.Add((int)item.IdClaseUsuario, item.Descripcion.ToUpper() ));
                return result;
            }catch(Exception err){
                Console.WriteLine($">> Error al obtener el catalogo CatClasesUsuario de la oficina {oficina.Oficina}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
                return null;
            }
        }
        
        public Dictionary<int,string> CatalogoEstatusPadron(){
            var _result = new Dictionary<int, string>();
            // _result.Add(0, "SIN ESPECIFICAR");
            _result.Add(1, "ACTIVO");
            _result.Add(2, "SALA DE ESPERA");
            _result.Add(4, "BAJA DEFINITIVA");
            _result.Add(51, "CONGELADO");
            return _result;
        }

        //****** Historial Modificaciones *******//
        public List<HistorialModificacion> ObtenerHistorialModificaciones(DateTime fecha1, DateTime fecha2 ){
            throw new NotImplementedException();
            // var _result = new List<HistorialModificacion>();
            // try{
            //     var _datosHist = sicemContext.ModsOficinas.Where( item => item.Fecha > fecha1 && item.Fecha < fecha2 )
            //         .Select( item => HistorialModificacion.FromEntity(item)).ToList<HistorialModificacion>();
            //     _datosHist.ForEach(hist => {
            //         hist.Oficina = sicemContext.Rutas.Find(hist.Id_Oficina).Oficina;
            //         hist.Detalle = sicemContext.DetModsOficinas.Where(item => item.IdModif == hist.Id ).Select( item => HistorialModificacionDetalle.FromEntity(item)).ToList();
            //     });
            //     return _datosHist;
            // }catch(Exception err){
            //     Console.WriteLine($">>Error al obtener el historial de modificaciones \n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
            //     return null;
            // }
        }


        //****** Historial Sesiones ******//
        public List<SesionInfo> ObtenerHistorialSessiones(DateTime fecha1, DateTime fecha2){
           throw new NotImplementedException();
           
            // var _result = new List<SesionInfo>();

            // _result = sicemContext.OprSesiones.Where(item => (item.Inicio > fecha1 && item.Inicio < fecha2) || (item.Caducidad > fecha1 && item.Caducidad < fecha2) )
            //     .OrderByDescending(item => item.Inicio)
            //     .Select( item => SesionInfo.FromEntity(item))
            //     .ToList();
                
            // _result.ForEach( sesion => {
            //     var _usu = sicemContext.Usuarios.FirstOrDefault(item => item.Id == sesion.IdUsuario );
            //     sesion.Usuario = _usu == null?"":_usu.Nombre;
            // });

            // return _result;

        }

        //********** Funciones Estaticas *********
        public static string GenerarOpcionesOficinasXml(int[] idOficinas){
            var _xmlBuilder = new System.Text.StringBuilder();
            _xmlBuilder.Append("<Opciones>");
            foreach(int idOfi in idOficinas) {
                _xmlBuilder.Append($"<Opcion id=\"{idOfi}\" />");
            }
            _xmlBuilder.Append("</Opciones>");
            return _xmlBuilder.ToString();
        }
        

        public bool CheckOfficeConnected(IEnlace enlace)
        {
            var connected = false;
            try {
                var task = Task.Run(() => {
                    using(var sqlConnection = new SqlConnection(enlace.GetConnectionString()))
                    {
                        sqlConnection.Open();
                        var command = new SqlCommand("SELECT VALOR from [Global].[Cfg_Parametros] where parametro='REDONDEAR'", sqlConnection);
                        command.CommandTimeout = TimeSpan.FromSeconds(8).Seconds;
                        command.CommandType = System.Data.CommandType.Text;
                        var result = command.ExecuteScalar();
                        if(result != null)
                        {
                            connected = true;
                        }
                        sqlConnection.Close();
                    }
                });
                if (!task.Wait(TimeSpan.FromSeconds(6)))
                {
                    this.logger.LogInformation("Timeout al verificar la conexion con la oficina {Nombre}", enlace.Nombre);
                }
            }
            catch(Exception err)
            {
                this.logger.LogInformation("Error al verificar la conexion con la oficina {Nombre}: {Message}", enlace.Nombre, err.Message);
            }
            return connected;
        }
    
    }

}