using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Model
{
    public class Prescricao
    {
        public int ID
        {
            get;
            set;
        }
        [Required]
        [StringLength(200)]
        public string Paciente
        {
            get;
            set;
        }
        [Required]
        public DateTime DataDeFim
        {
            get;
            set;
        }
        public bool liberada
        {
            get;
            set;
        }
        [Required]
        public virtual List<Dosagem> Dosagens
        {
            get;
            set;
        }
    }
}
