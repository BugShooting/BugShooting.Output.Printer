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
      TextPositionComboBox.SelectedIndex = (printEngine.TextTopOfImage) ? 0 : 1;
      TextSizeTextBox.Text = printEngine.TextSize.ToString();
      TextTextBox.Text = printEngine.Text;
            
      PrinterComboBox.SelectionChanged += ValidateData;
      NumberOfCopiesTextBox.TextChanged += ValidateData;
      PageOrientationComboBox.SelectionChanged += ValidateData;
      TextPositionComboBox.SelectionChanged += ValidateData;
      TextSizeTextBox.TextChanged += ValidateData;
      ValidateData(null, null);

      PrinterComboBox.SelectionChanged += PrintSettings_Changed;
      NumberOfCopiesTextBox.TextChanged += PrintSettings_Changed;
      PageOrientationComboBox.SelectionChanged += PrintSettings_Changed;
      CenterImageCheckBox.Checked += PrintSettings_Changed;
      CenterImageCheckBox.Unchecked += PrintSettings_Changed;
      FitImageCheckBox.Checked += PrintSettings_Changed;
      FitImageCheckBox.Unchecked += PrintSettings_Changed;
      TextPositionComboBox.SelectionChanged += PrintSettings_Changed;
      TextSizeTextBox.TextChanged += PrintSettings_Changed;
      TextTextBox.TextChanged += PrintSettings_Changed;
      
    }

    public string Text
    {
      get { return TextTextBox.Text; }
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

      if (Validation.IsValid(TextPositionComboBox))
      {
        printEngine.TextTopOfImage = TextPositionComboBox.SelectedIndex == 0;
      }

      if (Validation.IsValid(TextSizeTextBox))
      {
        printEngine.TextSize = Convert.ToInt32(TextSizeTextBox.Text);
      }

      printEngine.CenterImage = CenterImageCheckBox.IsChecked.Value;
      printEngine.FitImage = FitImageCheckBox.IsChecked.Value;
      printEngine.Text = TextTextBox.Text;

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

    private void TextSize_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
    }

    private void ValidateData(object sender, EventArgs e)
    {
      OK.IsEnabled = Validation.IsValid(PrinterComboBox) &&
                     Validation.IsValid(NumberOfCopiesTextBox) &&
                     Validation.IsValid(PageOrientationComboBox) &&
                     Validation.IsValid(TextPositionComboBox);
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = true;
    }

  }

}
