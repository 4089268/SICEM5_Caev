using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sicem_Blazor.SeguimientoCobros.Models;

namespace Sicem_Blazor.SeguimientoCobros.Data;

public class IncomeDataService
{
    private List<LiveIncome> data = new();
    public event Action OnChange;

    public List<LiveIncome> GetData() => data;

    public void SetData(List<LiveIncome> newData)
    {
        data = newData;
        NotifyDataChanged();
    }

    private void NotifyDataChanged() => OnChange?.Invoke();
}