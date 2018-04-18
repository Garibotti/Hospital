using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Model
{
    public class Dosagem
    {
        public int ID
        {
            get;
            set;
        }
        public int PrescricaoId
        {
            get;
            set;
        }
        [Required]
        [StringLength(30)]
        public string ViaDeAdm
        {
            get;
            set;
        }
        public int Quantidade
        {
            get;
            set;
        }
        [Required]
        [StringLength(30)]
        public string UnidadeDeMedida
        {
            get;
            set;
        }
        public int Frequencia
        {
            get;
            set;
        }
        [Required]
        public DateTime HorarioDeInicio
        {
            get;
            set;
        }
        [Required]
        [StringLength(200)]
        public string Medicamento
        {
            get;
            set;
        }

        public virtual List<Dose> Dose
        {
            get;
            set;
        }
    }
}
