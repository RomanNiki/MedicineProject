using DevExpress.Mvvm;
using Microsoft.EntityFrameworkCore;
using PredpriyatieProject.Tabels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace PredpriyatieProject.ViewModels
{
  public  class FirstVM : OnPropertyChangedClass
    {
       
        private DateTime _dateTime;
        public DateTime dateTime { get => _dateTime; set => SetProperty(ref _dateTime, value); }  
        private DateTime _dateTimesecond;
        public DateTime dateTimesecond { get => _dateTimesecond; set => SetProperty(ref _dateTimesecond, value); }

        MedicineContext MedicineContext = new MedicineContext();
        public List<Приходвсе> PrihodVse { get; set; }
        public List<УходВсе> UhodVse { get; set; }
        
        public List<StorageList> GlList { get; set; }
        public List<StorageList> AmbulatoriList { get; set; }
        public List<StorageList> GorodList { get; set; }
        public List<StorageList> VolnoList { get; set; }
        public DelegateCommand ResultForDate { get; }
        public DelegateCommand OpenNewaddWindow { get; }
       
        public string TextForSearch { get => _textForSearch; set => SetProperty(ref _textForSearch, value); }
        private string _textForSearch;

       public FirstVM()
        {
            OpenNewaddWindow = new DelegateCommand(() => { AddWind addWind = new AddWind(); addWind.ShowDialog(); });
            ResultForDate = new DelegateCommand(() => {  MessageBox.Show(dateTimesecond.ToString()); });
            int ostatok = 0;
            
            var a = MedicineContext.ДокументыВещиs.Include(u => u.ДокументNavigation).Include(u=>u.ДокументNavigation).Include(u=>u.НаименованиеЦенностиNavigation).ToList();
            var Edizmerenia = MedicineContext.НаименованиеЛекарственныхСредствs.Include(u => u.ЕдиницаИзмеренияNavigation);
            var prihod = MedicineContext.Приходвсеs.Include(u => u.НаименованиеЦенностей);
            var uhod = MedicineContext.УходВсеs.Include(u => u.НаименованиеЦенностей);
            if (a.Count>0)
            {


                foreach (var e in a)
                {
                    if (e != null)
                    {

                        try
                        {
                            var prihodkol = prihod.Where(u => u.НаименованиеЦенностей == e.НаименованиеЦенностиNavigation.НаименованиеЦенностей).Select(f => f.SumКоличество);
                            var uhodkol = uhod.Where(u => u.НаименованиеЦенностей == e.НаименованиеЦенностиNavigation.НаименованиеЦенностей).Select(f => f.SumКоличество);
                            var EdIzm = Edizmerenia.Where(u=>u.НаименованиеЦенностей== e.НаименованиеЦенностиNavigation.НаименованиеЦенностей).Select(f=>f.ЕдиницаИзмеренияNavigation.ЕдИзмерения);
                            string name = e.НаименованиеЦенностиNavigation.НаименованиеЦенностей;
                           
                            
                            string dateAndNameDOk =  (e.ДокументNavigation!= null)? e.ДокументNavigation.Дата + " " + e.ДокументNavigation.Название: "";


                            StorageList newItemOfStoraglist = new StorageList(
                               name,
                               EdIzm.FirstOrDefault(),
                                " " , 
                                ostatok,
                                dateAndNameDOk
                                ,
                                Convert.ToInt32(prihodkol), Convert.ToInt32(uhodkol));
                            GlList.Add(newItemOfStoraglist);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }

    }
}
