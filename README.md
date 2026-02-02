<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1313893)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->

# Blazor Grid – How to Implement Custom Column Chooser with Sorting, Search, and Select All Capabilities

This example implements a custom Column Chooser dialog for the [DevExpress Blazor Grid](https://docs.devexpress.com/Blazor/403143/components/grid) component. The dialog displays alphabetically sorted Grid columns and allows users to change column visibility. It also includes select/deselect all and search features (useful when working with a large number of columns).

![Custom Column Chooser for DevExpress Blazor Grid](images/custom-column-chooser.png)

Unlike the custom dialog, a built-in Column Chooser displays columns in the same order as the Grid and allows users to reorder them. Use buttons above the Grid component to open custom and built-in Column Chooser dialogs and compare their functionality.

## Implementation Details

The custom Column Chooser dialog displays Grid columns using the [DevExpress Blazor List Box](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxListBox-2) component. The List Box retrieves an `IGridColumn` collection from the Grid and excludes columns whose [ShowInColumnChooser](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGridColumn.ShowInColumnChooser) property is set to `false`. The editor operates in multi-selection mode and synchronizes selected and visible Grid columns:

```razor
<DxListBox Data="@AllColumns"
           ShowSearchBox="true"
           SelectionMode="ListBoxSelectionMode.Multiple"
           ShowCheckboxes="true"
           ShowSelectAllCheckbox="true"
           TextFieldName="Caption"
           Values="VisibleColumns"
           ValuesChanged="@((IEnumerable<IGridColumn> values) => SelectedDataItemsChanged(values))">
</DxListBox>
```

```csharp
private void InitializeColumnList() {
    AllColumns = MyGrid.GetColumns().Where(i => i.ShowInColumnChooser).OrderBy(i => i, ColumnsComparerImpl.Default).ToList();
    if (ReverseOrder)
        AllColumns = AllColumns.Reverse();
    VisibleColumns = MyGrid.GetVisibleColumns();
}
void SelectedDataItemsChanged(IEnumerable<IGridColumn> values) {
    VisibleColumns = values;
    UpdateColumnsVisibility();
}
void UpdateColumnsVisibility() {
    MyGrid.BeginUpdate();
    var columnCollection = MyGrid.GetColumns();
    foreach (var column in columnCollection)
        if (column.ShowInColumnChooser)
            column.Visible = VisibleColumns.Contains(column);
    MyGrid.EndUpdate();
}
```

The List Box sorts items in the following order:

1) Selection column
2) Command column
3) Data columns, sorted alphabetically by caption/field name

```csharp
class ColumnsComparerImpl : IComparer<IGridColumn> {
    public static IComparer<IGridColumn> Default { get; } = new ColumnsComparerImpl();
    ColumnsComparerImpl() { }
    int IComparer<IGridColumn>.Compare(IGridColumn x, IGridColumn y) {
        if (x is IGridSelectionColumn)
            return -1;
        if (x is IGridCommandColumn && y is IGridDataColumn)
            return -1;
        if (x is IGridDataColumn xData && y is IGridDataColumn yData) {
            var xName = !string.IsNullOrEmpty(xData.Caption) ? xData.Caption : xData.FieldName;
            var yName = !string.IsNullOrEmpty(yData.Caption) ? yData.Caption : yData.FieldName;
            return string.Compare(xName, yName);
        }
        return 0;
    }
}
```

A [CheckBox editor](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxCheckBox-1) below the List Box allows users to reverse the item sort order dynamically:

```razor
<DxCheckBox Checked="ReverseOrder" CheckedChanged="@((bool value) => OnReverseOrder(value))">
    Reverse Order
</DxCheckBox>
```

```csharp
private void OnReverseOrder(bool newValue) {
    ReverseOrder = newValue;
    AllColumns = AllColumns.Reverse();
}
```

## Files to Review

- [Index.razor](CS/Components/Pages/Index.razor)
- [site.css](CS/wwwroot/css/site.css)

## Documentation

- [Blazor Grid – Column Chooser](https://docs.devexpress.com/Blazor/404479/components/grid/columns/columns#column-chooser)
- [Blazor List Box – Item Selection](https://docs.devexpress.com/Blazor/405403/components/data-editors/listbox/item-selection)

<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=draft-Custom-Column-Chooser&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=draft-Custom-Column-Chooser&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
