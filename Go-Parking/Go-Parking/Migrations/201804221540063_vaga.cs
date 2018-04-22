namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vaga : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vaga",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Modelo = c.String(),
                        placa = c.String(),
                        VeiculoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Veiculo", t => t.VeiculoId, cascadeDelete: true)
                .Index(t => t.VeiculoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vaga", "VeiculoId", "dbo.Veiculo");
            DropIndex("dbo.Vaga", new[] { "VeiculoId" });
            DropTable("dbo.Vaga");
        }
    }
}
