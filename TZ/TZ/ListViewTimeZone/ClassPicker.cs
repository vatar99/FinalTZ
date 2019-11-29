using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace TZ.ListViewTimeZone
{
    public class ClassPicker : INotifyPropertyChanged
    {
        public List<_keyTimeZone> timeZoneslist { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private _keyTimeZone _selectTimeZone {get; set;}
        public _keyTimeZone SelectTimeZone
        {
            get { return _selectTimeZone; }
            set
            {
                if (_selectTimeZone !=value)
                {
                    _selectTimeZone = value;
                    MyTimeZone = "Select TimeZome:" + _selectTimeZone.Value;

                }
            }
        }
        private string _myTimeZone;
        public string MyTimeZone
        {
            get { return _myTimeZone; }
            set
            {
                if (_myTimeZone != value)
                {
                    _myTimeZone = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClassPicker()
        {
            timeZoneslist = GetTimeZones().OrderBy(t => t.Value).ToList();
        }

        public List<_keyTimeZone> GetTimeZones()
        {
            var zone = new List<_keyTimeZone>()
            {
                new _keyTimeZone() {Key = 1, Value = "Московское время"},
                new _keyTimeZone() {Key = 2, Value = "Японское время"},
                new _keyTimeZone() {Key = 3, Value = "Тихоокеанское время"}

            };
            return zone;

        }
    }


    public class _keyTimeZone
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public int Time { get; set; }

    }
}
