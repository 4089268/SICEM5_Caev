using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Models{

    public class ConsultaGral_Documentos{
        public string Id_Imagen {get;set;} = "";
        public string Id_Padron {get;set;} = "";
        public string Descripcion {get;set;} = "";
        public string Archivo {get;set;} = "";
        public string Extencion {get;set;} = "";
        public DateTime? Fecha_Insert {get;set;}
        public byte[] Documento { get; set; }
        public long Tamano { get; set; }
        public string Personal { get; set; }

    }

}