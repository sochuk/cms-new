namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.M_SETTING")]
    public partial class M_SETTING
    {
        [Key]
        public decimal USER_ID { get; set; }

        public decimal? GRID_PAGESIZE { get; set; }

        public decimal? GRID_THEME { get; set; }

        public decimal? GRID_ZEBRACOLOR { get; set; }

        public decimal? GRID_WRAP_COLUMN { get; set; }

        public decimal? GRID_WRAP_CELL { get; set; }

        public decimal? GRID_SHOWFILTERROW { get; set; }

        public decimal? GRID_SHOWFILTERBAR { get; set; }

        public decimal? GRID_SELECTBYROW { get; set; }

        public decimal? GRID_FOCUSEROW { get; set; }

        public decimal? GRID_ELLIPSIS { get; set; }

        public decimal? GRID_SHOWFOOTER { get; set; }

        public decimal? GRID_RESPONSIVE { get; set; }

        public virtual M_USER M_USER { get; set; }
    }
}
