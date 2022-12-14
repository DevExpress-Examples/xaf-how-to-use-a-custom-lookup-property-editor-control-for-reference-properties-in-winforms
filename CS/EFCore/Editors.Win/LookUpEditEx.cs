using System;
using System.ComponentModel;
using DevExpress.ExpressApp.Win.Core;
using DevExpress.ExpressApp.Localization;

namespace Editors.Win {
    [ToolboxItem(false)]
    public class LookUpEditEx : DevExpress.XtraEditors.LookUpEdit, IGridInplaceEdit {
        private object gridEditingObject;
        static LookUpEditEx() { RepositoryItemLookUpEditEx.Register(); }
        public LookUpEditEx() {
            base.DataBindings.CollectionChanged += DataBindings_CollectionChanged;
        }
        protected override void Dispose(bool disposing) {
            if(disposing) {
                base.DataBindings.CollectionChanged -= DataBindings_CollectionChanged;
            }
            base.Dispose(disposing);
        }
        public override string EditorTypeName {
            get { return RepositoryItemLookUpEditEx.EditorName; }
        }
        public new RepositoryItemLookUpEditEx Properties {
            get { return (RepositoryItemLookUpEditEx)base.Properties; }
        }
        public override object EditValue {
            get { return base.EditValue; }
            set {
                if(value != DBNull.Value && value != null) {
                    if(!Properties.Helper.LookupObjectType.IsInstanceOfType(value)) {
                        if(Properties.ThrowExceptionOnInvalidLookUpEditValueType) {
                            throw new InvalidCastException(SystemExceptionLocalizer.GetExceptionMessage(ExceptionId.UnableToCast,
                                value.GetType(),
                                Properties.Helper.LookupObjectType));
                        }
                        else {
                            base.EditValue = null;
                            return;
                        }
                    }
                }
                base.EditValue = value;
            }
        }
        public object FindEditingObject() {
            return BindingHelper.FindEditingObject(this);
        }
        private void OnEditingObjectChanged() {
            InitializeDataSource();
            if(FindEditingObject() == null && EditValue != null) {
                EditValue = null;
            }
        }
        public void InitializeDataSource() {
            if(Properties != null && Properties.Helper != null) {
                Properties.DataSource = Properties.Helper.CreateCollectionSource(FindEditingObject()).List;
            }
        }
        private void DataBindings_CollectionChanged(object sender, CollectionChangeEventArgs e) {
            OnEditingObjectChanged();
        }
        public new void UpdateDisplayText() {
            base.UpdateDisplayText();
            base.Refresh();
        }

        #region IGridInplaceEdit Members
        System.Windows.Forms.ControlBindingsCollection IGridInplaceEdit.DataBindings {
            get { return base.DataBindings; }
        }
        object IGridInplaceEdit.GridEditingObject {
            get { return gridEditingObject; }
            set {
                if(gridEditingObject != value) {
                    gridEditingObject = value;
                    OnEditingObjectChanged();
                }
            }
        }
        #endregion
    }
}
