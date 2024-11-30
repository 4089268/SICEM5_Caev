using System;
using Microsoft.AspNetCore.Components.Forms;

namespace Sicem_Blazor.Helpers;

public class BrowserFileValidator
{
    public static bool IsCsvFile(IBrowserFile file)
    {
        // Check MIME type (text/csv is the standard for CSV files)
        if (file.ContentType != "text/csv")
        {
            return false;
        }

        // Check file extension
        if (!file.Name.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }
}
