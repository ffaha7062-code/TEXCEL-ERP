using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.Enums
{
    /// <summary>
    /// Represents the skill type of a textile/garment worker.
    /// </summary>
    public enum SkillType
    {
        [Display(Name = "Machine Operator")]
        MachineOperator = 1,

        [Display(Name = "Helper")]
        Helper = 2,

        [Display(Name = "Cutter")]
        Cutter = 3,

        [Display(Name = "Checker")]
        Checker = 4,

        [Display(Name = "Packer")]
        Packer = 5,

        [Display(Name = "Supervisor")]
        Supervisor = 6,

        [Display(Name = "Quality Inspector")]
        QualityInspector = 7,

        [Display(Name = "Sample Maker")]
        SampleMaker = 8,

        [Display(Name = "Iron Man")]
        IronMan = 9,

        [Display(Name = "Others")]
        Others = 99
    }
}