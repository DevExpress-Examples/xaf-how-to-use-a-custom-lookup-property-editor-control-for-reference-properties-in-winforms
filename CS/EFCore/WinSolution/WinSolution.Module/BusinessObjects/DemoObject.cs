using System;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp;

namespace WinSolution.Module {
    [DefaultClassOptions]
    [DefaultListViewOptions(true, NewItemRowPosition.Top)]
    public class DemoObject : BaseObject {
        public virtual string Name { get; set; }
        [ImmediatePostData]
        [VisibleInListView(true)]
        public virtual DemoLookupObject LookupProperty { get; set; }
    }
}
