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
    
    public partial class ForumMessageTag
    {
        public long ForumMessageId { get; set; }
        public string Tag { get; set; }
    
        public virtual ForumMessage ForumMessage { get; set; }
    }
}
