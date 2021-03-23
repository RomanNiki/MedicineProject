using DevExpress.Mvvm;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PredpriyatieProject.ViewModels
{
    public class AddChangeDocumentVM : OnPropertyChangedClass
    {
     public   class DocumentsDate
        {
            public DocumentsDate()
            {
            }
            public DocumentsDate(string name, DateTime date, int id,string type)
            {
                Name = name;
                Date = date;
                Id = id;
                this.type = type;
            }

            public string Name { get; set; }
            public DateTime Date { get; set; }
            public int Id { get; set; }
            public string type { get; set; }
        }
        MedicineContext datacontext { get; set; } = FirstVM.MedCont;
        ПриходРасход Doc { get; set; }
        private DocumentsDate _selecteditem;
         public  DocumentsDate selecteditem { get => _selecteditem; set {  _selecteditem=value; if (value != null) { NameOfDoc = value.Name; IdOfDoc = value.Id; IdTypeOfDoc = int.Parse(datacontext.ТипДокументацииs.Where(x => x.Название == value.type).Select(x => x.Id).FirstOrDefault().ToString()); } } }
        public string NameOfDoc { get; set; }
        public int IdOfDoc { get; set; }
        public List<string> TypeOfDoc { get; set; } 
        public int IdTypeOfDoc { get; set; } 
        //  public Enumerable DockList { get; set; } => FirstVM.MedCon;
        private DateTime _selectedMonthYear = DateTime.Today;
        public DateTime SelectedMonthYear { get => _selectedMonthYear; set { SetProperty(ref _selectedMonthYear, value); ResfrashTable(); } }
      //  LocatorDynamic Locator = new LocatorDynamic();
        private DateTime _selectedDateDoc;
        public DateTime SelectedDateDoc { get => _selectedDateDoc; set { SetProperty(ref _selectedDateDoc, value);} }
        public List<DocumentsDate> DocumentsList { get; set; }
        public AddChangeDocumentVM()
        {
            TypeOfDoc = datacontext.ТипДокументацииs.Select(x => x.Название).ToList();
            ResfrashTable();
            _selectedDateDoc = DateTime.Today;
        }
        public void ResfrashTable()
        {
            DocumentsList = new List<DocumentsDate>();
            DateTime dateTimeStart = new DateTime(SelectedMonthYear.Year, SelectedMonthYear.Month, 1);
            DateTime dateTimeend = new DateTime(SelectedMonthYear.Year, SelectedMonthYear.Month, DateTime.DaysInMonth(SelectedMonthYear.Year, SelectedMonthYear.Month));
            var a = datacontext.ПриходРасходs.Join(datacontext.ТипДокументацииs,x=>x.ТипДокумента,y=>y.Id,(x,y)=>  new{y, x}).Where(x => x.x.Дата >= dateTimeStart && x.x.Дата <= dateTimeend && x.x.Склад == FirstVM.kostile).Select(x => new { Name = x.x.Название, date = x.x.Дата, id = x.x.Id, type= x.y.Название });
            foreach (var e in a)
            {
                DocumentsList.Add(new DocumentsDate(e.Name, (DateTime)e.date, e.id, e.type));
            }

        }
        public ICommand Deletebutte
        {
            get
            {
                return new RelayCommand(() =>
                {
                  var fordelete=   datacontext.ПриходРасходs.Where(x => x.Id == selecteditem.Id).Select(x=>x).FirstOrDefault();
                    datacontext.ПриходРасходs.Remove(fordelete);
                    datacontext.SaveChanges();
                    ResfrashTable();
                });
            }
        }

        public ICommand ChangeButton
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var changeItem = datacontext.ПриходРасходs.Where(x => x.Id == selecteditem.Id).Select(x=>x).FirstOrDefault();
                    changeItem.Дата = SelectedDateDoc;
                    changeItem.Название = NameOfDoc;
                    changeItem.ТипДокумента = IdTypeOfDoc;
                    datacontext.ПриходРасходs.Update(changeItem);
                    datacontext.SaveChanges();
                });
            }
        }
        public static int podrazdelenie = 17;
        public ICommand AddButton
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ПриходРасход addItem = new ПриходРасход();
                    addItem.Дата = SelectedDateDoc;
                    addItem.Название = NameOfDoc;
                    addItem.Подразделение = podrazdelenie;
                    addItem.Склад = FirstVM.kostile;
                    addItem.ТипДокумента = IdTypeOfDoc;
                    var f = datacontext.ПриходРасходs.Where(x => x.Дата == addItem.Дата && x.Название == addItem.Название).Select(x => x).FirstOrDefault();
                    if (f==null)
                    {
                        datacontext.ПриходРасходs.Add(addItem);
                        datacontext.SaveChanges();
                    }
                    
                });
            }
        }
    }
}
