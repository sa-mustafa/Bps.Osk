namespace Bps.Osk.ViewModels
{
    public class KeypadViewModel : Stylet.Screen
    {
        #region Public Properties

        public string Result { get; private set; } = "0";

        #endregion

        public void OnButtonClick(object param)
        {
            string content = string.Empty;
            if (param is System.Windows.Shapes.Path)
                content = "back";
            else
                content = (string)param;

            if (Result == "0") Result = "";

            switch (content)
            {
                case "back":
                    if (Result.Length > 0)
                        Result = Result.Remove(Result.Length - 1);
                    break;
                case "+/-":
                    if (Result.IndexOf('-') == -1)
                        Result = "-" + Result;
                    else
                        Result = Result.TrimStart('-');
                    break;
                case "0":
                    if (Result == "0")
                        return;
                    Result += content;
                    break;
                case ".":
                    if (Result.IndexOf('.') == -1)
                        Result += ".";
                    break;
                case "Esc":
                    //RequestClose(false);
                    break;
                case "Enter":
                    //RequestClose(true);
                    break;
                default:
                    Result += content;
                    break;
            }

            if (Result.Length == 0 || Result == "-")
                Result = "0";

            NotifyOfPropertyChange(nameof(Result));
        }

    }
}
