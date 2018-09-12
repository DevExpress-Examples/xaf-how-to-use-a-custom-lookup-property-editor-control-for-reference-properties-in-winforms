# How to use a custom Lookup Property Editor control for reference properties in WinForms
![alt text](http://downloads.devexpress.com/Share/XAF/e1101.png "Logo Title Text 1")

### FEATURES
LookupPropertyEditorEx
This is a regular XAF PropertyEditor class that can be used for referenced properties instead of the standard LookupPropertyEditor in Windows Forms applications.
By default, this editor allows you to choose a value from the drop down list. It's possible to find the required record in the drop down list by typing text (auto-incremental search).
If you want to clear a value, use either the Control+Delete keystroke or click the minus (-) button.
If you want to add a new object, click the plus (+) button. It will show a modal DetailView allowing you to fill object properties and save it.

### IMPORTANT NOTES
1. From this example, you need use only one assembly (the WinSolution from the example is just a demo application, and it has no relation to the solution): Editors.Win.dll
Since this is a regular XAF module, you should add this module to your application to be able to use its features. In case of standard modules, you add them from the Toolbox via the Application or Module designer. Since we deal with a custom module, you should add this into the Toolbox manually. Please refer to the How to: Add Items to the Toolbox article in MSDN, for more details.
Alternatively, you can take the source code and include it in your solution.

2. The LookupPropertyEditorEx is set as default for all lookup properties in the application.
If you want to change this, then invoke the Model Editor for the Windows Forms application project or module, and change the EditorType property of the DetailViewItems | PropertyEditors | LookupProperty node. Or, change the PropertyEditorType property for a class member, ListView column or DetailView item nodes individually.

#### See Also:
Implement Custom Property Editors
PropertyEditors.Lookup - Provide the capability to work with referenced properties via a simple drop down list


