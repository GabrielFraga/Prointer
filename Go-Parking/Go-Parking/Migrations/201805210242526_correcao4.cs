namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correcao4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reserva", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Reserva", "UserId");
            AddForeignKey("dbo.Reserva", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reserva", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Reserva", new[] { "UserId" });
            DropColumn("dbo.Reserva", "UserId");
        }
    }
}
