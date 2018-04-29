namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteracaoIDVaga : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vaga", "VeiculoId", "dbo.Veiculo");
            DropIndex("dbo.Vaga", new[] { "VeiculoId" });
            AddColumn("dbo.Vaga", "veiculo_Id", c => c.Int());
            AlterColumn("dbo.Vaga", "VeiculoId", c => c.String());
            CreateIndex("dbo.Vaga", "veiculo_Id");
            AddForeignKey("dbo.Vaga", "veiculo_Id", "dbo.Veiculo", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vaga", "veiculo_Id", "dbo.Veiculo");
            DropIndex("dbo.Vaga", new[] { "veiculo_Id" });
            AlterColumn("dbo.Vaga", "VeiculoId", c => c.Int(nullable: false));
            DropColumn("dbo.Vaga", "veiculo_Id");
            CreateIndex("dbo.Vaga", "VeiculoId");
            AddForeignKey("dbo.Vaga", "VeiculoId", "dbo.Veiculo", "Id", cascadeDelete: true);
        }
    }
}
