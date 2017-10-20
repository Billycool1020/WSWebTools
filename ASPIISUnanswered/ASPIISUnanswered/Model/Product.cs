//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASPIISUnanswered.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.ASPIISForums = new HashSet<ASPIISForum>();
            this.FollowUpThreads = new HashSet<FollowUpThread>();
            this.MSDNForums = new HashSet<MSDNForum>();
        }
    
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string ProductName { get; set; }
        public int SubTeamId { get; set; }
        public double OPVAR { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASPIISForum> ASPIISForums { get; set; }
        public virtual Engineer Engineer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FollowUpThread> FollowUpThreads { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MSDNForum> MSDNForums { get; set; }
        public virtual Team Team { get; set; }
    }
}
