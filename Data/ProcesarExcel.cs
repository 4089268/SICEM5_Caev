using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Aspose.Cells;

namespace Sicem_Blazor.Data {
    public class ProcesarExcel {

        public DataTable ToDataTable(string fileName, int columns = 0)
        {
            var workbook = new Workbook(fileName);
            WorksheetCollection collections = workbook.Worksheets;

            var _workSheets1 = collections[0];

            int _totalFilas = _workSheets1.Cells.MaxDataRow;
            int _totalColumnas = columns;
            if(columns == 0)
            {
                _totalColumnas = _workSheets1.Cells.MaxDataColumn;
            }

            // Generar dataTable
            var dataTableResponse = new DataTable();
            for(int i = 0; i < _totalColumnas; i++)
            {
                dataTableResponse.Columns.Add(new DataColumn($"column{i}"));
            }

            // * Poblar dataTable
            for(int fila = 0; fila < _totalFilas; fila++)
            {
                var _tmpRow = dataTableResponse.NewRow();
                for(int columna = 0; columna < _totalColumnas; columna ++)
                {
                    try
                    {
                        _tmpRow[columna] = _workSheets1.Cells[fila, columna].Value.ToString();
                    }catch(Exception)
                    {
                        //
                    }
                }
                dataTableResponse.Rows.Add(_tmpRow);
            }

            return dataTableResponse;
        }
        public DataTable ToDataTableFirstCol(string fileName){
            Console.WriteLine($"Procesar Archivo {fileName}");
            var workbook = new Workbook(fileName);
            WorksheetCollection collections = workbook.Worksheets;

            var _workSheets1 = collections.First();

            int _totalFilas = _workSheets1.Cells.MaxDataRow;

            // Generar dataTable
            var dataTableResponse = new DataTable();
            dataTableResponse.Columns.Add(new DataColumn($"column0"));

            //Poblar dataTable
            for(int fila = 0; fila <= _totalFilas; fila++)
            {
                var _tmpRow = dataTableResponse.NewRow();
                _tmpRow[0] = _workSheets1.Cells[fila, 0].Value.ToString();
                dataTableResponse.Rows.Add(_tmpRow);
            }

            return dataTableResponse;
        }
    }
}