using System;
using System.Collections.Generic;
using System.Text;

namespace PredpriyatieProject.Tabels
{
 public  class StorageList
    {
       public StorageList()
        {

        }
       public StorageList(string naz, string edizm,string forma,int ostatok,string NomerIdataDok,int prihod,int rashod)
        {
            Название_ценности = naz;
            Единица_измерения = edizm;
            Форма_выпуска = forma;
            Остаток_на_начало = ostatok;
            Номер_и_Дата_Документа = NomerIdataDok;
            Приход = prihod;
            Расход = rashod;
            int ostnakonec = ostatok + prihod - rashod;
            Остаток_на_конец = ostnakonec;
        }
        public string Название_ценности { get; set; }
        public string Единица_измерения { get; set; }
        public string Форма_выпуска { get; set; }
        public int Остаток_на_начало { get; set; }
        public string  Номер_и_Дата_Документа { get; set; }
        public int Приход { get; set; }
        public int Расход { get; set; }
        public int Остаток_на_конец { get; set; }

    }
}
