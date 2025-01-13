using System;
using System.IO;
using System.Threading.Tasks;

namespace Sicem_Blazor.Helpers;

public class ConvertToBase64
{
    public static string ConvertFile(string fileSrc)
    {
        byte[] imageBytes = File.ReadAllBytes(fileSrc);
        return Convert.ToBase64String(imageBytes);
    }
}