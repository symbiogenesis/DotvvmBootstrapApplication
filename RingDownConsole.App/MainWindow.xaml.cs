using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RingDownConsole.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Prep stuff needed to remove close button on window
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        public MainWindow()
        {
            InitializeComponent();
            Loaded += ToolWindow_Loaded;
            Style = (Style) FindResource(typeof(Window));
            Application.Current.Exit += new ExitEventHandler(Current_Exit);

            //Icon = ConvertToBitmapSource((Path)this.Resources["CommunicationErrorIcon"]);
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            myNotifyIcon.Dispose();
        }

        public BitmapSource ConvertToBitmapSource(UIElement element)
        {
            var target = new RenderTargetBitmap((int) (element.RenderSize.Width), (int) (element.RenderSize.Height), 96, 96, PixelFormats.Pbgra32);
            var brush = new VisualBrush(element);

            var visual = new DrawingVisual();
            var drawingContext = visual.RenderOpen();


            drawingContext.DrawRectangle(brush, null, new Rect(new Point(0, 0),
                new Point(element.RenderSize.Width, element.RenderSize.Height)));

            drawingContext.Close();

            target.Render(visual);

            return target;
        }
    }
}
