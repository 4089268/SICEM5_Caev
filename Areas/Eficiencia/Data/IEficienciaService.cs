using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Sicem_Blazor.Data;
using Sicem_Blazor.Eficiencia.Models;
using Sicem_Blazor.Models;
using Sicem_Blazor.Services;

namespace Sicem_Blazor.Eficiencia.Data {
    public interface IEficienciaService{
        
        public EficienciaResumen ObtenerResumenEnlace(IEnlace enlace, int anio, int mes, int sb, int sec);
        public IEnumerable<EficienciaResumen> ObtenerResumenAnual(IEnlace enlace, int anio, int sb, int sec);

        public dynamic[] ObtenerEficienciaPorOficinas(Ruta[] oficinas, DateTime fecha1, DateTime fecha2, bool agregarTotal = true);
        public Eficiencia_Sectores_Resp ObtenerEficienciaSectores(int Id_Oficina, int Ano, int Mes, int Sb);
        public Eficiencia_Colonia[] ObtenerEficienciaColonias(int Id_Oficina, int Ano, int Mes, int Sb, int Sect, int Tipo);
        public Eficiencia_Conceptos[] ObtenerEficienciaConceptos( int Id_Oficina, int Ano, int Mes, int Sb, int Sect);
        public Eficiencia_Detalle[] ObtenerEficienciaDetalleSector(int Id_Oficina, int Ano, int Mes, int Sb, int Sect, int Tipo);
        public Eficiencia_Detalle[] ObtenerEficienciaDetalleColonia(int Id_Oficina, int Ano, int Mes, int Sb, int Sect, int Tipo, int IdColonia, int IdLocalidad);


    }
}