using System;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace BugShooting.Output.Printer
{
  partial class Send : Window
  {

    PrintEngine printEngine;

    public Send(PrintEngine printEngine)
    {
      InitializeComponent();

      this.printEngine = printEngine;

      PrinterComboBox.ItemsSource = PrinterSettings.InstalledPrinters;
      PrinterComboBox.SelectedValue = printEngine.PrinterName;

      NumberOfCopiesTextBox.Text = printEngine.NumberOfCopies.ToString();
      PageOrientationComboBox.SelectedIndex = (printEngine.Landscape) ? 1 : 0;
      CenterImageCheckBox.IsChecked = printEngine.CenterImage;
      FitImageCheckBox.IsChecked = printEngine.FitImage;
      InfoTopOfImageComboBox.SelectedIndex = (printEngine.InfoTopOfImage) ? 0 : 1;
      InfoWorkstationCheckBox.IsChecked = printEngine.InfoWorkstation;
      InfoCurrentUserCheckBox.IsChecked = printEngine.InfoCurrentUser;
      InfoPrintDateCheckBox.IsChecked = printEngine.InfoPrintDate;
      InfoImageTitleCheckBox.IsChecked = printEngine.InfoImageTitle;
      InfoImageNoteCheckBox.IsChecked = printEngine.InfoImageNote;
      InfoImageCreateDateCheckBox.IsChecked = printEngine.InfoImageCreateDate;
      InfoImageLastChangeDateCheckBox.IsChecked = printEngine.InfoImageLastChangeDate;
      CommentTextBox.Text = printEngine.Comment;
      InfoTextSizeTextBox.Text = printEngine.InfoTextSize.ToString();


      PrinterComboBox.SelectionChanged += ValidateData;
      NumberOfCopiesTextBox.TextChanged += ValidateData;
      PageOrientationComboBox.SelectionChanged += ValidateData;
      InfoTopOfImageComboBox.SelectionChanged += ValidateData;
      InfoTextSizeTextBox.TextChanged += ValidateData;
      ValidateData(null, null);

      PrinterComboBox.SelectionChanged += PrintSettings_Changed;
      NumberOfCopiesTextBox.TextChanged += PrintSettings_Changed;
      PageOrientationComboBox.SelectionChanged += PrintSettings_Changed;
      CenterImageCheckBox.Checked += PrintSettings_Changed;
      CenterImageCheckBox.Unchecked += PrintSettings_Changed;
      FitImageCheckBox.Checked += PrintSettings_Changed;
      FitImageCheckBox.Unchecked += PrintSettings_Changed;
      InfoTopOfImageComboBox.SelectionChanged += PrintSettings_Changed;
      InfoWorkstationCheckBox.Checked += PrintSettings_Changed;
      InfoWorkstationCheckBox.Unchecked += PrintSettings_Changed;
      InfoCurrentUserCheckBox.Checked += PrintSettings_Changed;
      InfoCurrentUserCheckBox.Unchecked += PrintSettings_Changed;
      InfoPrintDateCheckBox.Checked += PrintSettings_Changed;
      InfoPrintDateCheckBox.Unchecked += PrintSettings_Changed;
      InfoImageTitleCheckBox.Checked += PrintSettings_Changed;
      InfoImageTitleCheckBox.Unchecked += PrintSettings_Changed;
      InfoImageNoteCheckBox.Checked += PrintSettings_Changed;
      InfoImageNoteCheckBox.Unchecked += PrintSettings_Changed;
      InfoImageCreateDateCheckBox.Checked += PrintSettings_Changed;
      InfoImageCreateDateCheckBox.Unchecked += PrintSettings_Changed;
      InfoImageLastChangeDateCheckBox.Checked += PrintSettings_Changed;
      InfoImageLastChangeDateCheckBox.Unchecked += PrintSettings_Changed;
      CommentTextBox.TextChanged += PrintSettings_Changed;
      InfoTextSizeTextBox.TextChanged += PrintSettings_Changed;

    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      InitPreview();
    }

    private void PrintSettings_Changed(object sender, EventArgs e)
    {

      if (Validation.IsValid(PrinterComboBox))
      {
        printEngine.PrinterName = (string)PrinterComboBox.SelectedItem;
      }

      if (Validation.IsValid(NumberOfCopiesTextBox))
      {
        printEngine.NumberOfCopies = Convert.ToInt32(NumberOfCopiesTextBox.Text);
      }

      if (Validation.IsValid(PageOrientationComboBox))
      {
        printEngine.Landscape = PageOrientationComboBox.SelectedIndex == 1;
      }

      if (Validation.IsValid(InfoTopOfImageComboBox))
      {
        printEngine.InfoTopOfImage = InfoTopOfImageComboBox.SelectedIndex == 0;
      }

      if (Validation.IsValid(InfoTextSizeTextBox))
      {
        printEngine.InfoTextSize = Convert.ToInt32(InfoTextSizeTextBox.Text);
      }

      printEngine.CenterImage = CenterImageCheckBox.IsChecked.Value;
      printEngine.FitImage = FitImageCheckBox.IsChecked.Value;
      printEngine.InfoWorkstation = InfoWorkstationCheckBox.IsChecked.Value;
      printEngine.InfoCurrentUser = InfoCurrentUserCheckBox.IsChecked.Value;
      printEngine.InfoPrintDate = InfoPrintDateCheckBox.IsChecked.Value;
      printEngine.InfoImageTitle = InfoImageTitleCheckBox.IsChecked.Value;
      printEngine.InfoImageNote = InfoImageNoteCheckBox.IsChecked.Value;
      printEngine.InfoImageCreateDate = InfoImageCreateDateCheckBox.IsChecked.Value;
      printEngine.InfoImageLastChangeDate = InfoImageLastChangeDateCheckBox.IsChecked.Value;
      printEngine.Comment = CommentTextBox.Text;
     
      InitPreview();

    }

    private void InitPreview()
    {

      int paperHeight;

      if (printEngine.Landscape)
      {
        PrintPreviewGrid.Width = PrintPreviewGrid.ActualHeight / printEngine.PrintDocument.DefaultPageSettings.PaperSize.Width * printEngine.PrintDocument.DefaultPageSettings.PaperSize.Height;
        paperHeight = printEngine.PrintDocument.DefaultPageSettings.PaperSize.Width;
      }
      else
      {
        PrintPreviewGrid.Width = PrintPreviewGrid.ActualHeight / printEngine.PrintDocument.DefaultPageSettings.PaperSize.Height * printEngine.PrintDocument.DefaultPageSettings.PaperSize.Width;
        paperHeight = printEngine.PrintDocument.DefaultPageSettings.PaperSize.Height;
      }


      System.Drawing.Bitmap previewImage = new System.Drawing.Bitmap((int)PrintPreviewGrid.Width, (int)PrintPreviewGrid.ActualHeight);
      using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(previewImage))
      {
        printEngine.DrawPage(graphics, previewImage.Size, paperHeight);
      }

      PrintPreviewImage.Source = Imaging.CreateBitmapSourceFromHBitmap(previewImage.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

    }

    private void NumberOfCopies_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
    }

    private void InfoTextSize_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
    }

    private void ValidateData(object sender, EventArgs e)
    {
      OK.IsEnabled = Validation.IsValid(PrinterComboBox) &&
                     Validation.IsValid(NumberOfCopiesTextBox) &&
                     Validation.IsValid(PageOrientationComboBox) &&
                     Validation.IsValid(InfoTopOfImageComboBox);
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = true;
    }

  }

}
