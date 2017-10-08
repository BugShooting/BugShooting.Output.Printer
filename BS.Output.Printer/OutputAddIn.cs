using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace BS.Output.Printer
{
  public class OutputAddIn: V3.OutputAddIn<Output>
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
                                 false, 
                                 true,
                                 false,
                                 false,
                                 false,
                                 false,
                                 false,
                                 false,
                                 false,
                                 true,
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
                          edit.InfoTopOfImage,
                          edit.InfoWorkstation,
                          edit.InfoCurrentUser,
                          edit.InfoPrintDate,
                          edit.InfoImageTitle,
                          edit.InfoImageNote,
                          edit.InfoImageCreateDate,
                          edit.InfoImageLastChangeDate,
                          edit.ChangeSettingBeforePrint);
      }
      else
      {
        return null; 
      }

    }

    protected override OutputValueCollection SerializeOutput(Output Output)
    {

      OutputValueCollection outputValues = new OutputValueCollection();

      outputValues.Add(new OutputValue("Name", Output.Name));
      outputValues.Add(new OutputValue("NumberOfCopies", Output.NumberOfCopies.ToString()));
      outputValues.Add(new OutputValue("Landscape", Output.Landscape.ToString()));
      outputValues.Add(new OutputValue("CenterImage", Output.CenterImage.ToString()));
      outputValues.Add(new OutputValue("FitImage", Output.FitImage.ToString()));
      outputValues.Add(new OutputValue("InfoTopOfImage", Output.InfoTopOfImage.ToString()));
      outputValues.Add(new OutputValue("InfoWorkstation", Output.InfoWorkstation.ToString()));
      outputValues.Add(new OutputValue("InfoCurrentUser", Output.InfoCurrentUser.ToString()));
      outputValues.Add(new OutputValue("InfoPrintDate", Output.InfoPrintDate.ToString()));
      outputValues.Add(new OutputValue("InfoImageTitle", Output.InfoImageTitle.ToString()));
      outputValues.Add(new OutputValue("InfoImageNote", Output.InfoImageNote.ToString()));
      outputValues.Add(new OutputValue("InfoImageCreateDate", Output.InfoImageCreateDate.ToString()));
      outputValues.Add(new OutputValue("InfoImageLastChangeDate", Output.InfoImageLastChangeDate.ToString()));
      outputValues.Add(new OutputValue("ChangeSettingBeforePrint", Output.ChangeSettingBeforePrint.ToString()));

      return outputValues;
      
    }

    protected override Output DeserializeOutput(OutputValueCollection OutputValues)
    {

      return new Output(OutputValues["Name", this.Name].Value,
                        Convert.ToInt32(OutputValues["NumberOfCopies", Convert.ToString(1)].Value),
                        Convert.ToBoolean(OutputValues["Landscape", Convert.ToString(true)].Value),
                        Convert.ToBoolean(OutputValues["CenterImage", Convert.ToString(false)].Value),
                        Convert.ToBoolean(OutputValues["FitImage", Convert.ToString(true)].Value),
                        Convert.ToBoolean(OutputValues["InfoTopOfImage", Convert.ToString(false)].Value),
                        Convert.ToBoolean(OutputValues["InfoWorkstation", Convert.ToString(false)].Value),
                        Convert.ToBoolean(OutputValues["InfoCurrentUser", Convert.ToString(false)].Value),
                        Convert.ToBoolean(OutputValues["InfoPrintDate", Convert.ToString(false)].Value),
                        Convert.ToBoolean(OutputValues["InfoImageTitle", Convert.ToString(false)].Value),
                        Convert.ToBoolean(OutputValues["InfoImageNote", Convert.ToString(false)].Value),
                        Convert.ToBoolean(OutputValues["InfoImageCreateDate", Convert.ToString(false)].Value),
                        Convert.ToBoolean(OutputValues["InfoImageLastChangeDate", Convert.ToString(false)].Value),
                        Convert.ToBoolean(OutputValues["ChangeSettingBeforePrint", Convert.ToString(true)].Value));

    }

    protected override async Task<V3.SendResult> Send(IWin32Window Owner, Output Output, V3.ImageData ImageData)
    {

      try
      {

        using (PrintEngine printEngine = new PrintEngine(ImageData))
        {

          printEngine.NumberOfCopies = Output.NumberOfCopies;
          printEngine.Landscape = Output.Landscape;
          printEngine.CenterImage = Output.CenterImage;
          printEngine.FitImage = Output.FitImage;
          printEngine.InfoTopOfImage = Output.InfoTopOfImage;
          printEngine.InfoWorkstation = Output.InfoWorkstation;
          printEngine.InfoCurrentUser = Output.InfoCurrentUser;
          printEngine.InfoPrintDate = Output.InfoPrintDate;
          printEngine.InfoImageTitle = Output.InfoImageTitle;
          printEngine.InfoImageNote = Output.InfoImageNote;
          printEngine.InfoImageCreateDate = Output.InfoImageCreateDate;
          printEngine.InfoImageLastChangeDate = Output.InfoImageLastChangeDate;

          if (Output.ChangeSettingBeforePrint)
          {

            Send send = new Send(printEngine);

            var ownerHelper = new System.Windows.Interop.WindowInteropHelper(send);
            ownerHelper.Owner = Owner.Handle;

            if (!send.ShowDialog() == true)
            {
              return new V3.SendResult(V3.Result.Canceled);
            }

          }

          printEngine.Print();
          return new V3.SendResult(V3.Result.Success);

        }
        
      }
      catch (Exception ex)
      {
        return new V3.SendResult(V3.Result.Failed, ex.Message);
      }

    }
      
  }
}
