//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WSWebTool.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ForumUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ForumUser()
        {
            this.ForumMessages = new HashSet<ForumMessage>();
        }
    
        public string Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string ProfilePictureUri { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ForumMessage> ForumMessages { get; set; }
    }
}
