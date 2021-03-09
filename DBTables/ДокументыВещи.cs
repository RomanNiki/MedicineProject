using System;
using System.Collections.Generic;

#nullable disable

namespace PredpriyatieProject
{
    public partial class ДокументыВещи
    {
        public int Id { get; set; }
        public int? Документ { get; set; }
        public int? НаименованиеЦенности { get; set; }
        public int? Количество { get; set; }

        public virtual ПриходРасход ДокументNavigation { get; set; }
        public virtual НаименованиеЛекарственныхСредств НаименованиеЦенностиNavigation { get; set; }
    }
}
