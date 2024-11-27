using System;
using System.Collections.Generic;
using Sicem_Blazor.Conceptos.Models;

namespace Sicem_Blazor.Conceptos.Data {
    public interface IConceptosRepository {
        public IEnumerable<Concepto> ObtenerConceptos();
    }
}