Imports System.Configuration

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Win
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.ExpressApp.Xpo

Namespace WinSolution.Win
	Friend NotInheritable Class Program
		''' <summary>
		''' The main entry point for the application.
		''' </summary>
		Private Sub New()
		End Sub
		<STAThread> _
		Shared Sub Main()
			Application.EnableVisualStyles()
			Application.SetCompatibleTextRenderingDefault(False)
			EditModelPermission.AlwaysGranted = Debugger.IsAttached
			Dim app As New WinSolutionWindowsFormsApplication()
			InMemoryDataStoreProvider.Register()
			app.ConnectionString = InMemoryDataStoreProvider.ConnectionString
			If ConfigurationManager.ConnectionStrings("ConnectionString") IsNot Nothing Then
				app.ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
			End If
			Try
				DevExpress.ExpressApp.Xpo.InMemoryDataStoreProvider.Register()
				app.ConnectionString = DevExpress.ExpressApp.Xpo.InMemoryDataStoreProvider.ConnectionString
				app.Setup()
				app.Start()
			Catch e As Exception
				app.HandleException(e)
			End Try
		End Sub
	End Class
End Namespace