using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Model
{
    public class Dose
    {
        public int ID
        {
            get;
            set;
        }

        public int DosagemId
        {
            get;
            set;
        }

        [Required]
        public DateTime DataHora
        {
            get;
            set;
        }

        public bool Administrada
        {
            get;
            set;
        }
    }
}
