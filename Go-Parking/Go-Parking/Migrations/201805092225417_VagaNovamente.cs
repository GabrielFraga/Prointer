namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VagaNovamente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reserva", "Vaga_Id", c => c.Int());
            CreateIndex("dbo.Reserva", "Vaga_Id");
            AddForeignKey("dbo.Reserva", "Vaga_Id", "dbo.Vaga", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reserva", "Vaga_Id", "dbo.Vaga");
            DropIndex("dbo.Reserva", new[] { "Vaga_Id" });
            DropColumn("dbo.Reserva", "Vaga_Id");
        }
    }
}
