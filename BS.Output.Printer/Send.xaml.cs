using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace BS.Output.Printer
{
  partial class Send : Window
  {
 
    public Send(int numberOfCopies,
                bool landscape,
                bool centerImage,
                bool fitImage,
                bool infoTopOfImage,
                bool infoWorkstation,
                bool infoCurrentUser,
                bool infoPrintDate,
                bool infoImageTitle,
                bool infoImageNote,
                bool infoImageCreateDate,
                bool infoImageLastChangeDate)
    {
      InitializeComponent();

      // TODO Drucker-Auswahl hinzufügen

      NumberOfCopiesTextBox.Text = numberOfCopies.ToString();
      PageOrientationComboBox.SelectedIndex = (landscape) ? 1 : 0;
      CenterImageCheckBox.IsChecked = centerImage;
      FitImageCheckBox.IsChecked = fitImage;
      InfoTopOfImageComboBox.SelectedIndex = (infoTopOfImage) ? 0 : 1;
      InfoWorkstationCheckBox.IsChecked = infoWorkstation;
      InfoCurrentUserCheckBox.IsChecked = infoCurrentUser;
      InfoPrintDateCheckBox.IsChecked = infoPrintDate;
      InfoImageTitleCheckBox.IsChecked = infoImageTitle;
      InfoImageNoteCheckBox.IsChecked = infoImageNote;
      InfoImageCreateDateCheckBox.IsChecked = infoImageCreateDate;
      InfoImageLastChangeDateCheckBox.IsChecked = infoImageLastChangeDate;
      
      NumberOfCopiesTextBox.TextChanged += ValidateData;
      PageOrientationComboBox.SelectionChanged += ValidateData;
      InfoTopOfImageComboBox.SelectionChanged += ValidateData;
      ValidateData(null, null);

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

    private void NumberOfCopies_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
    }

    private void ValidateData(object sender, EventArgs e)
    {
      OK.IsEnabled = Validation.IsValid(NumberOfCopiesTextBox) &&
                     Validation.IsValid(PageOrientationComboBox) &&
                     Validation.IsValid(InfoTopOfImageComboBox);
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = true;
    }

  }

}
