//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DiabetApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Diary_Product = new HashSet<Diary_Product>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<float> Glycemic_Index { get; set; }
        public Nullable<float> Calories { get; set; }
        public Nullable<float> Protein { get; set; }
        public Nullable<float> Fats { get; set; }
        public Nullable<float> Carbohydrates { get; set; }
        public Nullable<int> ID_Type_Product { get; set; }
        public Nullable<int> ID_Glycemic_Index { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Diary_Product> Diary_Product { get; set; }
        public virtual Glycemic_Index Glycemic_Index1 { get; set; }
        public virtual Type_Product Type_Product { get; set; }
    }
}
