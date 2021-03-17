using DevExpress.Mvvm;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.EntityFrameworkCore;
using PredpriyatieProject.Tabels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PredpriyatieProject.ViewModels
{
  public  class FirstVM : OnPropertyChangedClass
    {

     
        private DateTime _dateTime;
        public DateTime dateTimeStart { get => _dateTime; set { SetProperty(ref _dateTime, value); RefreshTable.Execute(RefreshTable); } }  
        private DateTime _dateTimesecond;
        public DateTime dateTimeEnd { get => _dateTimesecond; set { SetProperty(ref _dateTimesecond, value); RefreshTable.Execute(RefreshTable); } }

        MedicineContext MedCont = new MedicineContext();
        public List<Приходвсего> PrihodVse { get; set; }
        public List<УходВсе> UhodVse { get; set; }

        public List<StorageList> GlList { get; set; }
        public List<StorageList> AmbulatoriList { get; set; }
        public List<StorageList> GorodList { get; set; }
        public List<StorageList> VolnoList { get; set; }
        public DelegateCommand ResultForDate { get; }
        public DelegateCommand OpenNewaddWindow { get; }
       
        public string TextForSearch { get => _textForSearch; set => SetProperty(ref _textForSearch, value); }
        private string _textForSearch;
         public ICommand RefreshTable
         {
             get { return new RelayCommand(() => {
                 GlList = new List<StorageList>();
                 MedCont.НаименованиеЛекарственныхСредствs.Load();
                 MedCont.ДокументыВещиs.Load();
                 MedCont.ПриходРасходs.Load();

                 var group = MedCont.НаименованиеЛекарственныхСредствs.Local.Join(MedCont.ДокументыВещиs.Local, x => x.Id, x => x.НаименованиеЦенности, (x, y) => new { doc = y.Документ, name = x.НаименованиеЦенностей, kol = y.Количество });
                 var gr2 = MedCont.ПриходРасходs.Local.Join(group, x => x.Id, x => x.doc, (x, y) => new { type = x.ТипДокумента, slad = x.Склад, name = y.name, kol = y.kol, date = x.Дата });
                 var filterstart = gr2.Where(x => (x.type == 1) && (x.slad == 4) ).AsEnumerable();
                 var grupedstart = filterstart.GroupBy(x => x.name, (x, y) => new { Name = x, Kol = y.Select(x => x.kol).Sum() });
                 var filterend = gr2.Where(x => (x.type == 2) && (x.slad == 4)).AsEnumerable();
                 var grupedend = filterend.GroupBy(x => x.name, (x, y) => new { Name = x, Kol = y.Select(x => x.kol).Sum() });
               
              
             } ); }
         }
        RelayCommand a = new RelayCommand(()=> { });
        public ICommand Addbutte
        {
            get
            {
                return new RelayCommand(() => {
                  
                });
            }
        }
      



        public FirstVM()
        {

            a.Execute(RefreshTable);
              

            OpenNewaddWindow = new DelegateCommand(() => { AddWind addWind = new AddWind(); addWind.ShowDialog();  });
            ResultForDate = new DelegateCommand(() => {  MessageBox.Show(dateTimeEnd.ToString()); });

        }

    }
}
