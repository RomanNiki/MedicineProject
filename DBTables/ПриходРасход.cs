using System;
using System.Collections.Generic;

#nullable disable

namespace PredpriyatieProject
{
    public partial class ПриходРасход
    {
        public ПриходРасход()
        {
            ДокументыВещиs = new HashSet<ДокументыВещи>();
        }

        public int Id { get; set; }
        public int? ТипДокумента { get; set; }
        public int? Склад { get; set; }
        public string Название { get; set; }
        public DateTime? Дата { get; set; }
        public int? Подразделение { get; set; }

        public virtual Бригады ПодразделениеNavigation { get; set; }
        public virtual Склады СкладNavigation { get; set; }
        public virtual ТипДокументации ТипДокументаNavigation { get; set; }
        public virtual ICollection<ДокументыВещи> ДокументыВещиs { get; set; }
    }
}
