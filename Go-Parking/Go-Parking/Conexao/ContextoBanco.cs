namespace Go_Parking.Conexao
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ContextoBanco : DbContext
    {
        public ContextoBanco()
            : base("name=ContextoBanco")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
