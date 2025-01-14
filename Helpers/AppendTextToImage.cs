using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Sicem_Blazor.Models;


namespace Sicem_Blazor.Helpers;

public class AppendTextToImage
{
    public float TextPadding {get;set;} = 60f;
    public string TextFont {get;set;} = "Arial";
    public float TextFontSize {get;set;} = 30f;
    public string TextColorHex {get;set;} = "#342222FF";
    private Font font;


    /// <summary>
    ///  insert the texto into the original imagen and saved to the destination path
    /// </summary>
    /// <param name="fileSrc"></param>
    /// <param name="message"></param>
    /// <param name="fileDestination"></param>
    /// <exception cref="FileLoadException"></exception>
    /// <exception cref="FontException"></exception>
    /// <returns></returns>
    public async Task AppendText(string fileSrc, string message, string fileDestination)
    {
        // * attempt to load the image
        Image image;
        try
        {
            image = await Image.LoadAsync(fileSrc);
        }
        catch (Exception err)
        {
            throw new FileLoadException($"Fail to load the original image: {err.Message}", err);
        }

        // * attempt to load the font
        FontFamily fontFamily;
        if (!SystemFonts.TryGet(TextFont, out fontFamily))
        {
            throw new FontException($"Couldn't find font {TextFont}");
        }
        this.font = fontFamily.CreateFont(TextFontSize, FontStyle.Regular);
        var options = new TextOptions(font)
        {
            Dpi = 72,
            KerningMode = KerningMode.Auto,
            HorizontalAlignment = SixLabors.Fonts.HorizontalAlignment.Center,
            VerticalAlignment = SixLabors.Fonts.VerticalAlignment.Center,
            WrappingLength = image.Width - (TextPadding * 2),
        };

        // * calculate the size of the text
        var rect = TextMeasurer.MeasureSize(message, options);

        var richTextOptions = new RichTextOptions(font);
        richTextOptions.Origin = new PointF(TextPadding, TextPadding * 4f);
        richTextOptions.TextAlignment = TextAlignment.Center;
        richTextOptions.TextJustification = TextJustification.None;
        richTextOptions.WrappingLength = image.Width - (TextPadding * 1.9f);
        richTextOptions.LineSpacing = 1.25f;

        // * draw the text into the image
        var pointF = new PointF(TextPadding, TextPadding + 10);
        image.Mutate(x => x.DrawText(
            richTextOptions,
            message,
            new Color(Rgba32.ParseHex(TextColorHex))
        ));

        // * save the file with the text
        await image.SaveAsJpegAsync(fileDestination);
    }

    /// <summary>
    ///  insert the texto into the original imagen and saved to the destination path
    /// </summary>
    /// <param name="fileSrc"></param>
    /// <param name="message"></param>
    /// <param name="fileDestination"></param>
    /// <param name="appendTextSettings"></param>
    /// <exception cref="FileLoadException"></exception>
    /// <exception cref="FontException"></exception>
    /// <returns></returns>
    public async Task AppendText(string fileSrc, string message, string fileDestination, AppendTextSettings appendTextSettings)
    {
        // * attempt to load the image
        Image image;
        try
        {
            image = await Image.LoadAsync(fileSrc);
        }
        catch (Exception err)
        {
            throw new FileLoadException($"Fail to load the original image: {err.Message}", err);
        }

        // * attempt to load the font
        FontFamily fontFamily;
        if (!SystemFonts.TryGet(TextFont, out fontFamily))
        {
            throw new FontException($"Couldn't find font {TextFont}");
        }
        this.font = fontFamily.CreateFont(appendTextSettings.FontSize, FontStyle.Regular);

        var __textAlignment = appendTextSettings.TextAlignment switch
        {
            0 => TextAlignment.Start,
            1 => TextAlignment.End,
            _ => TextAlignment.Center
        };

        float __wrappingLength = image.Width - (TextPadding * 2);
        if(appendTextSettings.WrappingLength != null)
        {
            __wrappingLength = appendTextSettings.WrappingLength.Value;
        }

        var richTextOptions = new RichTextOptions(font);
        richTextOptions.Origin = new PointF(appendTextSettings.PaddingLeft, appendTextSettings.PaddingTop);
        richTextOptions.TextAlignment = __textAlignment;
        richTextOptions.TextJustification = TextJustification.None;
        richTextOptions.WrappingLength = __wrappingLength;
        richTextOptions.LineSpacing = appendTextSettings.LineSpacing;

        // * draw the text into the image
        image.Mutate(x => x.DrawText(
            richTextOptions,
            message,
            new Color(Rgba32.ParseHex(appendTextSettings.FontColor))
        ));

        // * save the file with the text
        await image.SaveAsJpegAsync(fileDestination);
    }
}