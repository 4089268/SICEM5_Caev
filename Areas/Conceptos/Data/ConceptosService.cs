using System;
using System.Collections.Generic;
using System.Linq;
using Sicem_Blazor.Data;
using Sicem_Blazor.Models;
using Sicem_Blazor.Conceptos.Models;
using Sicem_Blazor.Conceptos.Data;
using Sicem_Blazor.Models.Entities.Arquos;

namespace Sicem_Blazor.Services {
    public class ConceptosService {
        private readonly SicemService sicemService;
        private readonly SicemContext sicemContext;

        private ConceptosAdapter conceptosAdapter;

        public ConceptosService(SicemService s, SicemContext c){
            conceptosAdapter = new ConceptosAdapter();
            this.sicemService = s;
            this.sicemContext = c;
        }

        /// <summary>
        /// Realiza una consulta a la tabla de cat_conceptos de la oficina que se pase como parámetro.
        /// </summary>
        public IEnumerable<Concepto> ObtenerCatalogoConceptos(IEnlace enlace){
            IEnumerable<Concepto> datos = new List<Concepto>();
            try{

                //*** Obtener los conceptos de la oficina
                IConceptosRepository conceptosRepository = new ConceptosArquosRepository(enlace);
                datos = conceptosRepository.ObtenerConceptos();

                //*** Obtener el catalogo de estatus 
                var _catEstatus = ObtenerCatalogoEstatus(enlace);

                //*** Modificar el estatus
                foreach(var item in datos){
                    if(_catEstatus.ContainsKey(item.Id_Estatus)){
                        item.Estatus = _catEstatus[(int)item.Id_Estatus].ToString();
                    }
                }

                return datos;
            }catch(Exception err){
                Console.WriteLine($">> Error al obtener el catalogo de conceptos de la oficina {enlace.Nombre}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
                return null;
            }
        }

        /// <summary>
        /// Realiza una consulta al catálogo de estatus y regresando únicamente los campos relacionados con la tabla de conceptos.
        /// </summary>
        public Dictionary<int,string> ObtenerCatalogoEstatus(IEnlace enlace){
            var result = new Dictionary<int,string>();
            var _datos = new List<CatEstatus>();
            try{
                using(var context = new ArquosContext(enlace.GetConnectionString())){
                    _datos = context.CatEstatuses.Where(item => item.IdEstatus == 0 || item.Tabla == "CAT_CONCEPTOS").ToList();
                }
                _datos.ForEach(item => result.Add((int)item.IdEstatus, item.Descripcion));
                return result;
            }catch(Exception err){
                Console.WriteLine($">> Error al obtener el catalogo de esatus de la oficina {enlace.Nombre}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
                return null;
            }
        }

        /// <summary>
        /// Realiza una modificacion directa a la table de cat_conceptos de la oficina que se pase como parametro.
        /// También registra la modificación echa en el historial de modificaciones de sicem.
        /// </summary>
        public bool ModificarConcepto(IEnlace enlace, Concepto concepto){
            throw new NotImplementedException();
            // try {

            //     Concepto _conceptoAnterior;
            //     CatConcepto _conceptoNuevo;
            //     int resultUpdate = 0;

            //     //****** Aplicar modificacion
            //     using(var context = new ArquosContext(enlace.GetConnectionString() )){
            //         _conceptoAnterior =  conceptosAdapter.FromCatConcepto(context.CatConceptos.Find(concepto.Id_Concepto));
            //         _conceptoNuevo = context.CatConceptos.Find(concepto.Id_Concepto);
            //         _conceptoNuevo.Importe = concepto.Importe;
            //         _conceptoNuevo.IdEstatus = concepto.Id_Estatus;
            //         _conceptoNuevo.Credito = concepto.Credito;
            //         _conceptoNuevo.Mostrar = concepto.Mostrar;
            //         _conceptoNuevo.Inactivo = concepto.Inactivo;
            //         _conceptoNuevo.CostoEstatico = concepto.Costo_Estatico;
            //         context.CatConceptos.Update(_conceptoNuevo);
            //         resultUpdate = context.SaveChanges();
            //     }

            //     //****** Comprobar si se realizo la modificacion en la oficina
            //     if( resultUpdate <= 0){
            //         return false;
            //     }

            //     //****** Inicializar el modelo de historial de modificaciones
            //     var _modificacionItem = new ModsOficina(){
            //         IdUsuario = ConvertUtils.ParseInteger(sicemService.Usuario.Id),
            //         IdOficina = enlace.Id,
            //         Tabla = "Cat_Conceptos",
            //         Fecha = DateTime.Now,
            //         Descripcion = $"Modificacion concepto id {_conceptoNuevo.IdConcepto} - {_conceptoNuevo.Descripcion}"
            //     };

            //     //**** Comparar los conceptos para obtener las modificaciones
            //     var _resultComparasion = CompararConceptos(_conceptoAnterior, conceptosAdapter.FromCatConcepto(_conceptoNuevo));
                
            //     //****** Almacenar al historial de modificaciones
            //     sicemContext.ModsOficinas.Add(_modificacionItem);
            //     sicemContext.SaveChanges();

            //     var _listDetMods = new List<DetModsOficina>();                
            //     _resultComparasion.ForEach( item => _listDetMods.Add( new DetModsOficina(){
            //         IdModif = _modificacionItem.Id,
            //         Descripcion = item["Campo"].ToString(),
            //         ValorAnt = item["ValorAnt"].ToString(),
            //         ValorNuevo = item["ValorNuevo"].ToString()
            //     }));
            //     sicemContext.DetModsOficinas.AddRange(_listDetMods);
            //     sicemContext.SaveChanges();

            //     return true;
            // }catch(Exception err){
            //     Console.WriteLine($">> Error al tratar de modificar el concepto {concepto.Descripcion} de la oficina {enlace.Nombre}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
            //     return false;
            // }
        }

        /// <summary>
        /// Realiza una modificacion directa a la table de cat_conceptos de la oficina que se pase como parametro.
        /// También registra la modificación echa en el historial de modificaciones de sicem.
        /// </summary>
        public bool ModificarConceptos(IEnlace oficina, List<Concepto> conceptos){
            throw new NotImplementedException();
            // try {

            //     List<Concepto> _conceptosAnteriores;
            //     List<CatConcepto> _conceptosNuevos;
            //     int resultUpdate = 0;

            //     //****** Aplicar modificacion
            //     using(var context = new ArquosContext(oficina.GetConnectionString() )){
            //         _conceptosAnteriores = context.CatConceptos.Where( item => conceptos.Select(item => (int) item.Id_Concepto).ToList<int>().Contains((int)item.IdConcepto))
            //             .Select( item => conceptosAdapter.FromCatConcepto(item))
            //             .ToList<Concepto>();
            //         _conceptosNuevos = context.CatConceptos.Where( item => conceptos.Select(item => (int) item.Id_Concepto).ToList<int>().Contains((int)item.IdConcepto)).ToList();
            //         _conceptosNuevos.ForEach( item => {
            //             Concepto _concepto = conceptos.FirstOrDefault(c => c.Id_Concepto == (int) item.IdConcepto);
            //             if(_concepto != null){
            //                 item.Importe = _concepto.Importe;
            //                 item.IdEstatus = _concepto.Id_Estatus;
            //                 item.Credito = _concepto.Credito;
            //                 item.Mostrar = _concepto.Mostrar;
            //                 item.Inactivo = _concepto.Inactivo;
            //                 item.CostoEstatico = _concepto.Costo_Estatico;
            //             }
            //             context.CatConceptos.Update(item);
            //         });

            //         resultUpdate = context.SaveChanges();
            //     }
            //     Console.WriteLine($">> Registros modificados: {resultUpdate}");

            //     //****** Comprobar si se realizo la modificacion en la oficina
            //     if( resultUpdate <= 0){
            //         return false;
            //     }


            //     //****** Inicializar el modelo de historial de modificaciones
            //     foreach( var _concepto in _conceptosNuevos){

            //         var _modificacionItem = new ModsOficina(){
            //             IdUsuario = ConvertUtils.ParseInteger(sicemService.Usuario.Id),
            //             IdOficina = oficina.Id,
            //             Tabla = "Cat_Conceptos",
            //             Fecha = DateTime.Now,
            //             Descripcion = $"Modificacion concepto id {_concepto.IdConcepto} - {_concepto.Descripcion}"
            //         };

            //         //**** Comparar los conceptos para obtener las modificaciones
            //         var _conceptoAnterior = _conceptosAnteriores.FirstOrDefault(item => item.Id_Concepto == _concepto.IdConcepto);
            //         var _resultComparasion = CompararConceptos(_conceptoAnterior, conceptosAdapter.FromCatConcepto(_concepto) );
                    
            //         //****** Almacenar al historial de modificaciones
            //         sicemContext.ModsOficinas.Add(_modificacionItem);
            //         sicemContext.SaveChanges();

            //         var _listDetMods = new List<DetModsOficina>();
            //         _resultComparasion.ForEach( item =>  _listDetMods.Add( new DetModsOficina(){
            //             IdModif = _modificacionItem.Id,
            //             Descripcion = item["Campo"].ToString(),
            //             ValorAnt = item["ValorAnt"].ToString(),
            //             ValorNuevo = item["ValorNuevo"].ToString()
            //         }));
            //         sicemContext.DetModsOficinas.AddRange(_listDetMods);
            //     }
            //     sicemContext.SaveChanges();

            //     return true;
            // }catch(Exception err){
            //     Console.WriteLine($">> Error al tratar de modificar los conceptos de la oficina {oficina.Nombre}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
            //     return false;
            // }
        }

        private List<Dictionary<string, object>> CompararConceptos(Concepto anterior, Concepto nuevo){
            var _result = new List<Dictionary<string, object>>();

            if(anterior.Descripcion != nuevo.Descripcion){
                _result.Add( new  Dictionary<string, object>{ {"Campo" , "Descripcion"}, {"ValorAnt", anterior.Descripcion}, { "ValorNuevo", nuevo.Descripcion}});
            }

            if(anterior.Importe != nuevo.Importe){
                _result.Add( new  Dictionary<string, object>{ {"Campo" , "Importe"}, {"ValorAnt", anterior.Importe}, { "ValorNuevo", nuevo.Importe}});
            }

            if(anterior.Credito != nuevo.Credito){
                _result.Add( new  Dictionary<string, object>{ {"Campo" , "Credito"}, {"ValorAnt", anterior.Credito}, { "ValorNuevo", nuevo.Credito}});
            }

            if(anterior.Mostrar != nuevo.Mostrar){
                _result.Add( new  Dictionary<string, object>{ {"Campo" , "Mostrar"}, {"ValorAnt", anterior.Mostrar}, { "ValorNuevo", nuevo.Mostrar}});
            }

            if(anterior.Id_Estatus != nuevo.Id_Estatus){
                _result.Add( new  Dictionary<string, object>{ {"Campo" , "IdEstatus"}, {"ValorAnt", anterior.Id_Estatus}, { "ValorNuevo", nuevo.Id_Estatus}});
            }

            if(anterior.Inactivo != nuevo.Inactivo){
                _result.Add( new  Dictionary<string, object>{ {"Campo" , "Inactivo"}, {"ValorAnt", anterior.Inactivo}, { "ValorNuevo", nuevo.Inactivo}});
            }

            if(anterior.Costo_Estatico != nuevo.Costo_Estatico){
                _result.Add( new  Dictionary<string, object>{ {"Campo" , "CostoEstatico"}, {"ValorAnt", anterior.Costo_Estatico}, { "ValorNuevo", nuevo.Costo_Estatico}});
            }

            return _result;
        }

    }
}
