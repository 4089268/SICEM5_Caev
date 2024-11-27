using System;
using System.Collections.Generic;
using System.Text;
using Sicem_Blazor.Models.Entities.Arquos;

namespace Sicem_Blazor.Models {
    public class Sicem_Colonia {
        public int Id_Colonia { get; set; } = 0;
        public string Descripcion { get; set; } = "";
        public int SubSistema { get; set; } = 0;
        public int Sector { get; set; } = 0;

        public static Sicem_Colonia FromEntity(CatColonia data){
            if(data == null){
                return null;
            }

            var _result = new Sicem_Colonia();
            _result.Id_Colonia = (int) data.IdColonia;
            _result.Descripcion = data.Descripcion;
            _result.SubSistema = (int) data.Sb;
            _result.Sector = (int) data.Sector;
            return _result;
        }
        
    }

    
}
