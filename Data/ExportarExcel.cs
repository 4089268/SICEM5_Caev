using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using OfficeOpenXml;


namespace Sicem_Blazor.Data {
    public class ExportarExcel<T> {

        private readonly Type dataType;
        private readonly ExportarExcelProperties propiedadesExportar;
        private readonly Uri tmpFolder;
        private ICollection<T> datos;


        public ExportarExcel(ICollection<T> datos, Uri tmpFolder ){
            this.dataType = typeof(T);
            this.datos = datos;
            this.tmpFolder = tmpFolder;
            this.propiedadesExportar = new ExportarExcelProperties();
        }

        public ExportarExcel(ICollection<T> datos, Uri tmpFolder,  ExportarExcelProperties props ){
            this.dataType = typeof(T);
            this.datos = datos;
            this.tmpFolder = tmpFolder;
            this.propiedadesExportar = props;
        }
        

        public string GenerarExcel(){
            var _guid = Guid.NewGuid().ToString();
            var _nombreArchivo = $"{propiedadesExportar.Titulo}.xlsx";
            var _rutaArchivo = $"{tmpFolder.AbsolutePath}{_guid}{System.IO.Path.DirectorySeparatorChar}{_nombreArchivo}";
            var _fileInfo = new System.IO.FileInfo(_rutaArchivo);
            _fileInfo.Directory.Create();
            
            Console.WriteLine("Generando archivo en ruta " + _rutaArchivo);
            
            using(var package = new ExcelPackage(_fileInfo.FullName))
            {
                var sheet = package.Workbook.Worksheets.Add("My Sheet");
                
                var _columnas = FiltrarColumnas(dataType.GetRuntimeFields().ToList());
                var _totalColumnas = _columnas.Count();
                int row = 1;

                
                // Generar cabecesar
                int col = 1;
                foreach(var _columna in _columnas){
                    sheet.Column(col).BestFit = true;
                    GenerarCabecera(row, col, sheet, _columna);
                    col ++;
                }
                row ++;


                // Agrupar filas por oficina    
                // var _rowsRange = sheet.Rows[row, row + datos.Count()];
                // _rowsRange.Group();
                // _rowsRange.CollapseChildren();

                // Generar datos
                foreach(var datoActual in datos){
                    col = 1;
                    foreach(var _columna in _columnas){
                        GenerarCelda(row, col, sheet, _columna, datoActual);
                        col ++;
                    }
                    row++;
                }

                // Save to file
                package.Save();                
            }

            return _guid;
        }

        private ICollection<FieldInfo> FiltrarColumnas(ICollection<FieldInfo> fields){
            return fields.Where(item => !propiedadesExportar.CamposOmitir.Contains(ProcesarNombreCampo(item).ToLower())).ToList();
        }
        private void GenerarCabecera(int row, int col, ExcelWorksheet sheet, FieldInfo field){
            var _cell = sheet.Cells[row, col];
            _cell.Value = ProcesarNombreCampo(field).ToUpper();
            _cell.Style.Font.Bold = true;
            _cell.Style.Font.Size = 14;
            //_cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Salmon);
        }
        private void GenerarCelda(int row, int col, ExcelWorksheet sheet, FieldInfo field, T valor ){
            var _cell = sheet.Cells[row, col];

            // switch(field.FieldType.Name){
            //     case "Int32":
            //         _cell.Value = field.GetValue(valor);
            //         _cell.Style.Numberformat.Format = "n0";
            //         _cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //         break;

            //     case "Int64":
            //         _cell.Value = field.GetValue(valor);
            //         _cell.Style.Numberformat.Format = "n0";
            //         _cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //         break;

            //     case "Double":
            //         _cell.Value = field.GetValue(valor);
            //         _cell.Style.Numberformat.Format = "n2";
            //         _cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //         break;

            //     case "Decimal":
            //         _cell.Value = field.GetValue(valor);
            //         _cell.Style.Numberformat.Format = "c2";
            //         _cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            //         break;

            //     default:
            //         _cell.Value = field.GetValue(valor);
            //         break;

            // }

            _cell.Value = field.GetValue(valor);
            
            _cell.Style.Font.Size = 12;
        }


        public override string ToString()
        {
            
            var _fields = dataType.GetRuntimeFields();
            Console.WriteLine($"Total Registro: {datos.Count()} Total Fields: {_fields.Count()}");
            var _stringBuilder = new System.Text.StringBuilder();

            foreach(var f in _fields){
                _stringBuilder.Append($"Field2:{ProcesarNombreCampo(f)} Type:{f.FieldType.Name}\n" );
            }

            // foreach(var d in datos){
            //     foreach(var field in _fields){
            //         _stringBuilder.Append($">>{field.Name.ToString()}:{field.GetValue(d)}:{field.FieldType.Name}   | ");
            //     }
            //     _stringBuilder.Append("\n");
            // }
            return _stringBuilder.ToString();
        }


        private string ProcesarNombreCampo(FieldInfo fieldText){
            var name = fieldText.Name.Replace("<","").Replace(">k__BackingField", "");
            return name;
        }

    }

    public class ExportarExcelProperties{
        public string Titulo {get;set;} = "Sicem";
        public ICollection<string> CamposOmitir {get;set;} = new string[]{"id_oficina", "id_padron"};
    }
}