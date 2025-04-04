using System;
using System.Data;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace Sicem_Blazor.Helpers;

public class ExportDataSet
{
    private DataSet dataSet;

    public ExportDataSet(DataSet dataSet)
    {
        this.dataSet = dataSet;
    }

    public string ExportToFile(string fileName)
    {
        // var _guid = Guid.NewGuid().ToString();
        // var _nombreArchivo = $"{fileName}.xlsx";
        // var _rutaArchivo = string.Format("{0}/{1}/{2}", Path.GetTempPath(), _guid, _nombreArchivo);
        var _fileInfo = new System.IO.FileInfo(fileName);
        _fileInfo.Directory.Create();

        using (var package = new ExcelPackage(_fileInfo.FullName))
        {
            foreach (DataTable table in dataSet.Tables)
            {
                var sheet = package.Workbook.Worksheets.Add(table.TableName);
                sheet.Cells["A1"].LoadFromDataTable(table, true, TableStyles.Medium9);
            }
            package.Save();
        }

        Console.WriteLine("Generando archivo en ruta: " + fileName);
        return _fileInfo.FullName;
    }

    public byte[] ExportToBytes(string fileName)
    {
        var _guid = Guid.NewGuid().ToString();
        var _nombreArchivo = $"{fileName}.xlsx";
        var _rutaArchivo = string.Format("{0}/{1}/{2}", Path.GetTempPath(), _guid, _nombreArchivo);
        var _fileInfo = new System.IO.FileInfo(_rutaArchivo);
        _fileInfo.Directory.Create();
        
        Console.WriteLine("Generando archivo en ruta " + _rutaArchivo);

        byte[] fileContents;
        using (var package = new ExcelPackage(_fileInfo.FullName))
        {
            foreach (DataTable table in dataSet.Tables)
            {
                var sheet = package.Workbook.Worksheets.Add(table.TableName);
                sheet.Cells["A1"].LoadFromDataTable(table, true, TableStyles.Medium9);
            }
            fileContents = package.GetAsByteArray();
        }
        return fileContents;
    }

}
