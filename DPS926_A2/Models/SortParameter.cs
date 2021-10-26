using System;
using System.Collections.Generic;
using System.Text;

namespace DPS926_A2.Models
{
    class SortParameter
    {
        private string _name;
        public string name {
            get {
                return _name + (active ? " ↓" : " ↑");
            }
            set {
                if (_name != value) _name = value;
            }
        }

        public bool active { get; set; }
    }
}
