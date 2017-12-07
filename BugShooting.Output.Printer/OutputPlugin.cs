using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using BS.Plugin.V3.Output;
using BS.Plugin.V3.Common;
using BS.Plugin.V3.Utilities;

namespace BugShooting.Output.Printer
{
  public class OutputPlugin: OutputPlugin<Output>
  {

    protected override string Name
    {
      get { return "Printer"; }
    }

    protected override Image Image64
    {
      get  { return Properties.Resources.logo_64; }
    }

    protected override Image Image16
    {
      get { return Properties.Resources.logo_16 ; }
    }

    protected override bool Editable
    {
      get { return true; }
    }

    protected override string Description
    {
      get { return "Send screenshots to printer."; }
    }
    
    protected override Output CreateOutput(IWin32Window Owner)
    {

      Output output = new Output(Name, 
                                 1,
                                 true, 
                                 true, 
                                 true,
                                 false,
                                 10,
                                 string.Empty,
                                 true);

      return EditOutput(Owner, output);

    }

    protected override Output EditOutput(IWin32Window Owner, Output Output)
    {

      Edit edit = new Edit(Output);

      var ownerHelper = new System.Windows.Interop.WindowInteropHelper(edit);
      ownerHelper.Owner = Owner.Handle;
      
      if (edit.ShowDialog() == true) {

        return new Output(edit.OutputName,
                          edit.NumberOfCopies,
                          edit.Landscape,
                          edit.CenterImage,
                          edit.FitImage,
                          edit.TextTopOfImage,
                          edit.TextSize,
                          edit.Text,
                          edit.ChangeSettingBeforePrint);
      }
      else
      {
        return null; 
      }

    }

    protected override OutputValues SerializeOutput(Output Output)
    {

      OutputValues outputValues = new OutputValues();

      outputValues.Add("Name", Output.Name);
      outputValues.Add("NumberOfCopies", Output.NumberOfCopies.ToString());
      outputValues.Add("Landscape", Output.Landscape.ToString());
      outputValues.Add("CenterImage", Output.CenterImage.ToString());
      outputValues.Add("FitImage", Output.FitImage.ToString());
      outputValues.Add("TextTopOfImage", Output.TextTopOfImage.ToString());
      outputValues.Add("InfoTextSize", Output.TextSize.ToString());
      outputValues.Add("Text", Output.Text);
      outputValues.Add("ChangeSettingBeforePrint", Output.ChangeSettingBeforePrint.ToString());

      return outputValues;
      
    }

    protected override Output DeserializeOutput(OutputValues OutputValues)
    {

      return new Output(OutputValues["Name", this.Name],
                        Convert.ToInt32(OutputValues["NumberOfCopies", Convert.ToString(1)]),
                        Convert.ToBoolean(OutputValues["Landscape", Convert.ToString(true)]),
                        Convert.ToBoolean(OutputValues["CenterImage", Convert.ToString(false)]),
                        Convert.ToBoolean(OutputValues["FitImage", Convert.ToString(true)]),
                        Convert.ToBoolean(OutputValues["TextTopOfImage", Convert.ToString(false)]),
                        Convert.ToInt32(OutputValues["TextSize", Convert.ToString(10)]),
                        OutputValues["Text", string.Empty],
                        Convert.ToBoolean(OutputValues["ChangeSettingBeforePrint", Convert.ToString(true)]));

    }

    protected override async Task<SendResult> Send(IWin32Window Owner, Output Output, ImageData ImageData)
    {

      try
      {

        using (PrintEngine printEngine = new PrintEngine(ImageData))
        {

          printEngine.NumberOfCopies = Output.NumberOfCopies;
          printEngine.Landscape = Output.Landscape;
          printEngine.CenterImage = Output.CenterImage;
          printEngine.FitImage = Output.FitImage;
          printEngine.TextTopOfImage = Output.TextTopOfImage;
          printEngine.TextSize = Output.TextSize;
          printEngine.Text = AttributeHelper.ReplaceAttributes(Output.Text, ImageData);

          if (Output.ChangeSettingBeforePrint)
          {

            Send send = new Send(printEngine);

            var ownerHelper = new System.Windows.Interop.WindowInteropHelper(send);
            ownerHelper.Owner = Owner.Handle;

            if (!send.ShowDialog() == true)
            {
              return new SendResult(Result.Canceled);
            }

          }
                   
          printEngine.Print();
          return new SendResult(Result.Success);

        }
        
      }
      catch (Exception ex)
      {
        return new SendResult(Result.Failed, ex.Message);
      }

    }
      
  }
}
