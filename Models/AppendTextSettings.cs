using System;
using Aspose.Pdf;

namespace Sicem_Blazor.Models;

public class AppendTextSettings
{
    public float PaddingLeft {get;set;} = 10f;
    public float PaddingTop {get;set;} = 10f;
    public float FontSize {get;set;} = 10f;
    public string FontColor {get;set;} = "#3a5a52";
    public float? WrappingLength {get;set;}
    public float LineSpacing {get;set;} = 1.25f;
    public int TextAlignment {get;set;} = 2; // 0=> Start; 2=>End; 2=>Center;

    public override string ToString()
    {
        return $"PaddingLeft: {PaddingLeft}, PaddingTop: {PaddingTop}, FontSize: {FontSize}, " +
               $"FontColor: {FontColor}, WrappingLength: {WrappingLength?.ToString() ?? "None"}, " +
               $"LineSpacing: {LineSpacing}, TextAlignment: {GetTextAlignment()}";
    }

    private string GetTextAlignment()
    {
        return TextAlignment switch
        {
            0 => "Start",
            1 => "Center",
            2 => "End",
            _ => "Unknown"
        };
    }

}