using System;
using DevExpress.Utils;
using DevExpress.Accessibility;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Editors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.ExpressApp.Win.Core;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;

namespace Editors.Win {
    public class RepositoryItemLookUpEditEx : DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit, ILookupEditRepositoryItem {
        internal const string EditorName = "LookUpEditEx";
        private LookupEditorHelper helper;
        static RepositoryItemLookUpEditEx() {
            Register();
        }
        public RepositoryItemLookUpEditEx() { }
        public static void Register() {
            if(!EditorRegistrationInfo.Default.Editors.Contains(EditorName)) {
                EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName, typeof(LookUpEditEx),
                    typeof(RepositoryItemLookUpEditEx), typeof(LookUpEditViewInfo),
                    new ButtonEditPainter(), true, EditImageIndexes.LookUpEdit, typeof(PopupEditAccessible)));
            }
        }
        public override string EditorTypeName { get { return EditorName; } }
        public new LookUpEditEx OwnerEdit {
            get { return (LookUpEditEx)base.OwnerEdit; }
        }
        public LookupEditorHelper Helper {
            get { return helper; }
        }
        public void Init(string displayFormat, LookupEditorHelper helper) {
            this.helper = helper;
            BeginUpdate();
            DisplayFormat.FormatString = displayFormat;
            DisplayFormat.FormatType = FormatType.Custom;
            EditFormat.FormatString = displayFormat;
            EditFormat.FormatType = FormatType.Custom;
            TextEditStyle = TextEditStyles.Standard;
            ExportMode = ExportMode.DisplayText;
            DisplayMember = ((ILookupEditRepositoryItem)this).DisplayMember;
            ValueMember = null;
            ShowHeader = false;
            DropDownRows = helper.SmallCollectionItemCount;
            SearchMode = SearchMode.AutoFilter;
            NullText = CaptionHelper.NullValueText;
            AllowNullInput = DefaultBoolean.True;
            EndUpdate();
        }
        public override string GetDisplayText(FormatInfo format, object editValue) {
            string result = base.GetDisplayText(format, editValue);
            if(string.IsNullOrEmpty(result) && editValue != null && helper != null) {
                result = helper.GetDisplayText(editValue, NullText, format.FormatString);
            }
            return result;
        }
        public override void Assign(RepositoryItem item) {
            RepositoryItemLookUpEditEx source = (RepositoryItemLookUpEditEx)item;
            try {
                base.Assign(source);
            }
            catch { }
            helper = source.helper;
            ThrowExceptionOnInvalidLookUpEditValueType = source.ThrowExceptionOnInvalidLookUpEditValueType;
        }
        #region ILookupEditRepositoryItem Members
        Type ILookupEditRepositoryItem.LookupObjectType {
            get { return helper.LookupObjectType; }
        }
        string ILookupEditRepositoryItem.DisplayMember {
            get { return helper.DisplayMember != null ? helper.DisplayMember.Name : string.Empty; }
        }
        #endregion
    }
}
