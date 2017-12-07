using BS.Plugin.V3.Common;
using System;
using System.Drawing;
using System.Drawing.Printing;

namespace BugShooting.Output.Printer
{
  internal class PrintEngine : IDisposable
  {

    ImageData imageData;
    PrintDocument printDocument;

    bool centerImage;
    bool fitImage;
    bool textTopOfImage;
    int textSize;
    string text;

    public PrintEngine(ImageData imageData)
    {
      this.imageData = imageData;

      printDocument = new PrintDocument();
      printDocument.DocumentName = "Bug Shooting";
      printDocument.PrintController = new StandardPrintController();
      printDocument.PrintPage += PrintDocument_PrintPage;

    }

    public ImageData ImageData
    {
      get { return imageData; }
    }

    public string PrinterName
    {
      get { return printDocument.PrinterSettings.PrinterName; }
      set { printDocument.PrinterSettings.PrinterName = value; }
    }

    public bool ValidPrinterSelected
    {
      get { return printDocument.PrinterSettings.IsValid; }
    }

    public int NumberOfCopies
    {
      get { return printDocument.PrinterSettings.Copies; }
      set { printDocument.PrinterSettings.Copies = (short)Math.Min(value, short.MaxValue); }
    }

    public bool Landscape
    {
      get { return printDocument.DefaultPageSettings.Landscape; }
      set { printDocument.DefaultPageSettings.Landscape = value; }
    }

    public bool CenterImage
    {
      get { return centerImage; }
      set { centerImage = value; }
    }

    public bool FitImage
    {
      get { return fitImage; }
      set { fitImage = value; }
    }

    public bool TextTopOfImage
    {
      get { return textTopOfImage; }
      set { textTopOfImage = value; }
    }

    public int TextSize
    {
      get { return textSize; }
      set { textSize = value; }
    }

    public string Text
    {
      get { return text; }
      set { text = value; }
    }

    public PrintDocument PrintDocument
    {
      get { return printDocument; }
    }

    public void Print()
    {
        printDocument.Print();
    }

    private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    {

      DrawPage(e.Graphics, e.PageBounds.Size, e.PageBounds.Size.Height);

      e.HasMorePages = false;

    }

    public void DrawPage(Graphics graphics, Size pageSize, int paperHeight)
    {

      double shrinkFactor = paperHeight / pageSize.Height;
      int marginLeft = (int)(printDocument.DefaultPageSettings.Margins.Left / shrinkFactor);
      int marginTop = (int)(printDocument.DefaultPageSettings.Margins.Top / shrinkFactor);
      int availableWidth = (int)(pageSize.Width - (printDocument.DefaultPageSettings.Margins.Right / shrinkFactor) - marginLeft);
      int availableHeight = (int)(pageSize.Height - (printDocument.DefaultPageSettings.Margins.Bottom / shrinkFactor) - marginTop);

      using (Font textFont = new Font(SystemFonts.DefaultFont.FontFamily, (int)(Math.Max(1, Math.Max(Math.Min(textSize, 30), 1) / shrinkFactor))))
      {

        // Calculate text height
        int textHeight = 0;
        string printText = string.Empty;

        if (!string.IsNullOrEmpty(text))
        {

          if (textTopOfImage)
          {
            printText = text + Environment.NewLine;
          }
          else
          {
            printText = Environment.NewLine + text;
          }

          textHeight = (int)(graphics.MeasureString(printText, textFont, new Size(availableWidth, availableHeight)).Height);

        }

        int availableImageHeight = availableHeight - textHeight;


        // Draw image
        double previewImageWidth = imageData.MergedImage.Width / shrinkFactor;
        double previewImageHeight = imageData.MergedImage.Height / shrinkFactor;
        
        if (fitImage)
        {
          if (previewImageWidth < availableWidth)
          {
            previewImageHeight = previewImageHeight / previewImageWidth * availableWidth;
            previewImageWidth = availableWidth;
          }

          if (previewImageHeight < availableImageHeight)
          {
            previewImageWidth = previewImageWidth / previewImageHeight * availableImageHeight;
            previewImageHeight = availableImageHeight;
          }

          if (previewImageWidth > availableWidth)
          {
            previewImageHeight = previewImageHeight / previewImageWidth * availableWidth;
            previewImageWidth = availableWidth;
          }

          if (previewImageHeight > availableImageHeight)
          {
            previewImageWidth = previewImageWidth / previewImageHeight * availableImageHeight;
            previewImageHeight = availableImageHeight;
          }

        }

        int imageLeft;
        int imageTop;

        if (centerImage)
        {
          imageLeft = (int)(marginLeft + availableWidth / 2 - previewImageWidth / 2);
          imageTop = (int)(marginTop + availableImageHeight / 2 - previewImageHeight / 2);

          if (textTopOfImage)
          {
            if (imageTop < marginTop + textHeight)
            {
              imageTop = marginTop + textHeight;
            }
          }
          else
          {
            if (imageTop < marginTop)
            {
              imageTop = marginTop;
            }
          }
        }
        else
        {
          imageLeft = marginLeft;
          imageTop = marginTop;

          if (textTopOfImage)
          {
            imageTop += textHeight;
          }
        }

        graphics.SetClip(new Rectangle(marginLeft, marginTop, availableWidth, availableHeight));
        graphics.DrawImage(imageData.MergedImage, imageLeft, imageTop, (int)(previewImageWidth), (int)(previewImageHeight));


        // Draw Text
        if (!string.IsNullOrEmpty(printText))
        {

          if (textTopOfImage)
          {
            graphics.DrawString(printText, textFont, Brushes.Black, new Rectangle(marginLeft, marginTop, availableWidth, textHeight));
          }
          else
          {
            graphics.DrawString(printText, textFont, Brushes.Black, new Rectangle(marginLeft, imageTop + (int)(previewImageHeight), availableWidth, textHeight));
          }

        }

        graphics.ResetClip();


      }

    }
     
    public void Dispose()
    {
      if (printDocument != null)
      {
        printDocument.PrintPage -= PrintDocument_PrintPage;
        printDocument.Dispose();
        printDocument = null;
      }

    }

  }

}
