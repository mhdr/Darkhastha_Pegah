using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;

namespace Darkhast.Lib
{
    internal class MyLocalization : LocalizationManager
    {
        public override string GetStringOverride(string key)
        {
            switch (key)
            {
                case "GridViewAlwaysVisibleNewRow":
                    return "برای اضافه کردن آیتم جدید اینجا کلیک  کنید";
                case "GridViewClearFilter":
                    return "فیلتر را پاک کن";
                case "GridViewFilter":
                    return "فیلتر کن";
                case "GridViewFilterAnd":
                    return "و";
                case "GridViewFilterContains":
                    return "شامل می شود";
                case "GridViewFilterDoesNotContain":
                    return "شامل نمی شود";
                case "GridViewFilterEndsWith":
                    return "تمام می شود با";
                case "GridViewFilterIsContainedIn":
                    return "قرار دارد در";
                case "GridViewFilterIsEqualTo":
                    return "برابر است با";
                case "GridViewFilterIsGreaterThan":
                    return "بزرگتر است از";
                case "GridViewFilterIsGreaterThanOrEqualTo":
                    return "بزرگتر یا مساوی است با";
                case "GridViewFilterIsLessThan":
                    return "کوچکتر است از";
                case "GridViewFilterIsLessThanOrEqualTo":
                    return "کوچکتر مساوی است با";
                case "GridViewFilterIsNotContainedIn":
                    return "قرار ندارد در";
                case "GridViewFilterIsNotEqualTo":
                    return "مساوی نیست با";
                case "GridViewFilterMatchCase":
                    return "یکسان باشد";
                case "GridViewFilterOr":
                    return "یا";
                case "GridViewFilterSelectAll":
                    return "همه را انتخاب کن";
                case "GridViewFilterShowRowsWithValueThat":
                    return "ردیف هایی را نشان بده که";
                case "GridViewFilterStartsWith":
                    return "شروع می شود با";
                case "GridViewGroupPanelText":
                    return "هدر یک ستون را بکشید و اینجا رها کنید تا بر اساس آن ستون گروه بندی شود";
                case "GridViewGroupPanelTopText":
                    return "هدر گروه";
                case "GridViewGroupPanelTopTextGrouped":
                    return "گروه بندی شده بر اساس:";
                    
                case "RadDataPagerOf":
                    return "از";
                case "RadDataPagerPage":
                    return "صفحه";

            }
            return base.GetStringOverride(key);
        }
    }
}
