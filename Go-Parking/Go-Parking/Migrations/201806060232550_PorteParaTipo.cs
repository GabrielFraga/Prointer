namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PorteParaTipo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Veiculo", "Tipo", c => c.String());
            AddColumn("dbo.Vaga", "Tipo", c => c.String());
            DropColumn("dbo.Veiculo", "Porte");
            DropColumn("dbo.Vaga", "Porte");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vaga", "Porte", c => c.String());
            AddColumn("dbo.Veiculo", "Porte", c => c.String());
            DropColumn("dbo.Vaga", "Tipo");
            DropColumn("dbo.Veiculo", "Tipo");
        }
    }
}
