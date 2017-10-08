using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace BS.Output.Printer
{
  partial class Edit : Window
  {

    public Edit(Output output)
    {
      InitializeComponent();
  
      NameTextBox.Text = output.Name;
      NumberOfCopiesTextBox.Text = output.NumberOfCopies.ToString();
      PageOrientationComboBox.SelectedIndex = (output.Landscape) ? 1 : 0;
      CenterImageCheckBox.IsChecked = output.CenterImage;
      FitImageCheckBox.IsChecked = output.FitImage;
      InfoTopOfImageComboBox.SelectedIndex = (output.InfoTopOfImage) ? 0 : 1;
      InfoWorkstationCheckBox.IsChecked = output.InfoWorkstation;
      InfoCurrentUserCheckBox.IsChecked = output.InfoCurrentUser;
      InfoPrintDateCheckBox.IsChecked = output.InfoPrintDate;
      InfoImageTitleCheckBox.IsChecked = output.InfoImageTitle;
      InfoImageNoteCheckBox.IsChecked = output.InfoImageNote;
      InfoImageCreateDateCheckBox.IsChecked = output.InfoImageCreateDate;
      InfoImageLastChangeDateCheckBox.IsChecked = output.InfoImageLastChangeDate;
      ChangeSettingBeforePrintCheckBox.IsChecked = output.ChangeSettingBeforePrint;

      NumberOfCopiesTextBox.TextChanged += ValidateData;
      PageOrientationComboBox.SelectionChanged += ValidateData;
      InfoTopOfImageComboBox.SelectionChanged += ValidateData;
      InfoTextSizeTextBox.TextChanged += ValidateData;
      ValidateData(null, null);

    }
     
    public string OutputName
    {
      get { return NameTextBox.Text; }
    }

    public int NumberOfCopies
    {
      get { return Convert.ToInt32(NumberOfCopiesTextBox.Text); }
    }

    public bool Landscape
    {
      get { return PageOrientationComboBox.SelectedIndex == 1; }
    }

    public bool CenterImage
    {
      get { return CenterImageCheckBox.IsChecked.Value; }
    }

    public bool FitImage
    {
      get { return FitImageCheckBox.IsChecked.Value; }
    }

    public bool InfoTopOfImage
    {
      get { return InfoTopOfImageComboBox.SelectedIndex == 0; }
    }

    public bool InfoWorkstation
    {
      get { return InfoWorkstationCheckBox.IsChecked.Value; }
    }

    public bool InfoCurrentUser
    {
      get { return InfoCurrentUserCheckBox.IsChecked.Value; }
    }

    public bool InfoPrintDate
    {
      get { return InfoPrintDateCheckBox.IsChecked.Value; }
    }

    public bool InfoImageTitle
    {
      get { return InfoImageTitleCheckBox.IsChecked.Value; }
    }

    public bool InfoImageNote
    {
      get { return InfoImageNoteCheckBox.IsChecked.Value; }
    }

    public bool InfoImageCreateDate
    {
      get { return InfoImageCreateDateCheckBox.IsChecked.Value; }
    }

    public bool InfoImageLastChangeDate
    {
      get { return InfoImageLastChangeDateCheckBox.IsChecked.Value; }
    }

    public int InfoTextSize
    {
      get { return Convert.ToInt32(InfoTextSizeTextBox.Text); }
    }

    public bool ChangeSettingBeforePrint
    {
      get { return ChangeSettingBeforePrintCheckBox.IsChecked.Value; }
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
      OK.IsEnabled = Validation.IsValid(NameTextBox) &&
                     Validation.IsValid(PageOrientationComboBox) &&
                     Validation.IsValid(InfoTopOfImageComboBox) &&
                     Validation.IsValid(InfoTextSizeTextBox);
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = true;     
    }

  }
}
