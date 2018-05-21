namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correcao2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Veiculo", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Veiculo", new[] { "UserId" });
            AddColumn("dbo.Veiculo", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Veiculo", "UserId", c => c.String());
            CreateIndex("dbo.Veiculo", "ApplicationUser_Id");
            AddForeignKey("dbo.Veiculo", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Veiculo", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Veiculo", new[] { "ApplicationUser_Id" });
            AlterColumn("dbo.Veiculo", "UserId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Veiculo", "ApplicationUser_Id");
            CreateIndex("dbo.Veiculo", "UserId");
            AddForeignKey("dbo.Veiculo", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
