//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GaptWebsite.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Lecturer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Lecturer()
        {
            this.StudyUnits = new HashSet<StudyUnit>();
        }
    
        public int LecturerID { get; set; }
        public string LecturerName { get; set; }
        public string LecturerEmail { get; set; }
        public string LecturerRoom { get; set; }
        public string LecturerJob { get; set; }
        public string Course { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudyUnit> StudyUnits { get; set; }
        public virtual Cours Cours { get; set; }
    }
}
