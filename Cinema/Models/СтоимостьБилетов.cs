//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cinema.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class СтоимостьБилетов
    {
        public int ID { get; set; }
        public Nullable<int> IDСеанса { get; set; }
        public Nullable<decimal> Стоимость { get; set; }
    
        public virtual Сеансы Сеансы { get; set; }
    }
}
