//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GolfClubMLD.Models.EFRepository
{
    using System;
    using System.Collections.Generic;
    
    public partial class RentDetails
    {
        public int id { get; set; }
        public int equipId { get; set; }
        public int rentId { get; set; }
        public int amount { get; set; }
    
        public virtual Equipment Equipment { get; set; }
        public virtual Rent Rent { get; set; }
    }
}
