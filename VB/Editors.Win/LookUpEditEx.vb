Imports System.ComponentModel
Imports DevExpress.ExpressApp.Win.Core
Imports DevExpress.ExpressApp.Localization

Namespace Editors.Win
	<ToolboxItem(False)> _
	Public Class LookUpEditEx
		Inherits DevExpress.XtraEditors.LookUpEdit
		Implements IGridInplaceEdit
		Private gridEditingObject_Renamed As Object
		Shared Sub New()
			RepositoryItemLookUpEditEx.Register()
		End Sub
		Public Sub New()
			AddHandler MyBase.DataBindings.CollectionChanged, AddressOf DataBindings_CollectionChanged
		End Sub
		Protected Overrides Overloads Sub Dispose(ByVal disposing As Boolean)
			If disposing Then
				RemoveHandler MyBase.DataBindings.CollectionChanged, AddressOf DataBindings_CollectionChanged
			End If
			MyBase.Dispose(disposing)
		End Sub
		Public Overrides ReadOnly Property EditorTypeName() As String
			Get
				Return RepositoryItemLookUpEditEx.EditorName
			End Get
		End Property
		Public Shadows ReadOnly Property Properties() As RepositoryItemLookUpEditEx
			Get
				Return CType(MyBase.Properties, RepositoryItemLookUpEditEx)
			End Get
		End Property
		Public Overrides Property EditValue() As Object
			Get
				Return MyBase.EditValue
			End Get
			Set(ByVal value As Object)
				If value IsNot DBNull.Value AndAlso value IsNot Nothing Then
					If Not Properties.Helper.LookupObjectType.IsInstanceOfType(value) Then
						If Properties.ThrowExceptionOnInvalidLookUpEditValueType Then
							Throw New InvalidCastException(SystemExceptionLocalizer.GetExceptionMessage(ExceptionId.UnableToCast, value.GetType(), Properties.Helper.LookupObjectType))
						Else
							MyBase.EditValue = Nothing
							Return
						End If
					End If
				End If
				MyBase.EditValue = value
			End Set
		End Property
		Public Function FindEditingObject() As Object
			Return BindingHelper.FindEditingObject(Me)
		End Function
		Private Sub OnEditingObjectChanged()
			If FindEditingObject() Is Nothing AndAlso EditValue IsNot Nothing Then
				EditValue = Nothing
			End If
		End Sub
		Private Sub DataBindings_CollectionChanged(ByVal sender As Object, ByVal e As CollectionChangeEventArgs)
			OnEditingObjectChanged()
		End Sub
		Public Shadows Sub UpdateDisplayText()
			MyBase.UpdateDisplayText()
			MyBase.Refresh()
		End Sub

		#Region "IGridInplaceEdit Members"
		Private ReadOnly Property IGridInplaceEdit_DataBindings() As ControlBindingsCollection Implements IGridInplaceEdit.DataBindings
			Get
				Return MyBase.DataBindings
			End Get
		End Property
		Private Property GridEditingObject() As Object Implements IGridInplaceEdit.GridEditingObject
			Get
				Return gridEditingObject_Renamed
			End Get
			Set(ByVal value As Object)
				If gridEditingObject_Renamed IsNot value Then
					gridEditingObject_Renamed = value
					OnEditingObjectChanged()
				End If
			End Set
		End Property
		#End Region
	End Class
End Namespace
