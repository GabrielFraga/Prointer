namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColunaValor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reserva", "Valor", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reserva", "Valor");
        }
    }
}
