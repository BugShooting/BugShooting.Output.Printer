using BS.Plugin.V3.Utilities;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BugShooting.Output.Printer
{
  partial class Edit : Window
  {

    public Edit(Output output)
    {
      InitializeComponent();

      foreach (string attributeReplacement in AttributeHelper.GetAttributeReplacements())
      {
        MenuItem item = new MenuItem();
        item.Header = new TextBlock() { Text = attributeReplacement };
        item.Tag = attributeReplacement;
        item.Click += TextAttributeReplacementItem_Click;
        TextAttributeReplacementList.Items.Add(item);
      }

      NameTextBox.Text = output.Name;
      NumberOfCopiesTextBox.Text = output.NumberOfCopies.ToString();
      PageOrientationComboBox.SelectedIndex = (output.Landscape) ? 1 : 0;
      CenterImageCheckBox.IsChecked = output.CenterImage;
      FitImageCheckBox.IsChecked = output.FitImage;
      TextPositionComboBox.SelectedIndex = (output.TextTopOfImage) ? 0 : 1;
      TextSizeTextBox.Text = output.TextSize.ToString();
      TextTextBox.Text = output.Text;
      ChangeSettingBeforePrintCheckBox.IsChecked = output.ChangeSettingBeforePrint;
      
      NumberOfCopiesTextBox.TextChanged += ValidateData;
      PageOrientationComboBox.SelectionChanged += ValidateData;
      TextPositionComboBox.SelectionChanged += ValidateData;
      TextSizeTextBox.TextChanged += ValidateData;
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

    public bool TextTopOfImage
    {
      get { return TextPositionComboBox.SelectedIndex == 0; }
    }

    public int TextSize
    {
      get { return Convert.ToInt32(TextSizeTextBox.Text); }
    }

    public string Text
    {
      get { return TextTextBox.Text; }
    }

    public bool ChangeSettingBeforePrint
    {
      get { return ChangeSettingBeforePrintCheckBox.IsChecked.Value; }
    }

    private void NumberOfCopies_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
    }

    private void TextSize_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
    }

    private void TextAttributeReplacement_Click(object sender, RoutedEventArgs e)
    {
      TextAttributeReplacement.ContextMenu.IsEnabled = true;
      TextAttributeReplacement.ContextMenu.PlacementTarget = TextAttributeReplacement;
      TextAttributeReplacement.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
      TextAttributeReplacement.ContextMenu.IsOpen = true;
    }

    private void TextAttributeReplacementItem_Click(object sender, RoutedEventArgs e)
    {

      MenuItem item = (MenuItem)sender;

      int selectionStart = TextTextBox.SelectionStart;

      TextTextBox.Text = TextTextBox.Text.Substring(0, TextTextBox.SelectionStart) + item.Tag.ToString() + TextTextBox.Text.Substring(TextTextBox.SelectionStart, TextTextBox.Text.Length - TextTextBox.SelectionStart);

      TextTextBox.SelectionStart = selectionStart + item.Tag.ToString().Length;
      TextTextBox.Focus();

    }

    private void ValidateData(object sender, EventArgs e)
    {
      OK.IsEnabled = Validation.IsValid(NameTextBox) &&
                     Validation.IsValid(PageOrientationComboBox) &&
                     Validation.IsValid(TextPositionComboBox) &&
                     Validation.IsValid(TextSizeTextBox);
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = true;     
    }

  }
}
