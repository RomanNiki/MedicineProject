using System;
using System.Collections.Generic;

#nullable disable

namespace PredpriyatieProject
{
    public partial class ТипДокументации
    {
        public ТипДокументации()
        {
            ПриходРасходs = new HashSet<ПриходРасход>();
        }

        public int Id { get; set; }
        public string Название { get; set; }

        public virtual ICollection<ПриходРасход> ПриходРасходs { get; set; }
    }
}
