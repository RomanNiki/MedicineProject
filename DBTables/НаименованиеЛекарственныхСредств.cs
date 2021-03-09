using System;
using System.Collections.Generic;

#nullable disable

namespace PredpriyatieProject
{
    public partial class НаименованиеЛекарственныхСредств
    {
        public НаименованиеЛекарственныхСредств()
        {
            Rofs = new HashSet<Rof>();
            ДокументыВещиs = new HashSet<ДокументыВещи>();
        }

        public int Id { get; set; }
        public int? Код { get; set; }
        public string НаименованиеЦенностей { get; set; }
        public int? ЕдиницаИзмерения { get; set; }

        public virtual ЕдиницыИзмерения ЕдиницаИзмеренияNavigation { get; set; }
        public virtual ICollection<Rof> Rofs { get; set; }
        public virtual ICollection<ДокументыВещи> ДокументыВещиs { get; set; }
    }
}
