//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SuperKinoStudio.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Presidents
    {
        public int PresidentId { get; set; }
        public int StudioId { get; set; }
        public int UserId { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public byte[] Image { get; set; }
    
        public virtual Studios Studios { get; set; }
        public virtual Users Users { get; set; }
    }
}
