using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Sicem_Blazor.Models;
using Sicem_Blazor.Helpers;

namespace Sicem_Blazor.Data.Notification;

public class MessageUtils
{

    /// <summary>
    /// make the message with the template and the user data
    /// </summary>
    /// <param name="mensajeTemplate"></param>
    /// <param name="usuario"></param>
    /// <returns></returns>
    public static string GenerateMessage(string mensajeTemplate, CatPadron usuario)
    {
        PropertyInfo[] propertiesCatPadron = typeof(CatPadron).GetProperties();
        IEnumerable<string> currencyProperties = new string[]{"Total", "Subtotal", "Iva"};
        IEnumerable<string> datesProperties = new string[]{"FechaVencimientoDate"};

        var mensajeResult = mensajeTemplate;
        foreach(var prop in propertiesCatPadron)
        {
            var propName = prop.Name;
            try
            {
                // Escape special characters in the property name for regex safety
                string escapedPropName = Regex.Escape(propName);

                mensajeResult = Regex.Replace(mensajeResult, @"\$\{\b" + propName + @"\b\}",
                    (match) =>
                    {

                    // Get the value of the property, handling null values
                    var value = prop.GetValue(usuario)?.ToString() ?? string.Empty;

                    // If the value is a numeric type like Total, format it
                    if (value != string.Empty && double.TryParse(value, out double result) && currencyProperties.Contains(propName))
                    {
                        return result.ToString("C2"); // Format as currency
                    }

                    // If the value is a date format it
                    if (value != string.Empty && DateTime.TryParse(value, out DateTime dateValue) && datesProperties.Contains(propName))
                    {
                        return dateValue.ToString("dd MMMM yyyy"); // Format as currency
                    }

                    return value;
                });
            }
            catch(Exception){
                // TODO: Handle exception
            }
        }
        return mensajeResult;
    }

    /// <summary>
    /// Generate a notification image with the message and return the image as base64 string
    /// </summary>
    /// <param name="pathDestination"></param>
    /// <param name="message"></param>
    /// <returns>The image in base64 format</returns>
    public static async Task<string> MakeImageNotification(string originalImagePath, string pathDestination, string message, AppendTextSettings appendTextSettings, string outputFileName = "notification.jpg")
    {
        // * prepare the destination path
        var destinationImagen = Path.Join(pathDestination, outputFileName);
        
        // * ensure the directory exists
        string directoryPath = Path.GetDirectoryName(destinationImagen);
        if (!string.IsNullOrEmpty(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var appendTextToImage = new AppendTextToImage();
        await appendTextToImage.AppendText(originalImagePath, message, destinationImagen, appendTextSettings);
        var imageBase64 = ConvertToBase64.ConvertFile(destinationImagen);

        return imageBase64;
    }

}