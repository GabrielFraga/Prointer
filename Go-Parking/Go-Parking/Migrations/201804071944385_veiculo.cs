namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class veiculo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoleViewModel",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Veiculo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Modelo = c.String(),
                        placa = c.String(),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Veiculo", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Veiculo", new[] { "UserId" });
            DropTable("dbo.Veiculo");
            DropTable("dbo.RoleViewModel");
        }
    }
}
