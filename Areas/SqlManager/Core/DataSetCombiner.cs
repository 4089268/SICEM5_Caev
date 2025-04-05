using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Sicem_Blazor.SqlManager.Core;

public class DataSetCombiner
{

    private DataSet[] dataSets;
    private readonly int totalTables;

    private DataTable[] combinedTables;


    /// <param name="dataSets"></param>
    /// <exception cref="ArgumentException">No datasets provided</exception>
    public DataSetCombiner(IEnumerable<DataSet> dataSets)
    {
        if(!dataSets.Any())
        {
            throw new ArgumentException("No datasets provided");
        }

        this.dataSets = dataSets.ToArray();
        var _firstDataSet = dataSets.First();
        this.totalTables = _firstDataSet.Tables.Count;
        
        // * initialize the combined tables
        combinedTables = new DataTable[totalTables];
        for(int i = 0; i < totalTables; i++)
        {
            var table = _firstDataSet.Tables[i];
            combinedTables[i] = table.Clone();
            combinedTables[i].TableName = $"Resultados{i + 1}";
        }
    }

    public DataSet Combine()
    {
        foreach(var ds in dataSets)
        {
            for(int i = 0; i < totalTables; i++)
            {
                var _currentTable = ds.Tables[i];

                foreach(DataRow dataRow in _currentTable.Rows)
                {
                    combinedTables[i].ImportRow(dataRow);
                }
            }
        }

        // Combine the combinedTables into a new DataSet
        var resultDataSet = new DataSet("Resultados");
        foreach (var table in combinedTables)
        {
            resultDataSet.Tables.Add(table);
        }
        return resultDataSet;
    }
}