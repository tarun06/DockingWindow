using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DockingLib
{
    /// <summary>
    /// Interaction logic for DockableContent.xaml
    /// </summary>
    public partial class DockableContent : UserControl, IDockable
    {
        private Control _owner;

        public DockableContent(string title, Control owner, Action<DockableContent> removeElement)
        {
            InitializeComponent();
            DataContext = this;
            ShowHeader = true;
            Title = title;
            _owner = owner;
            _removeElement = removeElement;
        }

        public Action<DockableContent> _removeElement { get; set; }

        public bool ShowHeader
        {
            get
            {
                return PaneHeader.Visibility == Visibility.Visible;
            }
            set
            {
                if (value)
                    PaneHeader.Visibility = Visibility.Visible;
                else
                    PaneHeader.Visibility = Visibility.Collapsed;
            }
        }

        public string Title { get; set; }

        private FloatingWindow? _window { get; set; }

        public bool Drag(FloatingWindow floatingWindow, Point point, Point offset)
        {
            if (!IsMouseCaptured)
            {
                var window = Window.GetWindow(_owner);
                if (window == null) return false;

                floatingWindow.Owner = window;
                floatingWindow.Show();
                floatingWindow.MouseDown -= FloatingWindow_MouseDown;
                floatingWindow.MouseDown += FloatingWindow_MouseDown;
                return true;
            }
            return false;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (!PaneHeader.IsMouseCaptured)
            {
                PaneHeader.CaptureMouse();
            }
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            PaneHeader.ReleaseMouseCapture();
            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (PaneHeader.IsMouseCaptured)
            {
                PaneHeader.ReleaseMouseCapture();

                _window = new FloatingWindow(this);
                this.ShowHeader = false;

                _removeElement?.Invoke(this);

                if (_window != null)
                {
                    var pos = Mouse.GetPosition(PaneHeader);
                    Drag(_window, pos, e.GetPosition(PaneHeader));
                }
            }
        }

        private void FloatingWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is FrameworkElement frameworkElement)
            {
                DragDropEffects dragDropEffects = DragDropEffects.None;
                try
                {
                    if (_window != null)
                    {
                        ShowHeader = true;
                        _window.Content = null;
                    }
                    dragDropEffects = DragDrop.DoDragDrop(this,
                        new DataObject(DataFormats.Serializable, this),
                        DragDropEffects.Move);
                }
                finally
                {
                    if (dragDropEffects == DragDropEffects.Move && _window != null)
                    {
                        _window.Close();
                        _window = null;
                    }
                }
            }
        }
    }
}