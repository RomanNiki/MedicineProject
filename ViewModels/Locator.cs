using System;
using System.Collections.Generic;
using System.Text;


namespace PredpriyatieProject.ViewModels
{
    public class DataContainer : OnPropertyChangedClass
    {
        private string _text;
        private int _number;

        public string Text { get => _text; set => SetProperty(ref _text, value); }
        public int Number { get => _number; set => SetProperty(ref _number, value); }
    }
    public class DateTimeContainer : OnPropertyChangedClass
    {
        private DateTime _birthday;

        public DateTime Date { get => _birthday; set => SetProperty(ref _birthday, value); }
    }
    public class LocatorDynamic
    {
        public DataContainer Data { get; }
            = new DataContainer()
            {
                Text = "",
                Number = 0
            };

        public DateTimeContainer Dates { get; }
            = new DateTimeContainer()
            {
                Date = DateTime.Now
            };

        public FirstVM firstVM { get; set; } = new FirstVM { dateTimeEnd = DateTime.Today, dateTimeStart = DateTime.Today.AddDays(-7), TextForSearch =""};
        

    }

}
