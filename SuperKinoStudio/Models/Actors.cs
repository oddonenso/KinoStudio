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
    
    public partial class Actors
    {
        public int ActorId { get; set; }
        public int MovieId { get; set; }
        public int StudioId { get; set; }
        public int AreaId { get; set; }
        public string NameActor { get; set; }
        public string SurnameActor { get; set; }
        public string MidnameActor { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<int> SalaryActor { get; set; }
        public byte[] Image { get; set; }
    
        public virtual Area Area { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual Studios Studios { get; set; }
    }
}
