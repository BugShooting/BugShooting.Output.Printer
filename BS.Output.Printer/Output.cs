namespace BS.Output.Printer
{

  public class Output: IOutput 
  {
    
    string name;
    bool showPrinterDialog;
  
    public Output(string name, 
                  bool showPrinterDialog)
    {
      this.name = name;
      this.showPrinterDialog = showPrinterDialog;
    }
    
    public string Name
    {
      get { return name; }
    }

    public string Information
    {
      get { return string.Empty; }
    }

    public bool ShowPrinterDialog
    {
      get { return showPrinterDialog; }
    }
 
  }
}
