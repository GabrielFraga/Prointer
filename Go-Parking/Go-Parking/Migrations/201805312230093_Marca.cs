namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Marca : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Veiculo", "Cor", c => c.String());
            AddColumn("dbo.Veiculo", "Marca", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Veiculo", "Marca");
            DropColumn("dbo.Veiculo", "Cor");
        }
    }
}
