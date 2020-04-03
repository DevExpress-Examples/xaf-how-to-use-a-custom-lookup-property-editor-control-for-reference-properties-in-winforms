using System;
using System.Collections;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.XtraEditors.Controls;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors.Repository;
using DevExpress.ExpressApp.Utils;

namespace Editors.Win {
    [PropertyEditor(typeof(object), EditorAliases.LookupPropertyEditor, false)]
    public class LookupPropertyEditorEx : DXPropertyEditor, IComplexViewItem {
        private const string AddButtonTag = "AddButtonTag";
        private const string MinusButtonTag = "MinusButtonTag";
        private const string OpenButtonTag = "OpenButtonTag";
        private LookupEditorHelper helperCore;
        private View lookupObjectView;

        public LookupPropertyEditorEx(Type objectType, IModelMemberViewItem item)
            : base(objectType, item) {
        }
        void IComplexViewItem.Setup(IObjectSpace objectSpace, XafApplication application) {
            if(this.helperCore == null) {
                this.helperCore = new LookupEditorHelper(application, objectSpace, MemberInfo.MemberTypeInfo, Model);
            }
            this.helperCore.SetObjectSpace(objectSpace);
            this.helperCore.ObjectSpace.Reloaded += ObjectSpace_Reloaded;
        }
        protected override void Dispose(bool disposing) {
            try {
                if(disposing) {
                    if(this.helperCore != null && this.helperCore.ObjectSpace != null) {
                        this.helperCore.ObjectSpace.Reloaded -= ObjectSpace_Reloaded;
                        this.helperCore = null;
                    }
                    lookupObjectView = null;
                }
            }
            finally {
                base.Dispose(disposing);
            }
        }
        protected override object CreateControlCore() {
            return new LookUpEditEx();
        }
        protected override RepositoryItem CreateRepositoryItem() {
            return new RepositoryItemLookUpEditEx();
        }
        protected override void SetupRepositoryItem(RepositoryItem item) {
            base.SetupRepositoryItem(item);
            RepositoryItemLookUpEditEx properties = (RepositoryItemLookUpEditEx)item;
            properties.Init(DisplayFormat, helperCore);
            properties.Enter += properties_Enter;
            properties.ButtonClick += properties_ButtonClick;
            properties.EditValueChanged += properties_EditValueChanged;
            properties.Closed += properties_Closed;
            properties.Disposed += properties_Disposed;

            EditorButton openButton = new EditorButton(ButtonPredefines.Ellipsis);
            openButton.Tag = OpenButtonTag;
            openButton.Enabled = AllowEdit.ResultValue;
            openButton.ToolTip = CaptionHelper.GetLocalizedText("Controls/LookupPropertyEditorEx", "OpenButtonToolTip");
            properties.Buttons.Add(openButton);

            EditorButton addButton = new EditorButton(ButtonPredefines.Plus);
            addButton.Tag = AddButtonTag;
            addButton.Enabled = AllowEdit.ResultValue;
            addButton.ToolTip = CaptionHelper.GetLocalizedText("Controls/LookupPropertyEditorEx", "AddButtonToolTip");
            properties.Buttons.Add(addButton);

            EditorButton minusButton = new EditorButton(ButtonPredefines.Minus);
            minusButton.Tag = MinusButtonTag;
            minusButton.Enabled = AllowEdit.ResultValue;
            minusButton.ToolTip = CaptionHelper.GetLocalizedText("Controls/LookupPropertyEditorEx", "MinusButtonToolTip");
            properties.Buttons.Add(minusButton);
        }
        private void properties_Disposed(object sender, EventArgs e) {
            RepositoryItemLookUpEditEx properties = (RepositoryItemLookUpEditEx)sender;
            properties.Enter -= properties_Enter;
            properties.ButtonClick -= properties_ButtonClick;
            properties.EditValueChanged -= properties_EditValueChanged;
            properties.Disposed -= properties_Disposed;
            properties.Closed -= properties_Closed;
        }
        private void properties_EditValueChanged(object sender, EventArgs e) {
            LookUpEditEx lookup = (LookUpEditEx)sender;
            UpdateButtons(lookup);
        }
        private void properties_Closed(object sender, ClosedEventArgs e) {
            LookUpEditEx lookup = (LookUpEditEx)sender;
            UpdateButtons(lookup);
        }
        private void properties_Enter(object sender, EventArgs e) {
            LookUpEditEx lookup = (LookUpEditEx)sender;
            InitializeDataSource(lookup);
            UpdateButtons(lookup);
        }
        private void UpdateButtons(LookUpEditEx lookup) {
            if (!lookup.IsPopupOpen) {
                bool enabled = (lookup.EditValue != null) && (lookup.EditValue != DBNull.Value);
                lookup.Properties.Buttons[1].Enabled = enabled;
                lookup.Properties.Buttons[3].Enabled = enabled;
            }
        }
        protected virtual void InitializeDataSource(LookUpEditEx lookup) {
            if(lookup != null && lookup.Properties != null && lookup.Properties.Helper != null) {
                lookup.InitializeDataSource();
            }
        }
        private void ObjectSpace_Reloaded(object sender, EventArgs e) {
            InitializeDataSource(Control);
        }
        private void properties_ButtonClick(object sender, ButtonPressedEventArgs e) {
            LookUpEditEx lookup = (LookUpEditEx)sender;
            string tag = Convert.ToString(e.Button.Tag);
            if(tag == OpenButtonTag) {
                OpenCurrentObject(lookup);
            }
            if(tag == MinusButtonTag) {
                ClearCurrentObject(lookup);
            }
            if(tag == AddButtonTag) {
                AddNewObject(lookup);
            }
        }
        protected virtual void OpenCurrentObject(LookUpEditEx lookup) {
            ShowViewParameters svp = new ShowViewParameters();
            IObjectSpace openObjectViewObjectSpace = lookup.Properties.Helper.Application.CreateObjectSpace(lookup.Properties.Helper.LookupObjectTypeInfo.Type);
            object targetObject = openObjectViewObjectSpace.GetObject(lookup.EditValue);
            if(targetObject != null) {
                EventHandler committedEventHandler = (s, e) => {
                    if(lookupObjectView != null) {
                        lookup.EditValue = lookup.Properties.Helper.ObjectSpace.GetObject(lookupObjectView.CurrentObject);
                    }
                };
                EventHandler disposedEventHandler = null;
                disposedEventHandler = (s, e) => {
                    IObjectSpace os = (IObjectSpace)s;
                    os.Disposed -= disposedEventHandler;
                    os.Committed -= committedEventHandler;
                };
                openObjectViewObjectSpace.Committed += committedEventHandler;
                openObjectViewObjectSpace.Disposed += disposedEventHandler;
                lookupObjectView = lookup.Properties.Helper.Application.CreateDetailView(openObjectViewObjectSpace, targetObject, true);
                svp.CreatedView = lookupObjectView;
                lookup.Properties.Helper.Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
            }
        }
        protected virtual void ClearCurrentObject(LookUpEditEx lookup) {
            lookup.EditValue = null;
            DevExpress.ExpressApp.Win.Core.BindingHelper.EndCurrentEdit(lookup);
        }
        protected virtual void AddNewObject(LookUpEditEx lookup) {
            ShowViewParameters svp = new ShowViewParameters();
            IObjectSpace newObjectViewObjectSpace = lookup.Properties.Helper.Application.CreateObjectSpace(lookup.Properties.Helper.LookupObjectTypeInfo.Type);
            object newObject = newObjectViewObjectSpace.CreateObject(lookup.Properties.Helper.LookupObjectTypeInfo.Type);
            lookupObjectView = lookup.Properties.Helper.Application.CreateDetailView(newObjectViewObjectSpace, newObject, true);
            svp.CreatedView = lookupObjectView;
            EventHandler committedEventHandler = (s, e) => {
                lookup.EditValue = lookup.Properties.Helper.ObjectSpace.GetObject(lookupObjectView.CurrentObject);
                if(lookup.Properties.DataSource != null) {
                    ((IList)lookup.Properties.DataSource).Add(lookup.EditValue);
                }
            };
            EventHandler disposedEventHandler = null;
            disposedEventHandler = (s, e) => {
                IObjectSpace os = (IObjectSpace)s;
                os.Disposed -= disposedEventHandler;
                os.Committed -= committedEventHandler;
            };
            newObjectViewObjectSpace.Committed += committedEventHandler;
            newObjectViewObjectSpace.Disposed += disposedEventHandler;
            lookup.Properties.Helper.Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }
        public LookupEditorHelper Helper {
            get { return helperCore; }
        }
        public new LookUpEditEx Control {
            get { return (LookUpEditEx)base.Control; }
        }
    }
}
