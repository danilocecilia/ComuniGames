//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComuniGamesWeb.Models.DataContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cidade
    {
        public Cidade()
        {
            this.Endereco = new HashSet<Endereco>();
        }
    
        public int cod_cidade { get; set; }
        public short cod_estado { get; set; }
        public string nom_cidade { get; set; }
    
        public virtual Estado Estado { get; set; }
        public virtual ICollection<Endereco> Endereco { get; set; }
    }
}
