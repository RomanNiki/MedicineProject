using System;
using System.Collections.Generic;

#nullable disable

namespace PredpriyatieProject
{
    public partial class Rof
    {
        public int Id { get; set; }
        public int? Ценность { get; set; }
        public int? МинимальноеКолВо { get; set; }
        public int? МаксимальноеКолВо { get; set; }

        public virtual НаименованиеЛекарственныхСредств ЦенностьNavigation { get; set; }
    }
}
