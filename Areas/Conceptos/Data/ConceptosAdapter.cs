using System;
using System.Linq;
using System.Collections.Generic;
using Sicem_Blazor.Models.Entities.Arquos;
using Sicem_Blazor.Conceptos.Models;

namespace Sicem_Blazor.Conceptos.Data{
    public class ConceptosAdapter {

        public Concepto FromCatConcepto(CatConcepto c){
            if(c == null){
                return new Concepto();
            }
            var _result = new Concepto();
            _result.Id_Concepto = (int) c.IdConcepto;
            _result.Descripcion = c.Descripcion;
            _result.Importe = c.Importe;
            _result.Credito = c.Credito;
            _result.Mostrar = c.Mostrar;
            _result.Id_Estatus = (int) c.IdEstatus;
            _result.Inactivo = c.Inactivo;
            _result.Id_TipoConcepto = (int) c.IdTipoconcepto;
            _result.Costo_Estatico = c.CostoEstatico??false;
            return _result;
        }
    }
}