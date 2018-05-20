namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OcupacaoVagas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vaga", "Ocupada", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vaga", "Ocupada");
        }
    }
}
