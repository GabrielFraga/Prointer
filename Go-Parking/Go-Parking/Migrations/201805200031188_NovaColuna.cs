namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NovaColuna : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reserva", "FormaPagamento", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reserva", "FormaPagamento");
        }
    }
}
