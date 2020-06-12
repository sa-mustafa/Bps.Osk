namespace Bps.Osk.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Bps.Osk.ViewModels;

    /// <summary>
    /// Interaction logic for KeyboardView.xaml
    /// </summary>
    public partial class KeyboardView : Window
    {
        #region Fields

        public static readonly DependencyProperty TouchScreenProperty =
            DependencyProperty.RegisterAttached("TouchScreen", typeof(bool),
                typeof(KeyboardView), new UIPropertyMetadata(false, TouchScreenPropertyChanged));

        public static readonly DependencyProperty MaskInputProperty =
            DependencyProperty.RegisterAttached("MaskInput", typeof(bool),
                typeof(KeyboardView), new UIPropertyMetadata(false));

        #endregion

        public KeyboardView()
        {
            InitializeComponent();
        }

        public static bool GetTouchScreen(DependencyObject obj)
        {
            return (bool)obj.GetValue(TouchScreenProperty);
        }

        public static void SetTouchScreen(DependencyObject obj, bool value)
        {
            obj.SetValue(TouchScreenProperty, value);
        }

        public static bool GetMaskInput(DependencyObject obj)
        {
            return (bool)obj.GetValue(MaskInputProperty);
        }

        public static void SetMaskInput(DependencyObject obj, bool value)
        {
            obj.SetValue(MaskInputProperty, value);
        }

        public void SetLocation(Control control)
        {
            if (control == null)
                return;

            var point = control.PointToScreen(new Point(0, control.ActualHeight + 2));
            if (Width + point.X > SystemParameters.VirtualScreenWidth)
                Left = SystemParameters.VirtualScreenWidth - Width;
            else if (point.X < 0)
                Left = 0;
            else
                Left = point.X;
            Top = point.Y;
        }

        static void OnGotFocus(object sender)
        {
            var host = sender as Control;
            Brush lastBackgroundBrush = host.Background;
            Brush lastBorderBrush = host.BorderBrush;
            Thickness lastBorderThickness = host.BorderThickness;

            host.Background = Brushes.Yellow;
            host.BorderBrush = Brushes.Red;
            host.BorderThickness = new Thickness(3);

            var vm = new KeyboardViewModel();
            vm.MaskInput = GetMaskInput(host);
            var view = Bootstrapper.Instance.CreateWindow(vm) as KeyboardView;
            view.SetLocation(host);
            if (view.ShowDialog().Value)
                (host as TextBox).Text = vm.Result;

            host.Background = lastBackgroundBrush;
            host.BorderBrush = lastBorderBrush;
            host.BorderThickness = lastBorderThickness;
            view.Close();
        }

        static void TouchScreenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var host = d as FrameworkElement;
            if (host == null) return;

            host.GotFocus += (s, _) => { OnGotFocus(s); };
            host.PreviewMouseLeftButtonUp += (s,_) => { OnGotFocus(s); };
            host.PreviewMouseRightButtonUp += (s, _) => { OnGotFocus(s); };
            host.PreviewTouchUp += (s, _) => { OnGotFocus(s); };
        }

    }
}