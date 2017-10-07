namespace BS.Output.Printer
{

  public class Output: IOutput 
  {
    
    string name;
    int numberOfCopies;
    bool landscape;
    bool centerImage;
    bool fitImage;
    bool infoTopOfImage;
    bool infoWorkstation;
    bool infoCurrentUser;
    bool infoPrintDate;
    bool infoImageTitle;
    bool infoImageNote;
    bool infoImageCreateDate;
    bool infoImageLastChangeDate;
    
    bool changeSettingBeforePrint;
  
    public Output(string name,
                  int numberOfCopies,
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
                  bool infoImageLastChangeDate,
                  bool changeSettingBeforePrint)
    {
      this.name = name;
      this.numberOfCopies = numberOfCopies;
      this.landscape = landscape;
      this.centerImage = centerImage;
      this.fitImage = fitImage;
      this.infoTopOfImage = infoTopOfImage;
      this.infoWorkstation = infoWorkstation;
      this.infoCurrentUser = infoCurrentUser;
      this.infoPrintDate = infoPrintDate;
      this.infoImageTitle = infoImageTitle;
      this.infoImageNote = infoImageNote;
      this.infoImageCreateDate = infoImageCreateDate;
      this.infoImageLastChangeDate = infoImageLastChangeDate;
      this.changeSettingBeforePrint = changeSettingBeforePrint;
    }
    
    public string Name
    {
      get { return name; }
    }

    public string Information
    {
      get { return string.Empty; }
    }

    public int NumberOfCopies
    {
      get { return numberOfCopies; }
    }

    public bool Landscape
    {
      get { return landscape; }
    }

    public bool CenterImage
    {
      get { return centerImage; }
    }

    public bool FitImage
    {
      get { return fitImage; }
    }

    public bool InfoTopOfImage
    {
      get { return infoTopOfImage; }
    }

    public bool InfoWorkstation
    {
      get { return infoWorkstation; }
    }

    public bool InfoCurrentUser
    {
      get { return infoCurrentUser; }
    }

    public bool InfoPrintDate
    {
      get { return infoPrintDate; }
    }

    public bool InfoImageTitle
    {
      get { return infoImageTitle; }
    }

    public bool InfoImageNote
    {
      get { return infoImageNote; }
    }

    public bool InfoImageCreateDate
    {
      get { return infoImageCreateDate; }
    }

    public bool InfoImageLastChangeDate
    {
      get { return infoImageLastChangeDate; }
    }

    public bool ChangeSettingBeforePrint
    {
      get { return changeSettingBeforePrint; }
    }

  }
}
