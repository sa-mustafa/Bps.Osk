namespace Bps.Osk.ViewModels
{
    using System.Windows;
    using System.Windows.Input;

    public class KeyboardViewModel : Stylet.Screen
    {
        #region Fields

        KeyConverter convertor = new KeyConverter();

        #endregion

        #region Public Properties

        public bool AllowEmptyInput { get; set; } = true;
        public string Entry { get; set; } = "Login";
        public bool MaskInput { get; set; }
        public string Result { get; private set; } = string.Empty;
        public int TextCursorPosition { get; set; }

        #endregion

        #region Methods

        void InserNewChar(string content)
        {
            if (string.IsNullOrEmpty(Result))
                Result = content;
            else
                Result = Result.Insert(TextCursorPosition, content);

            OnCursorForward(null);
        }

        public void CursorReset()
        {
            TextCursorPosition = 0;
            NotifyOfPropertyChange(nameof(TextCursorPosition));
        }

        public void OnCursorBackward(object param)
        {
            if (TextCursorPosition == 0)
                return;

            TextCursorPosition--;
            NotifyOfPropertyChange(nameof(TextCursorPosition));
        }

        public void OnCursorForward(object param)
        {
            if (TextCursorPosition == Result.Length)
                return;

            TextCursorPosition++;
            NotifyOfPropertyChange(nameof(TextCursorPosition));
        }

        public void OnBackspaceClick(object param)
        {
            if (string.IsNullOrEmpty(Result) || TextCursorPosition <= 0)
                return;

            Result = Result.Remove(TextCursorPosition - 1, 1);
            OnCursorBackward(null);
            NotifyOfPropertyChange(nameof(Result));
        }

        public void OnButtonClick(object param)
        {
            string content = (string)param;
            switch (content)
            {
                case "CLR":
                    Result = string.Empty;
                    CursorReset();
                    break;
                case "Esc":
                    RequestClose(false);
                    break;
                case "Enter":
                    RequestClose(true);
                    break;
                default:
                    InserNewChar(content);
                    break;
            }

            NotifyOfPropertyChange(nameof(Result));
        }

        public void OnEnterClick(object param)
        {
            if (!AllowEmptyInput && string.IsNullOrEmpty(Result))
                MessageBox.Show(Entry + " is empty!", "Warning", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            else
            {
                var window = param as Window;
                window.DialogResult = true;
                window.Close();
            }
        }

        public void OnEscapeClick(object param)
        {
            Result = string.Empty;
            var window = param as Window;
            window.DialogResult = false;
            window.Close();
        }

        public void OnKeyUp(KeyEventArgs e)
        {
            var mappedChar = convertor.ConvertToString(e.Key);
            var window = View as Window;

            switch (e.Key)
            {
                case Key.Clear:
                    Result = string.Empty;
                    CursorReset();
                    break;
                case Key.Left:
                    OnCursorBackward(null);
                    break;
                case Key.Right:
                    OnCursorForward(null);
                    break;
                case Key.Back:
                    OnBackspaceClick(null);
                    break;
                case Key.Enter:
                    window.DialogResult = true;
                    window.Close();
                    //RequestClose(true);
                    break;
                case Key.Escape:
                    window.DialogResult = false;
                    window.Close();
                    //RequestClose(false);
                    break;
                case Key.OemPlus:
                    InserNewChar("+");
                    break;
                case Key.OemMinus:
                    InserNewChar("-");
                    break;
                case Key.Decimal:
                case Key.OemPeriod:
                    InserNewChar(".");
                    break;
                default:
                    if (e.Key >= Key.D0 && e.Key <= Key.Z)
                    {
                        InserNewChar(mappedChar);
                    }
                    else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                    {
                        mappedChar = mappedChar.Substring(6);
                        InserNewChar(mappedChar);
                    }
                    else
                    {
                        e.Handled = true;
                    }
                    break;
            }
            NotifyOfPropertyChange(nameof(Result));
        }

        #endregion

    }

    /// <summary>
    /// Adds caret tracking capability to a text box. 
    /// CaretIndex is a CLR property, not a DependencyPropery;
    /// so it does not work with binding.
    /// </summary>
    /// <seealso cref="DependencyObject" />
    public class TextCaretBehavior : DependencyObject
    {
        public static readonly DependencyProperty CursorPositionProperty =
            DependencyProperty.RegisterAttached("CursorPosition", typeof(int), typeof(TextCaretBehavior),
                new FrameworkPropertyMetadata(int.MaxValue, CursorPositionChangedCallback)
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged
                });

        public static int GetCursorPosition(DependencyObject obj)
        {
            return (int)obj.GetValue(CursorPositionProperty);
        }

        public static void SetCursorPosition(DependencyObject obj, int value)
        {
            obj.SetValue(CursorPositionProperty, value);
        }

        public static void CursorPositionChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textbox = d as System.Windows.Controls.TextBox;
            if (textbox == null)
                return;

            // Only add the event once, when the value is default.
            if ((int)e.OldValue == int.MaxValue)
                textbox.TextChanged += OnTextChanged;

            // Unfortunately, the caret index gets reset when the control's text changes.
            // So, we should set the caret index after a text change (in the OnTextChanged).
            textbox.Tag = (int)e.NewValue;
            textbox.CaretIndex = (int)e.NewValue;
            // Set the focus on the text box so that the caret can be seen.
            textbox.Focus();
        }

        private static void OnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var textbox = sender as System.Windows.Controls.TextBox;
            if (textbox == null)
                return;

            // The caret index should be set after a text change, or the caret goes to the starting position.
            textbox.CaretIndex = (int)textbox.Tag;
        }

    }

}