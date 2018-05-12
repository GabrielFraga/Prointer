namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ALteraçõesEmReserva : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reserva", "Vaga_Id", "dbo.Vaga");
            DropIndex("dbo.Reserva", new[] { "Vaga_Id" });
            DropColumn("dbo.Reserva", "Vaga_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reserva", "Vaga_Id", c => c.Int());
            CreateIndex("dbo.Reserva", "Vaga_Id");
            AddForeignKey("dbo.Reserva", "Vaga_Id", "dbo.Vaga", "Id");
        }
    }
}
