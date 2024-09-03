<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128594111/24.2.1%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E1101)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->

# XAF WinForms - How to use a custom Lookup Property Editor control for reference properties

> **Note**  
> The description of this example is currently under construction and may not match the code in the example. We are currently working to provide you with up-to-date content.

## Scenario

This example demonstrates a simple drop-down editor for reference properties. The editor provides the **Open**, **Add** and **Clear** buttons.

![Custom Lookup](./media/CustomLookup.png)

## Implementation Details

1. The _Editors.Win_ module implements a reusable `LookupPropertyEditorEx` class. This is an XAF Property Editor that can be used for reference properties instead of the standard `LookupPropertyEditor` in WinForms XAF applications.

2. The `LookUpEditEx` and `RepositoryItemLookUpEditEx` classes implement a control for this Property Editor. This control is inherited from the `LookUpEdit` class and extends it with additional features required for the Property Editor.

## Additional Information

1. From this example, you need use only one assembly (the _WinSolution_ from the example is just a demo application, and it has no relation to the solution): `Editors.Win.dll`.
   
   Since this is a regular XAF module, you should add this module to your application to be able to use its features. In case of standard modules, you add them from the Toolbox via the Application or Module designer. Since we deal with a custom module, you should add this into the Toolbox manually. For additional information, refer to the following article: [To add a tool to the toolbox](https://learn.microsoft.com/en-us/visualstudio/modeling/customizing-tools-and-the-toolbox?view=vs-2022#to-add-a-tool-to-the-toolbox).
   
   Alternatively, you can take the source code and include it in your solution.

2. The `LookupPropertyEditorEx` is set as default for all lookup properties in the application.
   
   If you want to change this, then invoke the Model Editor for the Windows Forms application project or module, and change the **EditorType** property of the **DetailViewItems** | **PropertyEditors** | **LookupProperty** node. Or change the **PropertyEditorType** property for a class member, List View column or Detail View item nodes individually.

## Files to Review (XPO)
* [LookupPropertyEditorEx.cs](./CS/XPO/Editors.Win/LookupPropertyEditorEx.cs) 
* [LookUpEditEx.cs](./CS/XPO/Editors.Win/LookUpEditEx.cs) 
* [RepositoryItemLookUpEditEx.cs](./CS/XPO/Editors.Win/RepositoryItemLookUpEditEx.cs) 

## Files to Review (EF Core)
* [LookupPropertyEditorEx.cs](./CS/EFCore/Editors.Win/LookupPropertyEditorEx.cs)
* [LookUpEditEx.cs](./CS/EFCore/Editors.Win/LookUpEditEx.cs)
* [RepositoryItemLookUpEditEx.cs](./CS/EFCore/Editors.Win/RepositoryItemLookUpEditEx.cs)


## Documentation

* [Implement Custom Property Editors](https://documentation.devexpress.com/eXpressAppFramework/113097/Concepts/UI-Construction/View-Items/Implement-Custom-Property-Editors)
* [PropertyEditors.Lookup - How to provide alternative data representations for reference lookup properties (e.g., a simple drop-down box, a complex multi-column grid, or a tree view)](https://www.devexpress.com/Support/Center/Question/Details/S92425/propertyeditors-lookup-how-to-provide-alternative-data-representations-for-reference)


<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=xaf-how-to-use-a-custom-lookup-property-editor-control-for-reference-properties-in-winforms&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=xaf-how-to-use-a-custom-lookup-property-editor-control-for-reference-properties-in-winforms&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
