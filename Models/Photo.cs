//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TraveloggiaWebApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Photo
    {
        public int PhotoID { get; set; }
        public string FileName { get; set; }
        public string Caption { get; set; }
        public Nullable<int> SiteID { get; set; }
        public Nullable<int> JournalID { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<System.DateTime> DateTaken { get; set; }
        public Nullable<bool> FromPhone { get; set; }
        public string StorageURL { get; set; }
        public string ThumbnailURL { get; set; }
    }
}