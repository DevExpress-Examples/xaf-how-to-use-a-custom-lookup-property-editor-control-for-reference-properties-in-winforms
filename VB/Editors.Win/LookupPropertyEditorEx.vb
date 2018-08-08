Imports System
Imports System.Collections
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Model
Imports DevExpress.ExpressApp.Editors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.ExpressApp.Win.Editors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.ExpressApp.Utils

Namespace Editors.Win
    <PropertyEditor(GetType(Object), EditorAliases.LookupPropertyEditor, False)> _
    Public Class LookupPropertyEditorEx
        Inherits DXPropertyEditor
        Implements IComplexViewItem

        Private Const AddButtonTag As String = "AddButtonTag"
        Private Const MinusButtonTag As String = "MinusButtonTag"
        Private Const OpenButtonTag As String = "OpenButtonTag"
        Private helperCore As LookupEditorHelper
        Private lookupObjectView As View

        Public Sub New(ByVal objectType As Type, ByVal item As IModelMemberViewItem)
            MyBase.New(objectType, item)
        End Sub
        Private Sub IComplexViewItem_Setup(ByVal objectSpace As IObjectSpace, ByVal application As XafApplication) Implements IComplexViewItem.Setup
            If Me.helperCore Is Nothing Then
                Me.helperCore = New LookupEditorHelper(application, objectSpace, MemberInfo.MemberTypeInfo, Model)
            End If
            Me.helperCore.SetObjectSpace(objectSpace)
            AddHandler Me.helperCore.ObjectSpace.Reloaded, AddressOf ObjectSpace_Reloaded
        End Sub
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing Then
                    If Me.helperCore IsNot Nothing AndAlso Me.helperCore.ObjectSpace IsNot Nothing Then
                        RemoveHandler Me.helperCore.ObjectSpace.Reloaded, AddressOf ObjectSpace_Reloaded
                        Me.helperCore = Nothing
                    End If
                    lookupObjectView = Nothing
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub
        Protected Overrides Function CreateControlCore() As Object
            Return New LookUpEditEx()
        End Function
        Protected Overrides Function CreateRepositoryItem() As RepositoryItem
            Return New RepositoryItemLookUpEditEx()
        End Function
        Protected Overrides Sub SetupRepositoryItem(ByVal item As RepositoryItem)
            MyBase.SetupRepositoryItem(item)
            Dim properties As RepositoryItemLookUpEditEx = CType(item, RepositoryItemLookUpEditEx)
            properties.Init(DisplayFormat, helperCore)
            AddHandler properties.Enter, AddressOf properties_Enter
            AddHandler properties.ButtonClick, AddressOf properties_ButtonClick
            AddHandler properties.EditValueChanged, AddressOf properties_EditValueChanged
            AddHandler properties.Disposed, AddressOf properties_Disposed

            Dim openButton As New EditorButton(ButtonPredefines.Ellipsis)
            openButton.Tag = OpenButtonTag
            openButton.Enabled = AllowEdit.ResultValue
            openButton.ToolTip = CaptionHelper.GetLocalizedText("Controls/LookupPropertyEditorEx", "OpenButtonToolTip")
            properties.Buttons.Add(openButton)

            Dim addButton As New EditorButton(ButtonPredefines.Plus)
            addButton.Tag = AddButtonTag
            addButton.Enabled = AllowEdit.ResultValue
            addButton.ToolTip = CaptionHelper.GetLocalizedText("Controls/LookupPropertyEditorEx", "AddButtonToolTip")
            properties.Buttons.Add(addButton)

            Dim minusButton As New EditorButton(ButtonPredefines.Minus)
            minusButton.Tag = MinusButtonTag
            minusButton.Enabled = AllowEdit.ResultValue
            minusButton.ToolTip = CaptionHelper.GetLocalizedText("Controls/LookupPropertyEditorEx", "MinusButtonToolTip")
            properties.Buttons.Add(minusButton)
        End Sub
        Private Sub properties_Disposed(ByVal sender As Object, ByVal e As EventArgs)
            Dim properties As RepositoryItemLookUpEditEx = DirectCast(sender, RepositoryItemLookUpEditEx)
            RemoveHandler properties.Enter, AddressOf properties_Enter
            RemoveHandler properties.ButtonClick, AddressOf properties_ButtonClick
            RemoveHandler properties.EditValueChanged, AddressOf properties_EditValueChanged
            RemoveHandler properties.Disposed, AddressOf properties_Disposed
        End Sub
        Private Sub properties_EditValueChanged(ByVal sender As Object, ByVal e As EventArgs)
            Dim lookup As LookUpEditEx = DirectCast(sender, LookUpEditEx)
            UpdateButtons(lookup)
        End Sub
        Private Sub UpdateButtons(ByVal lookup As LookUpEditEx)
            Dim enabled As Boolean = (lookup.EditValue IsNot Nothing) AndAlso (lookup.EditValue IsNot DBNull.Value)
            lookup.Properties.Buttons(1).Enabled = enabled
            lookup.Properties.Buttons(3).Enabled = enabled
        End Sub
        Private Sub properties_Enter(ByVal sender As Object, ByVal e As EventArgs)
            Dim lookup As LookUpEditEx = DirectCast(sender, LookUpEditEx)
            InitializeDataSource(lookup)
            UpdateButtons(lookup)
        End Sub
        Protected Overridable Sub InitializeDataSource(ByVal lookup As LookUpEditEx)
            If lookup IsNot Nothing AndAlso lookup.Properties IsNot Nothing AndAlso lookup.Properties.Helper IsNot Nothing Then
                lookup.InitializeDataSource()
            End If
        End Sub
        Private Sub ObjectSpace_Reloaded(ByVal sender As Object, ByVal e As EventArgs)
            InitializeDataSource(Control)
        End Sub
        Private Sub properties_ButtonClick(ByVal sender As Object, ByVal e As ButtonPressedEventArgs)
            Dim lookup As LookUpEditEx = DirectCast(sender, LookUpEditEx)
            Dim tag As String = Convert.ToString(e.Button.Tag)
            If tag = OpenButtonTag Then
                OpenCurrentObject(lookup)
            End If
            If tag = MinusButtonTag Then
                ClearCurrentObject(lookup)
            End If
            If tag = AddButtonTag Then
                AddNewObject(lookup)
            End If
        End Sub
        Protected Overridable Sub OpenCurrentObject(ByVal lookup As LookUpEditEx)
            Dim svp As New ShowViewParameters()
            Dim openObjectViewObjectSpace As IObjectSpace = lookup.Properties.Helper.Application.CreateObjectSpace()
            Dim targetObject As Object = openObjectViewObjectSpace.GetObject(lookup.EditValue)
            If targetObject IsNot Nothing Then
                Dim committedEventHandler As EventHandler = Sub(s, e)
                    If lookupObjectView IsNot Nothing Then
                        lookup.EditValue = lookup.Properties.Helper.ObjectSpace.GetObject(lookupObjectView.CurrentObject)
                    End If
                End Sub
                Dim disposedEventHandler As EventHandler = Nothing
                disposedEventHandler = Sub(s, e)
                    Dim os As IObjectSpace = DirectCast(s, IObjectSpace)
                    RemoveHandler os.Disposed, disposedEventHandler
                    RemoveHandler os.Committed, committedEventHandler
                End Sub
                AddHandler openObjectViewObjectSpace.Committed, committedEventHandler
                AddHandler openObjectViewObjectSpace.Disposed, disposedEventHandler
                lookupObjectView = lookup.Properties.Helper.Application.CreateDetailView(openObjectViewObjectSpace, targetObject, True)
                svp.CreatedView = lookupObjectView
                lookup.Properties.Helper.Application.ShowViewStrategy.ShowView(svp, New ShowViewSource(Nothing, Nothing))
            End If
        End Sub
        Protected Overridable Sub ClearCurrentObject(ByVal lookup As LookUpEditEx)
            lookup.EditValue = Nothing
            DevExpress.ExpressApp.Win.Core.BindingHelper.EndCurrentEdit(lookup)
        End Sub
        Protected Overridable Sub AddNewObject(ByVal lookup As LookUpEditEx)
            Dim svp As New ShowViewParameters()
            Dim newObjectViewObjectSpace As IObjectSpace = lookup.Properties.Helper.Application.CreateObjectSpace()
            Dim newObject As Object = newObjectViewObjectSpace.CreateObject(lookup.Properties.Helper.LookupObjectTypeInfo.Type)
            lookupObjectView = lookup.Properties.Helper.Application.CreateDetailView(newObjectViewObjectSpace, newObject, True)
            svp.CreatedView = lookupObjectView
            Dim committedEventHandler As EventHandler = Sub(s, e)
                lookup.EditValue = lookup.Properties.Helper.ObjectSpace.GetObject(lookupObjectView.CurrentObject)
                If lookup.Properties.DataSource IsNot Nothing Then
                    DirectCast(lookup.Properties.DataSource, IList).Add(lookup.EditValue)
                End If
            End Sub
            Dim disposedEventHandler As EventHandler = Nothing
            disposedEventHandler = Sub(s, e)
                Dim os As IObjectSpace = DirectCast(s, IObjectSpace)
                RemoveHandler os.Disposed, disposedEventHandler
                RemoveHandler os.Committed, committedEventHandler
            End Sub
            AddHandler newObjectViewObjectSpace.Committed, committedEventHandler
            AddHandler newObjectViewObjectSpace.Disposed, disposedEventHandler
            lookup.Properties.Helper.Application.ShowViewStrategy.ShowView(svp, New ShowViewSource(Nothing, Nothing))
        End Sub
        Public Overrides Sub Refresh()
            RefreshCore(Control)
            MyBase.Refresh()
        End Sub
        Private Sub RefreshCore(ByVal lookup As LookUpEditEx)
            If lookup IsNot Nothing Then
                UpdateButtons(lookup)
                lookup.RefreshEditValue()
                lookup.UpdateDisplayText()
                lookup.Refresh()
            End If
        End Sub
        Public ReadOnly Property Helper() As LookupEditorHelper
            Get
                Return helperCore
            End Get
        End Property
        Public Shadows ReadOnly Property Control() As LookUpEditEx
            Get
                Return CType(MyBase.Control, LookUpEditEx)
            End Get
        End Property
    End Class
End Namespace