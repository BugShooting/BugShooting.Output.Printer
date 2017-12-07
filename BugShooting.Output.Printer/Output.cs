using BS.Plugin.V3.Output;

namespace BugShooting.Output.Printer
{

  public class Output: IOutput 
  {
    
    string name;
    int numberOfCopies;
    bool landscape;
    bool centerImage;
    bool fitImage;
    bool textTopOfImage;
    int textSize;
    string text;

    bool changeSettingBeforePrint;
  
    public Output(string name,
                  int numberOfCopies,
                  bool landscape,
                  bool centerImage,
                  bool fitImage,
                  bool textTopOfImage,
                  int textSize,
                  string text,
                  bool changeSettingBeforePrint)
    {
      this.name = name;
      this.numberOfCopies = numberOfCopies;
      this.landscape = landscape;
      this.centerImage = centerImage;
      this.fitImage = fitImage;
      this.textTopOfImage = textTopOfImage;
      this.textSize = textSize;
      this.text = text;
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

    public bool TextTopOfImage
    {
      get { return textTopOfImage; }
    }
    
    public int TextSize
    {
      get { return textSize; }
    }

    public string Text
    {
      get { return text; }
    }


    public bool ChangeSettingBeforePrint
    {
      get { return changeSettingBeforePrint; }
    }

  }
}
