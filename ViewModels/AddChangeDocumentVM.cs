using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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


        //  public Enumerable DockList { get; set; } => FirstVM.MedCon;
        private DateTime _selectedMonthYear = DateTime.Today;
        public DateTime SelectedMonthYear { get => _selectedMonthYear; set { SetProperty(ref _selectedMonthYear, value); ResfrashTable(); } }
        public List<DocumentsDate> DocumentsList { get; set; }
        public AddChangeDocumentVM()
        {
            ResfrashTable();
        }
        public void ResfrashTable()
        {
            DocumentsList = new List<DocumentsDate>();
            DateTime dateTimeStart = new DateTime(SelectedMonthYear.Year, SelectedMonthYear.Month, 1);
            DateTime dateTimeend = new DateTime(SelectedMonthYear.Year, SelectedMonthYear.Month, DateTime.DaysInMonth(SelectedMonthYear.Year, SelectedMonthYear.Month));
            var a = FirstVM.MedCont.ПриходРасходs.Join(FirstVM.MedCont.ТипДокументацииs,x=>x.ТипДокумента,y=>y.Id,(x,y)=>  new{y, x}).Where(x => x.x.Дата >= dateTimeStart && x.x.Дата <= dateTimeend && x.x.Склад == FirstVM.kostile).Select(x => new { Name = x.x.Название, date = x.x.Дата, id = x.x.Id, type= x.y.Название });
            foreach (var e in a)
            {
                DocumentsList.Add(new DocumentsDate(e.Name, (DateTime)e.date, e.id, e.type));
            }

        }
    }
}
