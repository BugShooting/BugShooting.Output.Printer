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
      ShowPrinterDialogCheckBox.IsChecked = output.ShowPrinterDialog;

      NameTextBox.TextChanged += ValidateData;
      ValidateData(null, null);

    }
     
    public string OutputName
    {
      get { return NameTextBox.Text; }
    }
    
    public bool ShowPrinterDialog
    {
      get { return ShowPrinterDialogCheckBox.IsChecked.Value; }
    }

    private void NumberOfCopies_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
    }

    private void ValidateData(object sender, EventArgs e)
    {
      OK.IsEnabled = Validation.IsValid(NameTextBox);
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = true;     
    }

  }
}
