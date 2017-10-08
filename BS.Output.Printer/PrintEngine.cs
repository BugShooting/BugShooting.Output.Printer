using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;

namespace BS.Output.Printer
{
  internal class PrintEngine : IDisposable
  {

    V3.ImageData imageData;
    Image image;
    PrintDocument printDocument;

    bool centerImage;
    bool fitImage;
    bool infoTopOfImage;
    bool infoWorkstation;
    bool infoCurrentUser;
    bool infoPrintDate;
    bool infoImageTitle;
    bool infoImageNote;
    bool infoImageCreateDate;
    bool infoImageLastChangeDate;
    string comment;
    
    public PrintEngine(V3.ImageData imageData)
    {
      this.imageData = imageData;

      image = imageData.GetImage();

      printDocument = new PrintDocument();
      printDocument.DocumentName = "Bug Shooting";
      printDocument.PrintController = new StandardPrintController();
      printDocument.PrintPage += PrintDocument_PrintPage;

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

    public bool InfoTopOfImage
    {
      get { return infoTopOfImage; }
      set { infoTopOfImage = value; }
    }

    public bool InfoWorkstation
    {
      get { return infoWorkstation; }
      set { infoWorkstation = value; }
    }

    public bool InfoCurrentUser
    {
      get { return infoCurrentUser; }
      set { infoCurrentUser = value; }
    }

    public bool InfoPrintDate
    {
      get { return infoPrintDate; }
      set { infoPrintDate = value; }
    }

    public bool InfoImageTitle
    {
      get { return infoImageTitle; }
      set { infoImageTitle = value; }
    }

    public bool InfoImageNote
    {
      get { return infoImageNote; }
      set { infoImageNote = value; }
    }

    public bool InfoImageCreateDate
    {
      get { return infoImageCreateDate; }
      set { infoImageCreateDate = value; }
    }

    public bool InfoImageLastChangeDate
    {
      get { return infoImageLastChangeDate; }
      set { infoImageLastChangeDate = value; }
    }

    public string Comment
    {
      get { return comment; }
      set { comment = value; }
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

      using (Font infoTextFont = new Font(SystemFonts.DefaultFont.FontFamily, (int)(10 / shrinkFactor)))
      {

        // Init info text
        StringBuilder infoTextBuilder = new StringBuilder();

        if (infoWorkstation)
          infoTextBuilder.AppendFormat("Workstation: {0}\r\n", Environment.MachineName);

        if (infoCurrentUser)
          infoTextBuilder.AppendFormat("User name: {0}\r\n", Environment.UserName);

        if (infoPrintDate)
          infoTextBuilder.AppendFormat("Print date: {0}\r\n", DateTime.Now.ToString("G"));

        if (infoImageTitle)
          infoTextBuilder.AppendFormat("Image title: {0}\r\n", imageData.Title);

        if (infoImageNote)
          infoTextBuilder.AppendFormat("Image note: {0}\r\n", imageData.Note);

        if (infoImageCreateDate)
          infoTextBuilder.AppendFormat("Create date: {0}\r\n", imageData.CreateDate.ToString("G"));

        if (infoImageLastChangeDate)
          infoTextBuilder.AppendFormat("Last change: {0}\r\n", imageData.ChangeDate.ToString("G"));

        if (!string.IsNullOrEmpty(comment))
          infoTextBuilder.AppendFormat("\r\n{0}\r\n", comment);


        // Calculate info text height
        string infoText = infoTextBuilder.ToString();
        int infoTextHeight = 0;
        string printInfoText = string.Empty;
        
        if (!string.IsNullOrEmpty(infoText))
        {

          if (infoTopOfImage)
          {
            printInfoText = infoText + Environment.NewLine;
          }
          else
          {
            printInfoText = Environment.NewLine + infoText;
          }

          infoTextHeight = (int)(graphics.MeasureString(printInfoText, infoTextFont, new Size(availableWidth, availableHeight)).Height);

        }

        int availableImageHeight = availableHeight - infoTextHeight;


        // Draw image

        double previewImageWidth = image.Width / shrinkFactor;
        double previewImageHeight = image.Height / shrinkFactor;


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

          if (infoTopOfImage)
          {
            if (imageTop < marginTop + infoTextHeight)
            {
              imageTop = marginTop + infoTextHeight;
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

          if (infoTopOfImage)
          {
            imageTop += infoTextHeight;
          }
        }

        graphics.SetClip(new Rectangle(marginLeft, marginTop, availableWidth, availableHeight));
        graphics.DrawImage(image, imageLeft, imageTop, (int)(previewImageWidth), (int)(previewImageHeight));


        // Draw Text

        if (!string.IsNullOrEmpty(printInfoText))
        {

          if (infoTopOfImage)
          {
            graphics.DrawString(printInfoText, infoTextFont, Brushes.Black, new Rectangle(marginLeft, marginTop, availableWidth, infoTextHeight));
          }
          else
          {
            graphics.DrawString(printInfoText, infoTextFont, Brushes.Black, new Rectangle(marginLeft, imageTop + (int)(previewImageHeight), availableWidth, infoTextHeight));
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
