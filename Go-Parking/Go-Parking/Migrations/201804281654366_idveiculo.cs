namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idveiculo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vaga", "veiculo_Id", "dbo.Veiculo");
            DropIndex("dbo.Vaga", new[] { "veiculo_Id" });
            DropColumn("dbo.Vaga", "VeiculoId");
            RenameColumn(table: "dbo.Vaga", name: "veiculo_Id", newName: "VeiculoId");
            AlterColumn("dbo.Vaga", "VeiculoId", c => c.Int(nullable: false));
            AlterColumn("dbo.Vaga", "VeiculoId", c => c.Int(nullable: false));
            CreateIndex("dbo.Vaga", "VeiculoId");
            AddForeignKey("dbo.Vaga", "VeiculoId", "dbo.Veiculo", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vaga", "VeiculoId", "dbo.Veiculo");
            DropIndex("dbo.Vaga", new[] { "VeiculoId" });
            AlterColumn("dbo.Vaga", "VeiculoId", c => c.Int());
            AlterColumn("dbo.Vaga", "VeiculoId", c => c.String());
            RenameColumn(table: "dbo.Vaga", name: "VeiculoId", newName: "veiculo_Id");
            AddColumn("dbo.Vaga", "VeiculoId", c => c.String());
            CreateIndex("dbo.Vaga", "veiculo_Id");
            AddForeignKey("dbo.Vaga", "veiculo_Id", "dbo.Veiculo", "Id");
        }
    }
}
