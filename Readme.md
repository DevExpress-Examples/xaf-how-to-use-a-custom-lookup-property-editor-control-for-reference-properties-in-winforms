# How to use a custom Lookup Property Editor control for reference properties in WinForms
![Custom Lookup](https://github.com/Kill4kan/XAF_how-to-use-a-custom-lookup-property-editor-control-for-reference-properties-in-winforms-e1101/blob/18.1.5%2B/media/CustomLookup.png)

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
[Implement Custom Property Editors](https://documentation.devexpress.com/eXpressAppFramework/113097/Concepts/UI-Construction/View-Items/Implement-Custom-Property-Editors)
<br>
[PropertyEditors.Lookup - How to provide alternative data representations for reference lookup properties (e.g., a simple drop-down box, a complex multi-column grid, or a tree view)](https://www.devexpress.com/Support/Center/Question/Details/S92425/propertyeditors-lookup-how-to-provide-alternative-data-representations-for-reference)


