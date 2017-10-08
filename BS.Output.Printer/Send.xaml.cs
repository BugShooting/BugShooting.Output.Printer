using System;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace BS.Output.Printer
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
            
      PrinterComboBox.SelectionChanged += ValidateData;
      NumberOfCopiesTextBox.TextChanged += ValidateData;
      PageOrientationComboBox.SelectionChanged += ValidateData;
      InfoTopOfImageComboBox.SelectionChanged += ValidateData;
      ValidateData(null, null);

      PrinterComboBox.SelectionChanged += PrintSettings_Changed;
      NumberOfCopiesTextBox.TextChanged += PrintSettings_Changed;
      PageOrientationComboBox.SelectionChanged += PrintSettings_Changed;
      CenterImageCheckBox.Checked += PrintSettings_Changed;
      CenterImageCheckBox.Unchecked += PrintSettings_Changed;
      FitImageCheckBox.Checked += PrintSettings_Changed;
      FitImageCheckBox.Unchecked += PrintSettings_Changed;
      InfoTopOfImageComboBox.SelectionChanged += PrintSettings_Changed;
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

      InitPreview();

    }

    private void PrintSettings_Changed(object sender, EventArgs e)
    {

      printEngine.PrinterName = (string)PrinterComboBox.SelectedItem;
      printEngine.NumberOfCopies = Convert.ToInt32(NumberOfCopiesTextBox.Text);
      printEngine.Landscape = PageOrientationComboBox.SelectedIndex == 1;
      printEngine.CenterImage = CenterImageCheckBox.IsChecked.Value;
      printEngine.FitImage = FitImageCheckBox.IsChecked.Value;
      printEngine.InfoTopOfImage = InfoTopOfImageComboBox.SelectedIndex == 0;
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

      // TODO

    }

    private void NumberOfCopies_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
