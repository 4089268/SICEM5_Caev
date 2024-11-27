using System;
using System.Collections.Generic;
using Sicem_Blazor.Tarifas.Models;
using Sicem_Blazor.Models;
using Sicem_Blazor.Data;
using Sicem_Blazor.Tarifas.Data;

namespace Sicem_Blazor.Data {
    public interface ITarifasService {
        
        public IEnumerable<ITarifa> ObtenerTarifasSicem();
        public IEnumerable<CatTipoUsuario> ObtenerCatalogoTiposUsuarios();
        public bool ModificarTarifaSicem(ITarifa tarifa);

        public Tarifa ObtenerTarifaOficina(IEnlace enlace, int idTarifa);
        public bool GenerarHistorialTarifas(IEnlace enlace);
        public IEnumerable<HistTarifa> ObtenerHistorialTarifa(IEnlace enlace, int af, int mf);
        public IEnumerable<HistTarifaItem> ObtenerHistorialTarifaComplete(IEnlace enlace);
        
        public SimulacionResponse SimularFacturacion(IEnlace enlace, string tarifasXML, SimulacionParams param, out string mensaje);

        public bool AlmacenarCustomCatalogoTarifas(StoTarifa stoTarifa, IEnumerable<StoDetTarifa> detTarifas );
        public IEnumerable<StoTarifa> ObtenerCustomCatalogoTarifas();
        public IEnumerable<ITarifa> ObtenerDetalleCustomCatalogoTarifas(int idStoTarifa);


        // Calculo De Tarifas
        public IEnumerable<Dictionary<string,object>> ObtenerCataloTarifasResumido();
        public IEnumerable<CatTarifa> ObtenerCatalogoTarifas(int anio, int mes);
        public FactoresTarifarios ObtenerFactoresTarifarios(int anio, int mes);
        public IEnumerable<ITarifa> GenerarNuevoCatalogoTarifas(int anio, int mes, FactoresTarifarios factores);
        public bool ActualizarCatalogo(int anio, int mes, IEnumerable<ITarifa> catalogo);


        // Carga de Catalogos
        public IEnumerable<CatCfe> ObtenerCatalogoCFE();
        public void AgregarCatalogoCFE(CatCfe cat);
        public void EliminarCatalogoCFE(CatCfe cat);
        public void ActualizarCatalogoCFE(CatCfe cat);
        public IEnumerable<CatUma> ObtenerCatalogoUMA();
        public void AgregarCatalogoUMA(CatUma cat);
        public void EliminarCatalogoUMA(CatUma cat);
        public void ActualizarCatalogoUMA(CatUma cat);
        public IEnumerable<CatInpc> ObtenerCatalogoINPC();
        public void AgregarCatalogoINPC(CatInpc cat);
        public void EliminarCatalogoINPC(CatInpc cat);
        public void ActualizarCatalogoINPC(CatInpc cat);


        /// <summary>
        /// Actualiza los datos de la cabecera de la taria almacenada como el nombre o fecha de modificacion
        /// </summary>
        /// <param name="idStoTarifa">Id de la cebecera a actualizar</param>
        /// <param name="stoTarifa">Datos que se actualizaran</param>
        public void ActualizarCabeceraCustomCatalogoTarifas(int idStoTarifa, StoTarifa stoTarifa);
        
        /// <summary>
        /// Eliminar los datos del catalogo de tarias almacenado
        /// </summary>
        /// <param name="idStorTarifa">Id del catalogo de tarifas a eliminar</param>
        public void EliminarCustomCatalogoTarifas(int idStorTarifa);

        /// <summary>
        /// Actualizar el catalog de tarifas almacenado
        /// </summary>
        /// <param name="idStorTarifad"></param>
        /// <param name="detTarifas"></param>
        public void ActualizarCustomCatalogoTarifas(int idStorTarifad, IEnumerable<StoDetTarifa> detTarifas );

    }

}