using System;
using System.Collections.Generic;

#nullable disable

namespace PredpriyatieProject
{
    public partial class ЕдиницыИзмерения
    {
        public ЕдиницыИзмерения()
        {
            НаименованиеЛекарственныхСредствs = new HashSet<НаименованиеЛекарственныхСредств>();
        }

        public int Код { get; set; }
        public string ЕдИзмерения { get; set; }

        public virtual ICollection<НаименованиеЛекарственныхСредств> НаименованиеЛекарственныхСредствs { get; set; }
    }
}
