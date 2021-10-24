using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensionsAsync;

namespace DPS926_A2.Models
{
    public class Genre
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
