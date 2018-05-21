namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correcao : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Vaga", "Ocupada");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vaga", "Ocupada", c => c.Boolean(nullable: false));
        }
    }
}
