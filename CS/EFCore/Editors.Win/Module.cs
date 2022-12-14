using System;
using DevExpress.ExpressApp;
using System.ComponentModel;

namespace Editors.Win {
    [ToolboxItem(true)]
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public sealed partial class EditorsWindowsFormsModule : ModuleBase {
        public EditorsWindowsFormsModule() {
            InitializeComponent();
        }
    }
}
