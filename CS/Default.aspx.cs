using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.XtraPivotGrid;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            fieldCompanyName.CollapseAll();
        if (ASPxPivotGrid2.JSProperties.ContainsKey("cpFilter"))
            ASPxPivotGrid2.JSProperties.Remove("cpFilter");
    }
    protected void ASPxPivotGrid2_GroupFilterChanged(object sender, PivotGroupEventArgs e) {
        var fi = FilterInfo.GetFilterInfo(e.Group, PivotFilterType.Included);
        string result = FilterInfo.WriterHierachyToString(fi, "");
        ASPxPivotGrid2.JSProperties.Add("cpFilter", result);
    }

    protected void ASPxPivotGrid2_CustomGroupInterval(object sender, PivotCustomGroupIntervalEventArgs e)
    {
        if (e.Field.UnboundFieldName != "fieldCompanyGroup") return;
        if (Convert.ToChar(e.Value.ToString()[0]) < 'F')
        {
            e.GroupValue = "A-E";
            return;
        }
        if (Convert.ToChar(e.Value.ToString()[0]) > 'E' && Convert.ToChar(e.Value.ToString()[0]) < 'P')
        {
            e.GroupValue = "F-O";
            return;
        }
        if (Convert.ToChar(e.Value.ToString()[0]) > 'O' && Convert.ToChar(e.Value.ToString()[0]) < 'T')
        {
            e.GroupValue = "P-S";
            return;
        }
        if (Convert.ToChar(e.Value.ToString()[0]) > 'S')
            e.GroupValue = "T-Z";
    }




}

public class FilterInfo
{
    PivotGridFieldBase Field { get; set; }
    object Value { get; set; }
    FilterInfo[] ChildValues { get; set; }

    public FilterInfo(){
        ChildValues = new FilterInfo[0];
    }
    public FilterInfo(PivotGroupFilterValue v)
    {
        this.Field = v.Field;
        this.Value = v.Value;
        if (v.ChildValues == null)
            this.ChildValues = new FilterInfo[0];
        else
            this.ChildValues = v.ChildValues.Select(cv => new FilterInfo(cv)).ToArray();
    }
    public override string ToString() {
        return string.Format("{0}: {1}({2})", this.Field, this.Value, ChildValues.Length);
    }
    public static FilterInfo[] GetFilterInfo(PivotGridGroup group, PivotFilterType targetType)
    {
        if (group.FilterValues.HasFilter)
            if (group.FilterValues.FilterType == targetType)
                return group.FilterValues.Values.Select(v => new FilterInfo(v)).ToArray();
            else
                return GetConvertedFilerInfo(group, group.FilterValues.Values, null);
        else
            return new FilterInfo[0];
    }

    private static FilterInfo[] GetConvertedFilerInfo(PivotGridGroup group, PivotGroupFilterValuesCollection filterValues, object[] parentValues) {
        if( !group.FilterValues.HasFilter || filterValues == null || filterValues.Count== 0 )
            return new FilterInfo[0];
        
        var currentField = filterValues.First().Field;
        var uniqueValues = group.GetUniqueValues(parentValues);
        var invertedFilterInfo = new List<FilterInfo>();
        foreach (object v in uniqueValues) {
            PivotGroupFilterValue filterValue = filterValues.FirstOrDefault(fv => Object.Equals(fv.Value, v));
            if (filterValue == null)
                invertedFilterInfo.Add(new FilterInfo() { Field = currentField, Value = v });
            else {
                if (filterValue.ChildValues != null && filterValue.ChildValues.Count > 0) {
                    invertedFilterInfo.Add(new FilterInfo() {
                        Field = currentField,
                        Value = v,
                        ChildValues = GetConvertedFilerInfo(group, filterValue.ChildValues, (parentValues ?? new object[0]).Union(new object[] { v }).ToArray())
                    });
                }

            }
        }
        return invertedFilterInfo.ToArray();
    }

    public static string WriterHierachyToString(FilterInfo[] info, string prefix) {
        StringBuilder sb = new StringBuilder();
        info.ToList().ForEach(fi => {
            sb.AppendLine(prefix + fi.Field.GetDisplayText(fi.Value));
            string nestedValues = WriterHierachyToString(fi.ChildValues, prefix + "...");
            if( !String.IsNullOrEmpty(nestedValues ))
                sb.Append(nestedValues);
        });
        return sb.ToString();
    }

}

