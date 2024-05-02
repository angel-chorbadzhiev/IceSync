using System.ComponentModel.DataAnnotations;

namespace IceSync.Models
{
    /// <summary>
    /// Workflow data model.
    /// </summary>
    public class Workflow
    {
        #region Properties

        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Workflow Id")]
        public int WorkflowId { get; set; }
        [Required]
        [Display(Name = "Workflow Name")]
        public string? WorkflowName { get; set; }
        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [Required]
        [Display(Name = "Is Running")]
        public bool IsRunning{ get; set; }
        [Required]
        [Display(Name = "Multi Exec Behavior")]
        public string MultiExecBehavior { get; set; } = string.Empty;

        #endregion
    }
}