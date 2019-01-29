Imports System
Imports DevExpress.Utils
Imports DevExpress.Accessibility
Imports DevExpress.ExpressApp.Utils
Imports DevExpress.ExpressApp.Editors
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.ExpressApp.Win.Core
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.Registrator

Namespace Editors.Win
    Public Class RepositoryItemLookUpEditEx
        Inherits DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Implements ILookupEditRepositoryItem

        Friend Const EditorName As String = "LookUpEditEx"

        Private helper_Renamed As LookupEditorHelper
        Shared Sub New()
            Register()
        End Sub
        Public Sub New()
        End Sub
        Public Shared Sub Register()
            If Not EditorRegistrationInfo.Default.Editors.Contains(EditorName) Then
                EditorRegistrationInfo.Default.Editors.Add(New EditorClassInfo(EditorName, GetType(LookUpEditEx), GetType(RepositoryItemLookUpEditEx), GetType(LookUpEditViewInfo), New ButtonEditPainter(), True, EditImageIndexes.LookUpEdit, GetType(PopupEditAccessible)))
            End If
        End Sub
        Public Overrides ReadOnly Property EditorTypeName() As String
            Get
                Return EditorName
            End Get
        End Property
        Public Shadows ReadOnly Property OwnerEdit() As LookUpEditEx
            Get
                Return CType(MyBase.OwnerEdit, LookUpEditEx)
            End Get
        End Property
        Public ReadOnly Property Helper() As LookupEditorHelper
            Get
                Return helper_Renamed
            End Get
        End Property
        Public Sub Init(ByVal displayFormat As String, ByVal helper As LookupEditorHelper)
            Me.helper_Renamed = helper
            BeginUpdate()
            DisplayFormat.FormatString = displayFormat
            DisplayFormat.FormatType = FormatType.Custom
            EditFormat.FormatString = displayFormat
            EditFormat.FormatType = FormatType.Custom
            TextEditStyle = TextEditStyles.Standard
            ExportMode = ExportMode.DisplayText
            DisplayMember = DirectCast(Me, ILookupEditRepositoryItem).DisplayMember
            ValueMember = Nothing
            ShowHeader = False
            DropDownRows = helper.SmallCollectionItemCount
            SearchMode = SearchMode.AutoFilter
            NullText = CaptionHelper.NullValueText
            AllowNullInput = DefaultBoolean.True
            EndUpdate()
        End Sub
        Public Overrides Function GetDisplayText(ByVal format As FormatInfo, ByVal editValue As Object) As String
            Dim result As String = MyBase.GetDisplayText(format, editValue)
            If String.IsNullOrEmpty(result) AndAlso editValue IsNot Nothing AndAlso helper_Renamed IsNot Nothing Then
                result = helper_Renamed.GetDisplayText(editValue, NullText, format.FormatString)
            End If
            Return result
        End Function
        Public Overrides Sub Assign(ByVal item As RepositoryItem)
            Dim source As RepositoryItemLookUpEditEx = CType(item, RepositoryItemLookUpEditEx)
            Try
                MyBase.Assign(source)
            Catch
            End Try
            helper_Renamed = source.helper_Renamed
            ThrowExceptionOnInvalidLookUpEditValueType = source.ThrowExceptionOnInvalidLookUpEditValueType
        End Sub
        #Region "ILookupEditRepositoryItem Members"
        Private ReadOnly Property ILookupEditRepositoryItem_LookupObjectType() As Type Implements ILookupEditRepositoryItem.LookupObjectType
            Get
                Return helper_Renamed.LookupObjectType
            End Get
        End Property
        Private ReadOnly Property ILookupEditRepositoryItem_DisplayMember() As String Implements ILookupEditRepositoryItem.DisplayMember
            Get
                Return If(helper_Renamed.DisplayMember IsNot Nothing, helper_Renamed.DisplayMember.Name, String.Empty)
            End Get
        End Property
        #End Region
    End Class
End Namespace
