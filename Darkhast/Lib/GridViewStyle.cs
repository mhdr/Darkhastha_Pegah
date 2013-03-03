using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Darkhast.Lib
{
    public class GridViewStyle : StyleSelector
    {

        public Style InTrashStyle { get; set; }
        public Style NormalRowStyle { get; set; }
        public Style TaeedShodeStyle { get; set; }
        public Style TaeedNashodeStyle { get; set; }
        public Style DarHaleBarresiStyle { get; set; }

        public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container)
        {
            Darkhastha2 currentItem = (Darkhastha2)item;

            if (currentItem.IsTrash == (int)Trash.InTrash && Properties.Settings.Default.ApplyInTrashStyle)
            {
                return InTrashStyle;
            }
            else if (currentItem.Vaziat == (int)VaziatDarkhast.TaeedShode && Properties.Settings.Default.ApplyTaeedShodeStyle)
            {
                return TaeedShodeStyle;
            }
            else if (currentItem.Vaziat == (int)VaziatDarkhast.TaeedNashode && Properties.Settings.Default.ApplyTaeedNashodeStyle)
            {
                return TaeedNashodeStyle;
            }
            else if (currentItem.Vaziat==(int) VaziatDarkhast.DarHaleBarresi && Properties.Settings.Default.ApplyDarHaleBarresiStyle)
            {
                return DarHaleBarresiStyle;
            }

            return NormalRowStyle;
        }
    }
}
