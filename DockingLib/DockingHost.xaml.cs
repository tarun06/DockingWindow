using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DockingLib
{
    /// <summary>
    /// Interaction logic for DockingHost.xaml
    /// </summary>
    public partial class DockingHost : UserControl
    {
        private readonly Dictionary<DockableBase, IDockable> _dockableContentDictionary;

        public DockingHost()
        {
            InitializeComponent();

            _dockableContentDictionary = new Dictionary<DockableBase, IDockable>();
        }

        public void Add(DockableBase control, Dock dock)
        {
            if (this._dockableContentDictionary.ContainsKey(control))
                return;

            var uc = new DockableContent(control.Title, this, RemoveControl);
            uc.dockableContentPresenter.Content = control;
            _dockableContentDictionary.Add(control, uc);

            AddControl(dock, uc);
        }

        private void AddControl(Dock dock, DockableContent uc)
        {
            switch (dock)
            {
                case Dock.Left:
                    this.LeftStkPanel.Children.Add(uc);
                    break;

                case Dock.Right:
                    this.RightStkPanel.Children.Add(uc);
                    break;

                case Dock.Top:
                    this.TopStkPanel.Children.Add(uc);
                    break;

                case Dock.Bottom:
                    this.BottomStkPanel.Children.Add(uc);
                    break;
            }
        }

        private void AddControl(Panel panel, DockableContent dockableContent)
        {
            if (dockableContent is not null)
                panel.Children.Add(dockableContent);
        }

        private void Remove(DockableContent dockableContent)
        {
            if (this.LeftStkPanel.Children.Contains(dockableContent))
            {
                this.LeftStkPanel.Children.Remove(dockableContent);
                return;
            }

            if (this.RightStkPanel.Children.Contains(dockableContent))
            {
                this.RightStkPanel.Children.Remove(dockableContent);
                return;
            }
            if (this.TopStkPanel.Children.Contains(dockableContent))
            {
                this.TopStkPanel.Children.Remove(dockableContent);
                return;
            }
            if (this.BottomStkPanel.Children.Contains(dockableContent))
            {
                this.BottomStkPanel.Children.Remove(dockableContent);
                return;
            }
        }

        private void RemoveControl(DockableContent dockableContent)
        {
            var dockableBase = _dockableContentDictionary.FirstOrDefault(x => x.Value == dockableContent).Key;
            if (dockableBase is not null)
            {
                Remove(dockableContent);
                _dockableContentDictionary.Remove(dockableBase);
            }
        }

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);

            if (sender is Panel panel &&
                data is DockableContent dockableContent &&
                dockableContent.dockableContentPresenter.Content is DockableBase dockableBase)
            {
                AddControl(panel, dockableContent);
                _dockableContentDictionary[dockableBase] = dockableContent;
            }
        }
    }
}