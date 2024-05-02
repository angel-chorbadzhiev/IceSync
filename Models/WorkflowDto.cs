using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IceSync.Models
{
    public class WorkflowDto
    {
        #region Properties

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public string MultiExecBehavior { get; set; } = string.Empty;

        #endregion
    }
}