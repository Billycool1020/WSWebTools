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
    
    public partial class ForumMessageKeyword
    {
        public long ForumMessageId { get; set; }
        public int Type { get; set; }
        public string Keyword { get; set; }
    
        public virtual ForumMessage ForumMessage { get; set; }
    }
}
