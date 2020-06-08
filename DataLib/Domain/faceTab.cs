using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLib.Domain
{
    public class faceTab: IEntity
    {
        [Key]
        public int Id { get; set; }
        
        public string faceid { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime addtime { get; set; } = DateTime.Now.ToLocalTime();

    }
}
