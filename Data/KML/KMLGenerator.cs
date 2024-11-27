using System;
using System.Collections.Generic;
using System.Linq;
using SharpKml.Base;
using SharpKml.Dom;

namespace Sicem_Blazor.Data.KML {
    public class KMLGenerator {

        private List<KmlPoint> puntos;
        private List<Placemark> marcas;
        private Kml kml;

        public KMLGenerator(IEnumerable<KmlPoint> puntos){
            this.puntos = puntos.ToList();
            GenerarPlacemarks();
            GenerarKml();
        }

        private void GenerarPlacemarks(){
            marcas = new List<Placemark>();
            if(puntos.Count <= 0){
                return;
            }

            foreach(var p in puntos){
                var _point = new Point{
                    Coordinate = new Vector(p.Latitud, p.Longitud)
                };
                var _mark = new Placemark{
                    Name = p.Titulo,
                    Geometry = _point
                };
                this.marcas.Add(_mark);
            }

        }
        private void GenerarKml(){
            
            var folder = new Folder();
            folder.Id = "f0";
            folder.Name = "Predios";

            foreach(var feat in marcas){
                folder.AddFeature(feat);
            }

            // This is the root element of the file
            var placemark = marcas.FirstOrDefault();
            kml = new Kml
            {
                Feature = folder
            };

        }
        public string GenerarXml(){
            var serializer = new Serializer();
            serializer.Serialize(kml);
            var _kmlContent = serializer.Xml;
            return _kmlContent;
        }

        public void LeerKml(string kmlXml){
            var serializer = new Serializer();
            var parser = new Parser();
            parser.ParseString(kmlXml, true);

            var kml = (Kml)parser.Root;
            var placemark = (Placemark)kml.Feature;
            var point = (Point)placemark.Geometry;
        }

    }

}