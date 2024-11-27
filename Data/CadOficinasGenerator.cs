using System;
using System.Linq;

namespace Sicem_Blazor.Data {
    public class CadOficinasGenerator {
        
        public static string Genearar(int idOficina){
            var cadConexion = new char[ idOficina +1 ];
            for(int i = 0; i <= cadConexion.Count() - 1; i++){
                if(i == idOficina - 1){
                    cadConexion[i] = '1';
                }else{
                    cadConexion[i] = '0';
                }
            }
            return new String(cadConexion);
        }

    }
}