using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLib.Domain
{
    public class faceUser:IEntity
    {
        [Key]
        public int Id { get; set; }

        public int faceTabId { get; set; }

        public string fuName { get; set; }

        public faceTab faceTab { get; set; }
    }
}
